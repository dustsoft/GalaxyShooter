using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyA : MonoBehaviour
{

    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * 2);

        if (transform.position.y < -6.5f)
        {
            float randomX = Random.Range(-5.25f, 5.25f);
            transform.position = new Vector3(randomX, 6.5f, 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player !=null)
            {
                player.Damage();
            }


            Destroy(this.gameObject);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

    }
}
