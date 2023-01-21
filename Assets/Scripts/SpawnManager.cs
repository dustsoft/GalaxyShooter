using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemyContainer;
    //[SerializeField] float _enemySpawnRate = 4.75f;
    [SerializeField] GameObject[] _powerUps;

    public int waveNumber = 1;

    public int enemyDestroyedCount;

    public int _enemyMoveSetID;
    bool _stopSpawning = false;
    public bool startWaveTwo;

    void Start()
    {
        //StartCoroutine(EnemySpawnRoutine1());
        StartCoroutine(EnemySpawnRoutine2());
        StartCoroutine(PowerUpSpawnRoutine());
    }

    IEnumerator EnemySpawnRoutine1()
    {
        while (_stopSpawning == false)
        {
            float _enemySpawnRate = 4.75f;
            _enemyMoveSetID = Random.Range(0,3);

            switch (_enemyMoveSetID)
            {
                case 0: // Move Down
                    Vector3 posToSpawn = new Vector3(Random.Range(-5.19f, 5.19f), 7f, 0f);
                    GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                    yield return new WaitForSeconds(_enemySpawnRate);
                    break;
                case 1: // Move Left
                    Vector3 posToSpawn1 = new Vector3(-6.5f, Random.Range(4f, 1f), 0f);
                    GameObject newEnemy1 = Instantiate(_enemyPrefab, posToSpawn1, Quaternion.identity);
                    newEnemy1.transform.parent = _enemyContainer.transform;
                    yield return new WaitForSeconds(_enemySpawnRate);
                    break;
                case 2: // Move Right
                    Vector3 posToSpawn2 = new Vector3(6.5f, Random.Range(4f, 1f), 0f);
                    GameObject newEnemy2 = Instantiate(_enemyPrefab, posToSpawn2, Quaternion.identity);
                    newEnemy2.transform.parent = _enemyContainer.transform;
                    yield return new WaitForSeconds(_enemySpawnRate);
                    break;
            }
        }
    }

    IEnumerator EnemySpawnRoutine2()
    {
        while (_stopSpawning == false)
        {
            float _enemySpawnRate = 2.25f;
            _enemyMoveSetID = Random.Range(0, 3);

            switch (_enemyMoveSetID)
            {
                case 0: // Move Down
                    Vector3 posToSpawn = new Vector3(Random.Range(-5.19f, 5.19f), 7f, 0f);
                    GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                    yield return new WaitForSeconds(_enemySpawnRate);
                    break;
                case 1: // Move Left
                    Vector3 posToSpawn1 = new Vector3(-6.5f, Random.Range(4f, 1f), 0f);
                    GameObject newEnemy1 = Instantiate(_enemyPrefab, posToSpawn1, Quaternion.identity);
                    newEnemy1.transform.parent = _enemyContainer.transform;
                    yield return new WaitForSeconds(_enemySpawnRate);
                    break;
                case 2: // Move Right
                    Vector3 posToSpawn2 = new Vector3(6.5f, Random.Range(4f, 1f), 0f);
                    GameObject newEnemy2 = Instantiate(_enemyPrefab, posToSpawn2, Quaternion.identity);
                    newEnemy2.transform.parent = _enemyContainer.transform;
                    yield return new WaitForSeconds(_enemySpawnRate);
                    break;
            }
        }
    }


    IEnumerator PowerUpSpawnRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-5.19f, 5.19f), 7f, 0f);

            int _randomPowerUp = Random.Range(0, 3);

            Instantiate(_powerUps[_randomPowerUp], posToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(5, 15));
        }
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    void WaveSystem()
    {

    }


}
