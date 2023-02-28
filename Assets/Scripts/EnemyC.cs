using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : MonoBehaviour
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
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _target;

    float _minDistance = 4f;

    float _canFire = -1;
    bool _enemyIsDead = false;

    [SerializeField] SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spriteRenderer = _enemyObject.GetComponent<SpriteRenderer>();

        _rb = GetComponent<Rigidbody2D>();
        _target = GameObject.Find("Player").transform;

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is null on Enemy C");
        }
        else
        {
            _audioSource.clip = _explosionSFX;
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(_target.position, _rb.position);

        EnemyMovement();
    }

    void RamInPlayerDirection()
    {
        Debug.Log("RAM PLAYER!");
    }

    void EnemyMovement()
    {
        transform.Translate(Vector3.down * Time.deltaTime * 2f);

        if (transform.position.y < -6.5f)
        {
            Destroy(this.gameObject);
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
        if (other.tag == "Laser")
        {                         
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
        _spawnManager.enemyDestroyedCount = _spawnManager.enemyDestroyedCount + 1;
        yield return null;
        _explosionObject.SetActive(true);
        _enemyObject.SetActive(false);
        yield return null;

        _audioSource.pitch = Random.Range(0.8f, 1.08f);
        _audioSource.Play();

        _enemyHitBox.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.75f);
        yield return null;
        Destroy(this.gameObject);
    }

    #endregion
}
