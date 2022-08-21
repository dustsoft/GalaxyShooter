using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _playerSpeed = 5f;
    [SerializeField] int _lives = 3;
    [SerializeField] int _powerLevel = 0;
    [SerializeField] GameObject[] _laserPrefabs;
    //[SerializeField] GameObject _laserPrefab;
    //[SerializeField] GameObject _laserPowerUp01Prefab;
    [SerializeField] float _fireRate = 2f;
    //bool _isLaserPowerUp01Active = false;
    float _canFire = -1f;
    SpawnManager _spawnManager;
    public Animator _animator;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }
    }

    void Update()
    {
        PlayerMovement();
        ScreenClamp();

        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
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

    void ScreenClamp()
    {
        //Clamp X
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -5.15f, 5.15f), transform.position.y, transform.position.z);
        //Clamp Y
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.78f, 4.25f), transform.position.z);
    }

    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
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
            //add score
            Debug.Log("Add Score!");
        }
    }

    public void LaserPowerUp01Active()
    {
        //_isLaserPowerUp01Active = true;
        StartCoroutine(PowerUpCoolDown());
    }

    IEnumerator PowerUpCoolDown()
    {
        yield return new WaitForSeconds(10.0f);
        //_isLaserPowerUp01Active = false;
    }

}
