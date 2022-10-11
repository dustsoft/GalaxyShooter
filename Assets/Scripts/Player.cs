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

    [SerializeField] int _lives = 3;

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
    #endregion

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSFX;
        }

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }
    }

    void Update()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        ScreenClamp();

        #region BUTTON & KEY INPUTS

        // Player Movement Input
        PlayerMovement();

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
        if (_canPlay == true && _gameOver == false) // Player can only move ship if canPlay is true.
        {
            _animator.SetBool("xIsIdle", true);

            Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            transform.Translate(movement * _playerSpeed * Time.deltaTime);

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
        if (_playerDeathRoutine == true && _canPlay == true) // Invincibility period after losing a life
        {
            return;
        }

        if (_shieldPowerupActive == true) // One Free Hit! via Shield Powerup
        {
            _shieldPowerupActive = false;
            _shieldGraphicPrefab.SetActive(false);
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
            _uiManager.GameOverSequnce();
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
        if (_shieldPowerupActive == false)
        {
            _shieldPowerupActive = true;
            _shieldGraphicPrefab.SetActive(true);
        }
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
