using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region VARIABLES
    [Header("Enemy/MOB Info")]
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemyPrefab2;
    [SerializeField] GameObject _enemyPrefab3;
    [SerializeField] GameObject _enemyContainer;
    [HideInInspector] public int _enemyMoveSetID;

    [Header("Wave System Info")]
    public int waveNumber = 1;
    public int enemyDestroyedCount;
    [SerializeField] float _enemySpawnRate;

    [Header("Powerup Info")]
    [SerializeField] GameObject[] _powerUps; // 0 = tripleshot 1 = Shields 2 = 1UP 3 = NegItem

    bool _stopSpawning = false;
    int _randomPowerUp;


    UIManager _uiManager;

    #endregion

    #region METHODS/FUNCTIONS
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerUpSpawnRoutine());
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    void WaveSystem()
    {
        if (enemyDestroyedCount > 9)
        {
            waveNumber = 2;
        }

        //WAVE ONE
        if (waveNumber == 1 && _uiManager._waveOneStarted == true)
        {
            _enemySpawnRate = 5.5f;

            // Enemy A
            _enemyMoveSetID = Random.Range(0, 3);
            switch (_enemyMoveSetID)
            {
                case 0: // Move Down
                    Vector3 posToSpawn = new Vector3(Random.Range(-5.19f, 5.19f), 7f, 0f);
                    GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                    break;
                case 1: // Move Left
                    Vector3 posToSpawn1 = new Vector3(-6.5f, Random.Range(4f, 1f), 0f);
                    GameObject newEnemy1 = Instantiate(_enemyPrefab, posToSpawn1, Quaternion.identity);
                    newEnemy1.transform.parent = _enemyContainer.transform;
                    break;
                case 2: // Move Right
                    Vector3 posToSpawn2 = new Vector3(6.5f, Random.Range(4f, 1f), 0f);
                    GameObject newEnemy2 = Instantiate(_enemyPrefab, posToSpawn2, Quaternion.identity);
                    newEnemy2.transform.parent = _enemyContainer.transform;
                    break;
            }

            // Enemy B
            Vector3 posToSpawnB = new Vector3(Random.Range(-5.19f, 5.19f), 7f, 0f);
            GameObject newEnemyB = Instantiate(_enemyPrefab2, posToSpawnB, Quaternion.identity);
            newEnemyB.transform.parent = _enemyContainer.transform;
        }

        //WAVE TWO
        if (waveNumber == 2 && _uiManager._waveTwoStarted == true)
        {
            // Enemy A
            _enemySpawnRate = 4f;
            _enemyMoveSetID = Random.Range(0, 3);
            switch (_enemyMoveSetID)
            {
                case 0: // Move Down
                    Vector3 posToSpawn = new Vector3(Random.Range(-5.19f, 5.19f), 7f, 0f);
                    GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;

                    break;
                case 1: // Move Left
                    Vector3 posToSpawn1 = new Vector3(-6.5f, Random.Range(4f, 1f), 0f);
                    GameObject newEnemy1 = Instantiate(_enemyPrefab, posToSpawn1, Quaternion.identity);
                    newEnemy1.transform.parent = _enemyContainer.transform;

                    break;
                case 2: // Move Right
                    Vector3 posToSpawn2 = new Vector3(6.5f, Random.Range(4f, 1f), 0f);
                    GameObject newEnemy2 = Instantiate(_enemyPrefab, posToSpawn2, Quaternion.identity);
                    newEnemy2.transform.parent = _enemyContainer.transform;

                    break;
            }

            // Enemy B
            Vector3 posToSpawnB = new Vector3(Random.Range(-5.19f, 5.19f), 7f, 0f);
            GameObject newEnemyB = Instantiate(_enemyPrefab2, posToSpawnB, Quaternion.identity);
            newEnemyB.transform.parent = _enemyContainer.transform;

            // Enemy C
            Vector3 posToSpawnC = new Vector3(Random.Range(-5.19f, 5.19f), 7f, 0f);
            GameObject newEnemyC = Instantiate(_enemyPrefab3, posToSpawnC, Quaternion.identity);
            newEnemyC.transform.parent = _enemyContainer.transform;

        }
    }

    #endregion

    #region COROUTINES
    IEnumerator EnemySpawnRoutine()
    {
        while (_stopSpawning == false)
        {
            WaveSystem();
            yield return new WaitForSeconds(_enemySpawnRate);
        }
    }

    IEnumerator PowerUpSpawnRoutine()
    {

        while (_stopSpawning == false)
        {
            _randomPowerUp = Random.Range(0, 100);

            Vector3 posToSpawn = new Vector3(Random.Range(-5.19f, 5.19f), 7f, 0f);

            if (_randomPowerUp < 35 && _randomPowerUp > 10)
            {
                Instantiate(_powerUps[0], posToSpawn, Quaternion.identity);
            }

            if (_randomPowerUp <= 35)
            {
                Instantiate(_powerUps[Random.Range(1,3)], posToSpawn, Quaternion.identity);
            }

            if (_randomPowerUp <= 10)
            {
                Instantiate(_powerUps[3], posToSpawn, Quaternion.identity);
            }

            yield return new WaitForSeconds(Random.Range(5, 15));
        }
    }
    #endregion
}
