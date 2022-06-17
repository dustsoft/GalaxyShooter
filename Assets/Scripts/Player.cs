using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _playerSpeed = 5f;
    [SerializeField] GameObject _laserPrefab;



    void Start()
    {

    }


    void Update()
    {
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        }
    }

    void PlayerMovement()
    {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.Translate(movement * _playerSpeed * Time.deltaTime);

        //Clamp X
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -5.15f, 5.15f), transform.position.y, transform.position.z);
        //Clamp Y
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.78f, 4.25f), transform.position.z);

    }
}
