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

    [SerializeField] int _shieldHP;

    [Tooltip("The player laser fire rate.")]
    [SerializeField] float _fireRate = 1.5f;

    [SerializeField] int _scoreValue = 0;
    [SerializeField] int _highScoreValue;
    float _canFire = -1f;

    [Header ("Powerups")]
    [Tooltip("Power Level 0 is the default laser power.")]
    [SerializeField] int _powerLevel = 0;

    int _laserAmmo;
    [SerializeField] int _maxAmmo;
    [SerializeField] int _ammoRefillPowerup = 25;

    [Tooltip("Drag laser Prefabs here used for laser powerup levels.")]
    [SerializeField] GameObject[] _laserPrefabs;
    [SerializeField] GameObject[] _focusLaserPrefabs;

    [Tooltip("Drag shield Prefabs here for shields powerup VFX.")]
    [SerializeField] GameObject _shieldGraphicPrefab;

    [Header ("Game Objects & Components")]
    public GameObject _playerGraphic;
    public GameObject _explosionPrefab;
    public Animator _animator;

    [Header ("Scripts")]
    GameManager _gameManager;
    SpawnManager _spawnManager;
    UIManager _uiManager;
    FocusBar _focusBar;
    CameraShake _cameraShake;

    [Header("Audio FX")]
    [SerializeField] AudioClip _laserSFX;
    [SerializeField] AudioClip _emptySFX;
    [SerializeField] AudioClip _explosionSFX;
    [SerializeField] AudioSource _audioSource;

    bool _shieldPowerupActive = false;
    bool _gameOver = false;
    bool _playerDeathRoutine = false;
    bool _canPlay = true;
    bool _hitBox;
    bool _gameIsPaused = false;
    public bool _focusMode = false;
    public bool _energyEmpty = false; 
    #endregion

    void Start()
    {
        _laserAmmo = _maxAmmo;

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is NULL!");
        }
        else
        {
            _audioSource.clip = _laserSFX;
        }

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL!");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL!");
        }

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.Log("The Game Manager is NULL!");
        }

        _focusBar = GameObject.Find("Focus Mode").GetComponent<FocusBar>();
        if (_focusBar == null)
        {
            Debug.Log("The Focus Bar is NULL!");
        }

        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        if (_cameraShake == null)
        {
            Debug.Log("The Camera Shake is NULL!");
        }
    }

    void Update()
    {
        ScreenClamp();
        LaserAmmo();
        FocusMode();
        PlayerMovement();

        #region BUTTON & KEY INPUTS

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

    public void FocusMode()
    {
        if (_focusBar.current < 1)
        {
            _energyEmpty = true;
            _uiManager.ChangeFillBarColorRed();
        }

        if (_energyEmpty == true)
        {
            if (_focusBar.current == 100)
            {
                _uiManager.ChangeFillBarColorBlue();
                _energyEmpty = false;
            }
            else
            {
                _focusBar.EmptyRefillBar();
            }
        }

        if (Input.GetKey(KeyCode.LeftShift) && _energyEmpty == false)
        {
            _focusMode = true;
        }
        else
        {
            _focusMode = false;
        }
    }

    void FireLaser()
    {

        if (_laserAmmo > 0)
        {
            // Normal Mode
            if (_focusMode == false)
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
                }
            }

            // Focus Mode
            if (_focusMode == true)
            {
                switch (_powerLevel)
                {
                    case 0:
                        _canFire = Time.time + _fireRate * 0.085f;
                        Instantiate(_focusLaserPrefabs[0], transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                        break;
                    case 1:
                        _canFire = Time.time + _fireRate * 0.085f;
                        Instantiate(_focusLaserPrefabs[1], transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                        break;
                    case 2:
                        _canFire = Time.time + _fireRate * 0.085f;
                        Instantiate(_focusLaserPrefabs[2], transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                        break;
                    case 3:
                        _canFire = Time.time + _fireRate * 0.085f;
                        Instantiate(_focusLaserPrefabs[3], transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                        break;
                }
            }

            _audioSource.clip = _laserSFX;

            _audioSource.pitch = Random.Range(0.9f, 1.1f);

            _audioSource.Play();
            _laserAmmo = _laserAmmo - 1;
            _uiManager.UpdateAmmo(_laserAmmo);
        }

        else if (_laserAmmo == 0)
        {
            _audioSource.clip = _emptySFX;
            _audioSource.Play();
        }

    }

    void PlayerMovement()
    {
        if (_canPlay == true && _gameOver == false)
        {
            _animator.SetBool("xIsIdle", true);

            Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            // NORMAL MOVE SPEED - DEFAULT
            if (_focusMode == false)
            {
                transform.Translate(movement * _playerSpeed * Time.deltaTime);
            }

            // FOCUS MOVE SPEED
            if (_focusMode == true)
            {
                _focusBar.AdjustFocusBar();
                transform.Translate(movement * _playerFocusSpeed * Time.deltaTime);
            }
            else if (_energyEmpty == false)
            {
                _focusBar.RefillBar();
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

        _audioSource.clip = _explosionSFX;
        _audioSource.Play();
        _cameraShake.CameraShakeMethod();

        _focusBar.current = 100;

        _hitBox = GetComponent<CircleCollider2D>().enabled = false;
        _explosionPrefab.SetActive(true);
        _playerGraphic.SetActive(false);
        _lives--;
        _uiManager.UpdateLives(_lives);
        _playerDeathRoutine = true;
        StartCoroutine(PlayerDeathRoutine());

        // Game Over when losing all lives
        if (_lives < 0) 
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

    public void LaserAmmo()
    {
        if (_laserAmmo < 0)
        {
            _laserAmmo = 0;
        }

        if (_laserAmmo > _maxAmmo)
        {
            _laserAmmo = _maxAmmo;
        }
    }

    public void LaserPowerUp()
    {
        if (_laserAmmo > 0)
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
        else
        {
            _powerLevel = 0;
            AddScore();
        }

        _laserAmmo = _laserAmmo + _ammoRefillPowerup;
        LaserAmmo();
        _uiManager.UpdateAmmo(_laserAmmo);
    }

    public void ShieldPowerUp()
    {
        _shieldPowerupActive = true;
        _shieldHP = 3;
        _uiManager.UpdateShieldsUI(_shieldHP);
        _shieldGraphicPrefab.SetActive(true);
    }

    public void ExtraLivePowerUp()
    {
        if (_lives >= 0 && _lives != 3)
        {
            _lives = _lives + 1;
            _uiManager.UpdateLives(_lives);
        }
        else 
        {
            AddScore();
        }
    }

    public void NegPowerdown()
    {
        _focusBar.current = 0;
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
            _laserAmmo = _maxAmmo;
            LaserAmmo();
            _uiManager.UpdateAmmo(_laserAmmo);

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
