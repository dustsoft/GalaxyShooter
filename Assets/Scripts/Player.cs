using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _playerSpeed = 5f;
    [SerializeField] GameObject _laserPrefab;
    [SerializeField] float _fireRate = 2f;
    float _canFire = -1f;


    void Start()
    {

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
        _canFire = Time.time + _fireRate * 0.085f;
        Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
    }

    void PlayerMovement()
    {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.Translate(movement * _playerSpeed * Time.deltaTime);
    }

    void ScreenClamp()
    {
        //Clamp X
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -5.15f, 5.15f), transform.position.y, transform.position.z);
        //Clamp Y
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.78f, 4.25f), transform.position.z);
    }


}
