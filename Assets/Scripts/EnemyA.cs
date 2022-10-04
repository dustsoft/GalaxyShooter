using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyA : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject _enemyObject;
    [SerializeField] GameObject _enemyLaserPrefab;
    [SerializeField] GameObject _explosionObject;
    [SerializeField] GameObject _laserImpactVFX;
    [SerializeField] BoxCollider2D _enemyHitBox;
    [SerializeField] float _fireRate = 3f;
    [SerializeField] int _enemyHitPoints = 5;
    [SerializeField] int _hitFlashes;
    [SerializeField] Player _player;
    [SerializeField] AudioClip _explosionSFX;
    [SerializeField] AudioSource _audioSource;
    float _canFire = -1;
    public bool _enemyIsDead = false;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spriteRenderer = _enemyObject.GetComponent<SpriteRenderer>();


        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is null on Enemy A");
        }
        else
        {
            _audioSource.clip = _explosionSFX;
        }
    }

    void Update()
    {
        //_spriteRenderer = _enemyObject.GetComponent<SpriteRenderer>();
        EnemyMovement();
        EnemyShooting();

        if (_enemyIsDead == true)
        {
            //Debug.Log("enemy dead");
        }

    }

    void EnemyMovement()
    {
        transform.Translate(Vector3.down * Time.deltaTime * 2.75f);

        if (transform.position.y < -6.5f)
        {
            Destroy(this.gameObject);
        }
    }

    void EnemyShooting()
    {
        StartCoroutine(EnemyShootingRoutine());

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

            if (_laserImpactVFX != null)
            {
                _laserImpactVFX.SetActive(false);
            }

            yield return null;
            yield return null;
            howManyFlashes--;
        }
    }

    IEnumerator EnemyExplosion()
    {
        _enemyIsDead = true;
        yield return null;
        _explosionObject.SetActive(true);
        _enemyObject.SetActive(false);
        yield return null;
        _audioSource.Play();
        _enemyHitBox.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.75f);
        yield return null;
        GameObject laser = _enemyLaserPrefab;
        laser.GetComponent<Laser>().EnemyDestroyed();
        yield return null;
        yield return null;
        yield return null;
        Destroy(this.gameObject);
    }

    IEnumerator EnemyShootingRoutine()
    {
        if (Time.time > _canFire && _enemyIsDead == false)
        {
            _canFire = Time.time + _fireRate;

            yield return new WaitForSeconds(1f);
            var offset = new Vector3(0, -0.75f, 0);
            GameObject laser = Instantiate(_enemyLaserPrefab, transform.position + offset, Quaternion.identity);
            laser.GetComponent<Laser>().AssignEnemyLaser();
            yield return new WaitForSeconds(0.25f);
            laser = Instantiate(_enemyLaserPrefab, transform.position + offset, Quaternion.identity);
            laser.GetComponent<Laser>().AssignEnemyLaser();
            yield return new WaitForSeconds(0.25f);
            laser = Instantiate(_enemyLaserPrefab, transform.position + offset, Quaternion.identity);
            laser.GetComponent<Laser>().AssignEnemyLaser();
        }

    }
    #endregion
}
