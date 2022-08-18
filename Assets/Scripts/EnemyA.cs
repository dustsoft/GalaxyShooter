using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyA : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject _enemyObject;
    [SerializeField] int _enemyHitPoints = 5;
    [SerializeField] int _hitFlashes;

    void Update()
    {
        _spriteRenderer = _enemyObject.GetComponent<SpriteRenderer>(); //Need to null check

        //Moves the enemy down the screen
        //Will need to change later when the enemy needs to be more agile for gameplay enhancement
        transform.Translate(Vector3.down * Time.deltaTime * 5);
        if (transform.position.y < -6.5f)
        {
            float randomX = Random.Range(-5.25f, 5.25f);
            transform.position = new Vector3(randomX, 6.5f, 0f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        #region Player Collision
        if (other.tag == "Player") // Player Collision
        {
            Player player = other.transform.GetComponent<Player>();

            if (player !=null)
            {
                player.Damage();
            }

            Destroy(this.gameObject);
        }
        #endregion

        #region Laser Collision
        if (other.tag == "Laser") // Laser Collides with Enemy (Really the only no player object in the game).
        {                         // May beed to change later on.
            Destroy(other.gameObject);
            EnemyHitFlash();
            _enemyHitPoints--;

            if (_enemyHitPoints < 1)
            {
                Destroy(this.gameObject);
            }
        }
        #endregion
    }


    void EnemyHitFlash()
    {
        StartCoroutine(EnemyHitFlashing());
    }

    #region Coroutines
    IEnumerator EnemyHitFlashing()
    {
        int howManyFlashes = _hitFlashes;

        while (howManyFlashes > 0)
        {
            _spriteRenderer.color = new Color(255, 0, 0);
            yield return null;
            _spriteRenderer.color = new Color(255, 255, 255);
            yield return null;
            howManyFlashes--;
        }
    }

    #endregion
}
