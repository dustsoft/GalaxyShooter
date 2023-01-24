using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    [SerializeField] int _powerUpID; // 0 = Laser PowerUp, 1 = Shield PowerUp, 2 = 1UP PowerUp, 3 = Neg PowerDown
    [SerializeField] AudioClip _soundClip;


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

            AudioSource.PlayClipAtPoint(_soundClip, transform.position);

            if (player != null)
            {
                switch(_powerUpID)
                {
                    case 0:
                        player.LaserPowerUp();
                        break;
                    case 1:
                        player.ShieldPowerUp();
                        break;
                    case 2:
                        player.ExtraLivePowerUp();
                        break;
                    case 3:
                        player.NegPowerdown();
                        break;
                }
            }

            Destroy(this.gameObject);
        }
    }
}
