using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyA : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject _enemyObject;
    [SerializeField] GameObject _explosionObject;
    [SerializeField] GameObject _laserImpactVFX;
    [SerializeField] BoxCollider2D _enemyHitBox;
    [SerializeField] int _enemyHitPoints = 5;
    [SerializeField] int _hitFlashes;

    [SerializeField] Player _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        _spriteRenderer = _enemyObject.GetComponent<SpriteRenderer>();

        transform.Translate(Vector3.down * Time.deltaTime * 5);

        if (transform.position.y < -6.5f)
        {
            Destroy(this.gameObject);
            //float randomX = Random.Range(-5.25f, 5.25f);
            //transform.position = new Vector3(randomX, 6.5f, 0f);
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

            StartCoroutine(EnemyExplosion());
        }
        #endregion

        #region Laser Collision
        if (other.tag == "Laser") // Laser Collides with Enemy (Really the only no player object in the game).
        {                         // May need to change later on.
            Destroy(other.gameObject);

            if (_spriteRenderer != null)
            {
                EnemyHitFlash();
            }

            _enemyHitPoints--;

            if (_enemyHitPoints < 1)
            {
                if (_player != null)
                {
                    _player.AddScore();
                }

                StartCoroutine(EnemyExplosion());
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

        while (howManyFlashes > 0 && _laserImpactVFX != null)
        {
            _spriteRenderer.color = new Color(255, 0, 0);
            _laserImpactVFX.SetActive(true);
            yield return null;
            _spriteRenderer.color = new Color(255, 255, 255);
            yield return new WaitForSeconds(.015f);
            _laserImpactVFX.SetActive(false);
            yield return null;
            yield return null;
            howManyFlashes--;
        }
    }

    IEnumerator EnemyExplosion()
    {
        yield return null;
        _explosionObject.SetActive(true);
        _enemyObject.SetActive(false);
        _enemyHitBox.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.55f);
        yield return null;
        Destroy(this.gameObject);
    }
    #endregion
}
