using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyA : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * 2);

        if (transform.position.y < -6.5f)
        {
            float randomX = Random.Range(-5.25f, 5.25f);
            transform.position = new Vector3(randomX, 6.5f, 0f);
        }
    }
}
