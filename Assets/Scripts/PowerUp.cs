using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    [SerializeField] int _powerUpID; // 1 = Laser PowerUp 2 = Shield PowerUp

    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * 1.5f * _speed);

        if (transform.position.y < -6.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                switch(_powerUpID)
                {
                    case 0:
                        //player.LaserPowerUp01Active();
                        player.LaserPowerUp();
                        break;
                    case 1:
                        //player.ShieldPowerUp();
                        Debug.Log("Shield Powerup!");
                        break;
                }
            }

            Destroy(this.gameObject);
        }
    }
}
