using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float _bulletSpeed;

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * _bulletSpeed * 5);

        if (transform.position.y > 8f)
        {
            Destroy(this.gameObject);
        }
    }
}
