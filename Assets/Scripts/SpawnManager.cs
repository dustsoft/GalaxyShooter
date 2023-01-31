using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemyPrefab2;
    [SerializeField] GameObject _enemyContainer;
    //[SerializeField] float _enemySpawnRate = 4.75f;
    [SerializeField] GameObject[] _powerUps;

    UIManager _uiManager;


    public int waveNumber = 1;

    public int enemyDestroyedCount;

    public int _enemyMoveSetID;
    bool _stopSpawning = false;

    float _enemySpawnRate;

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerUpSpawnRoutine());


    }

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
            Vector3 posToSpawn = new Vector3(Random.Range(-5.19f, 5.19f), 7f, 0f);

            int _randomPowerUp = Random.Range(0, 4);

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
        if (enemyDestroyedCount > 9)
        {
            waveNumber = 2;
        }

        //WAVE ONE
        if (waveNumber == 1 && _uiManager._waveOneStarted == true)
        {
            _enemySpawnRate = 5.5f;
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
        }

        //WAVE TWO
        if (waveNumber == 2 && _uiManager._waveTwoStarted == true)
        {
            _enemySpawnRate = 4f;
            _enemyMoveSetID = Random.Range(0, 3);
            switch (_enemyMoveSetID)
            {
                case 0: // Move Down
                    Vector3 posToSpawn = new Vector3(Random.Range(-5.19f, 5.19f), 7f, 0f);
                    GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;

                    GameObject newEnemyB = Instantiate(_enemyPrefab2, posToSpawn, Quaternion.identity);
                    newEnemyB.transform.parent = _enemyContainer.transform;

                    break;
                case 1: // Move Left
                    Vector3 posToSpawn1 = new Vector3(-6.5f, Random.Range(4f, 1f), 0f);
                    GameObject newEnemy1 = Instantiate(_enemyPrefab, posToSpawn1, Quaternion.identity);
                    newEnemy1.transform.parent = _enemyContainer.transform;

                    GameObject newEnemyB1 = Instantiate(_enemyPrefab2, posToSpawn1, Quaternion.identity);
                    newEnemyB1.transform.parent = _enemyContainer.transform;

                    break;
                case 2: // Move Right
                    Vector3 posToSpawn2 = new Vector3(6.5f, Random.Range(4f, 1f), 0f);
                    GameObject newEnemy2 = Instantiate(_enemyPrefab, posToSpawn2, Quaternion.identity);
                    newEnemy2.transform.parent = _enemyContainer.transform;

                    GameObject newEnemyB2 = Instantiate(_enemyPrefab2, posToSpawn2, Quaternion.identity);
                    newEnemyB2.transform.parent = _enemyContainer.transform;

                    break;
            }
        }

        //WAVE THREE
        if (waveNumber == 3)
        {
            //CODE
        }
    }


}
