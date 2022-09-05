using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header ("Components")]
    public Animator _animator;

    [Header ("Powerups")]
    public bool _laserPowerupActive = false;
    [SerializeField] int _powerLevel = 0;
    [SerializeField] GameObject[] _laserPrefabs;
    [SerializeField] GameObject _shieldGraphicPrefab;
    public bool _shieldPowerupActive = false;

    [Header ("Player Info")]
    [SerializeField] float _playerSpeed = 5f;
    [SerializeField] int _lives = 3;
    [SerializeField] float _fireRate = 2f;
    [SerializeField] int _scoreValue = 0;

    float _canFire = -1f;
    SpawnManager _spawnManager;
    UIManager _uiManager;

    public GameObject _playerGraphic;
    public GameObject _explosionPrefab;

    bool _playerDeathRoutine = false;
    bool _canPlay = true;

    #region UNUSED VARIABLES
    //[SerializeField] GameObject _laserPrefab;
    //[SerializeField] GameObject _laserPowerUp01Prefab;
    //bool _isLaserPowerUp01Active = false;
    #endregion


    void Start()
    {
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
    }

    void Update()
    {
        PlayerMovement();
        ScreenClamp();

        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire && _canPlay == true)
        {
            FireLaser();
        }
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
        }



        //if (_isLaserPowerUp01Active)
        //{
          //  _canFire = Time.time + _fireRate * 0.085f;
            //Instantiate(_laserPowerUp01Prefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        //}
        //else
        //{
          //  _canFire = Time.time + _fireRate * 0.085f;
            //Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        //}
    }

    void PlayerMovement()
    {
        if (_canPlay == true)
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
        if (_playerDeathRoutine == true)
        {
            return;
        }

        if (_shieldPowerupActive == true) // One Free Hit! via Shield Powerup
        {
            _shieldPowerupActive = false;
            _shieldGraphicPrefab.SetActive(false);
            return;
        }

        _explosionPrefab.SetActive(true);
        _playerGraphic.SetActive(false);
        _lives--;
        _uiManager.UpdateLives(_lives);
        _playerDeathRoutine = true;
        StartCoroutine(PlayerDeathRoutine());

        if (_lives < 0)
        {
            _explosionPrefab.SetActive(true);
            _playerGraphic.SetActive(false);
            _playerDeathRoutine = true;
            StartCoroutine(PlayerGameOverRoutine());
        }
    }

    public void LaserPowerUp()
    {
        _laserPowerupActive = true;
        if (_powerLevel < 3)
        {
            _powerLevel++;
        }
        else
        {
            //add score
            Debug.Log("Add Score!");
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
        _scoreValue += 50;
        _uiManager.UpdateScore(_scoreValue);
    }

    IEnumerator PlayerDeathRoutine()
    {
        while (_playerDeathRoutine == true)
        {
            _canPlay = false;
            yield return new WaitForSeconds(1.5f);
            transform.position = new Vector3(0, -4, 0);
            _explosionPrefab.SetActive(false);
            _playerGraphic.SetActive(true);
            _powerLevel = 0;
            _canPlay = true;
            _shieldGraphicPrefab.SetActive(true);
            yield return new WaitForSeconds(1f);
            _shieldGraphicPrefab.SetActive(false);
            _playerDeathRoutine = false;
        }
    }

    IEnumerator PlayerGameOverRoutine()
    {
        while (_playerDeathRoutine == true)
        {
            _canPlay = false;
            yield return new WaitForSeconds(1.5f);
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
            _playerDeathRoutine = false;
        }

    }

    #region UNUSED CODE
    public void LaserPowerUp01Active() // not in use rn
    {
        //_isLaserPowerUp01Active = true;
        StartCoroutine(PowerUpCoolDown());
    }

    IEnumerator PowerUpCoolDown()  // not in use rn
    {
        yield return new WaitForSeconds(10.0f);
        //_isLaserPowerUp01Active = false;
    }
    #endregion
}
 