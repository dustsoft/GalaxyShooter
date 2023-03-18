using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    [SerializeField] int _powerUpID; // 0 = Laser PowerUp, 1 = Shield PowerUp, 2 = 1UP PowerUp, 3 = Neg PowerDown
    [SerializeField] AudioClip _soundClip;

    Player _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        ItemMagnet();

        transform.Translate(Vector3.down * Time.deltaTime * 1.5f * _speed);

        if (transform.position.y < -6.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void ItemMagnet()
    {
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, 2f * Time.deltaTime);
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
