using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyB : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject _enemyObject;
    [SerializeField] GameObject _enemyLaserPrefab;
    [SerializeField] GameObject _explosionObject;
    [SerializeField] GameObject _laserImpactVFX;
    [SerializeField] BoxCollider2D _enemyHitBox;
    [SerializeField] float _fireRate = 1f;
    [SerializeField] int _enemyHitPoints = 5;
    [SerializeField] int _hitFlashes;
    [SerializeField] Player _player;
    [SerializeField] AudioClip _explosionSFX;
    [SerializeField] AudioSource _audioSource;
    float _canFire = -1;
    public bool _enemyIsDead = false;
    [SerializeField] SpawnManager _spawnManager;

    // Enemy Movement
    [SerializeField] float _frequency = 1.0f;
    [SerializeField] float _amplitude = 5.0f;
    [SerializeField] float _cycleSpeed = 1.0f;

    Vector3 pos;
    Vector3 axis;

    private void Start()
    {
        #region COMPONENTS
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spriteRenderer = _enemyObject.GetComponent<SpriteRenderer>();
        #endregion

        pos = transform.position;
        axis = transform.right;


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
        EnemyMovement();

        EnemyShooting();
    }

    void EnemyMovement()
    {
        pos = pos + Vector3.down * Time.deltaTime * _cycleSpeed;
        transform.position = pos + axis * Mathf.Sin(Time.time * _frequency) * _amplitude;
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
        StopCoroutine(EnemyShootingRoutine());
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

    IEnumerator EnemyShootingRoutine()
    {
        //UPDATE USINGN WHILE LOOP

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
