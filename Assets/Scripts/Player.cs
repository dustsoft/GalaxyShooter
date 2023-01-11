using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region VARIABLES

    [Header("Player Info")]
    [Tooltip("How fast the player's movement speed is.")]
    [SerializeField] float _playerSpeed = 5f;
    [Tooltip("How fast the player's focus speed is.")]
    [SerializeField] float _playerFocusSpeed = 2f;

    [SerializeField] int _lives = 3;
    public int _shieldHP;

    [Tooltip("The player laser fire rate.")]
    [SerializeField] float _fireRate = 1.5f;

    [SerializeField] int _scoreValue = 0;
    [SerializeField] int _highScoreValue;
    float _canFire = -1f;

    [Header ("Powerups")]
    [Tooltip("Power Level 0 is the default laser power.")]
    [SerializeField] int _powerLevel = 0;

    [Tooltip("Drag laser Prefabs here used for laser powerup levels.")]
    [SerializeField] GameObject[] _laserPrefabs;

    [Tooltip("Drag shield Prefabs here for shields powerup VFX.")]
    [SerializeField] GameObject _shieldGraphicPrefab;

    [Header ("Game Objects & Components")]
    public GameObject _playerGraphic;
    public GameObject _explosionPrefab;
    public Animator _animator;

    GameManager _gameManager;
    SpawnManager _spawnManager;
    UIManager _uiManager;

    [Header("Audio FX")]
    [SerializeField] AudioClip _laserSFX;
    [SerializeField] AudioSource _audioSource;

    bool _shieldPowerupActive = false;
    bool _gameOver = false;
    bool _playerDeathRoutine = false;
    bool _canPlay = true;
    bool _hitBox;
    bool _gameIsPaused = false;
    bool _focusMode = false;
    #endregion

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSFX;
        }

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void Update()
    {
        ScreenClamp();

        #region BUTTON & KEY INPUTS

        // Player Movement Input
        PlayerMovement();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _focusMode = true;
        }
        else
        {
            _focusMode = false;
        }

        // Player Shooting Input
        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire && _canPlay == true && _gameOver == false)
        {
            FireLaser();
        }

        // Player Game Over Input
        if (Input.GetKeyDown(KeyCode.R) && _gameOver == true)
        {
            Time.timeScale = 1f;
            _canPlay = true;
            _gameOver = false;
            PlayerPrefs.SetInt("_highScoreValue", _scoreValue);
            _gameManager.RestartLevel();

        }

        // Pause Game Input
        if (Input.GetKeyDown(KeyCode.Escape) && _gameIsPaused == false)
        {
            _canPlay = false;
            _gameIsPaused = true;
            _uiManager.PauseMenu();
            _gameManager.PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _gameIsPaused == true)
        {
            _uiManager.UnPauseMenu();
            _gameManager.UnPauseGame();
            _canPlay = true;
            _gameIsPaused = false;
     
        }

        if (_gameIsPaused == true && Input.GetKeyDown(KeyCode.Q))
        {
            _gameManager.QuitGame();
        }

        #endregion
    }

    void FireLaser()
    {
        switch (_powerLevel)
        {
            case 0:
                _canFire = Time.time + _fireRate * 0.085f;
                Instantiate(_laserPrefabs[0], transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                break;
            case 1:
                _canFire = Time.time + _fireRate * 0.085f;
                Instantiate(_laserPrefabs[1], transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                break;
            case 2:
                _canFire = Time.time + _fireRate * 0.085f;
                Instantiate(_laserPrefabs[2], transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                break;
            case 3:
                _canFire = Time.time + _fireRate * 0.085f;
                Instantiate(_laserPrefabs[3], transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                break;
        } // Switch is used for different laser powerup levels.

        _audioSource.Play();

    }

    void PlayerMovement()
    {
        // Player can only move ship if canPlay is true.
        if (_canPlay == true && _gameOver == false)
        {
            _animator.SetBool("xIsIdle", true);

            Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (_focusMode == false)
            {
                transform.Translate(movement * _playerSpeed * Time.deltaTime);
            }
            else if (_focusMode == true)
            {
                transform.Translate(movement * _playerFocusSpeed * Time.deltaTime);
            }


            _animator.SetFloat("xMovement", Input.GetAxisRaw("Horizontal"));

            if (Input.GetAxisRaw("Horizontal") != 0f)
            {
                _animator.SetBool("xIsIdle", false);
            }
            else
            {
                _animator.SetBool("xIsIdle", true);
            }
        }
    }

    void ScreenClamp()
    {
        //Clamp X
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -5.15f, 5.15f), transform.position.y, transform.position.z);
        //Clamp Y
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.78f, 4.25f), transform.position.z);
    }

    public void Damage()
    {
        if (_playerDeathRoutine == true && _canPlay == true)
        {
            return;
        }

        if (_shieldPowerupActive == true && _shieldHP > 1)
        {
            _shieldHP = _shieldHP - 1;
            _uiManager.UpdateShieldsUI(_shieldHP);
            return;
        }

        if (_shieldPowerupActive == true && _shieldHP == 1)
        {
            _shieldHP = _shieldHP - 1;
            _shieldGraphicPrefab.SetActive(false);
            _uiManager.UpdateShieldsUI(_shieldHP);
            _shieldPowerupActive = false;
            return;
        }

        _canPlay = false;
        _hitBox = GetComponent<CircleCollider2D>().enabled = false;
        _explosionPrefab.SetActive(true);
        _playerGraphic.SetActive(false);
        _lives--;
        _uiManager.UpdateLives(_lives);
        _playerDeathRoutine = true;
        StartCoroutine(PlayerDeathRoutine());

        if (_lives < 0) // Game Over when losing all lives
        {
            _canPlay = false;
            _gameOver = true;
            _spawnManager.OnPlayerDeath();
            Time.timeScale = 0.25f;
            _explosionPrefab.SetActive(true);
            _playerGraphic.SetActive(false);
            _uiManager.GameOverSequence();
            _playerDeathRoutine = true;
            StartCoroutine(PlayerGameOverRoutine());

        }
    }

    public void LaserPowerUp()
    {
        if (_powerLevel < 3)
        {
            _powerLevel++;
        }
        else
        {
            AddScore();
        }
    }

    public void ShieldPowerUp()
    {
        _shieldPowerupActive = true;
        _shieldHP = 3;
        _uiManager.UpdateShieldsUI(_shieldHP);
        _shieldGraphicPrefab.SetActive(true);

        //if (_shieldPowerupActive == false)
        //{
        //}
    }

    public void AddScore()
    {
        _scoreValue += 50; // May need to use variable in future for point scaling

        if (_scoreValue > _highScoreValue) // Highscore System doesn't work yet.
        {
            PlayerPrefs.SetInt("_highScoreValue", _scoreValue);
        }

        _highScoreValue = PlayerPrefs.GetInt("_highScoreValue");

        _uiManager.UpdateScore(_scoreValue, _highScoreValue);

    }

    #region COROUTINES
    IEnumerator PlayerDeathRoutine() // Gameplay Death Routine
    {
        while (_playerDeathRoutine == true && _gameOver == false && _lives > -1)
        {
            yield return new WaitForSeconds(1.25f);
            transform.position = new Vector3(0, -4, 0);
            _explosionPrefab.SetActive(false);
            _powerLevel = 0;
            _playerGraphic.SetActive(true);
            _canPlay = true;
            _shieldGraphicPrefab.SetActive(true);
            yield return new WaitForSeconds(1f);
            _hitBox = GetComponent<CircleCollider2D>().enabled = true;
            _shieldGraphicPrefab.SetActive(false);
            _playerDeathRoutine = false;
        }
    }

    IEnumerator PlayerGameOverRoutine() // Game Over Routine
    {
        while (_playerDeathRoutine == true && _gameOver == true)
        {
            yield return new WaitForSeconds(0.25f);
            Time.timeScale = 1f;
            _playerDeathRoutine = false;
        }

    }
    #endregion

}
