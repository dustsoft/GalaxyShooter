using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float _bulletSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * _bulletSpeed * 7);
        if (transform.position.y > 8f)
        {
            Destroy(this.gameObject);
        }
    }
}
