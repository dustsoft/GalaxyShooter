using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float _bulletSpeed;
    bool _isEnemyLaser = false;
    bool _backAttack = false;


    void Update()
    {
        if (_backAttack == true)
            FireLaserAtPlayer();

        else if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }

    }

    void MoveUp() // Laser for the Player
    {
        transform.Translate(Vector3.up * Time.deltaTime * _bulletSpeed * 1.5f);

        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    void MoveDown()  // Laser for the Enemy
    {
        transform.Translate(Vector3.down * Time.deltaTime * 10f);

        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
            }
        }
        else if (other.tag == "Player" && _backAttack == true)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
            }
        }

        if (other.tag == "Item" && _isEnemyLaser == true)
        {
            Destroy(GameObject.FindWithTag("Item"));
            Destroy(this.gameObject);
        }
    }

    public void AssignBackAttack()
    {
        _backAttack = true;
    }

    public void FireLaserAtPlayer()
    {
        transform.Translate(Vector3.up * Time.deltaTime * 5);

        if (transform.position.y > 8)
        {
            Destroy(this.gameObject);
        }
    }

}
