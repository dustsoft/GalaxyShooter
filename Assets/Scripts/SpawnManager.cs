using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] float _enemySpawnRate = 4.75f;
    [SerializeField] GameObject[] _powerUps;
    bool _stopSpawning = false;



    void Start()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerUpSpawnRoutine());
    }

    IEnumerator EnemySpawnRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-5.19f, 5.19f), 7f, 0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnRate);
        }
    }

    IEnumerator PowerUpSpawnRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-5.19f, 5.19f), 7f, 0f);

            int _randomPowerUp = Random.Range(0, 2);

            Instantiate(_powerUps[_randomPowerUp], posToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(5, 15));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
