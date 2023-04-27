using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _PowerupPrefab;
    [SerializeField]
    private float _spawnRate = 8.0f;
    private int _wave = 1;
    private float _enemyCounter = 0;
    [SerializeField]
    private Transform _enemyContainer;
    [SerializeField]
    private GameObject[] Powerups;
    [SerializeField]
    private GameObject[] Enemies;
    [SerializeField]
    private float[] spawnChances;
    [SerializeField]
    private float[] enemySpawnChances;
    private bool _enemyLimit = false;
    private float _enemyWaveLimit = 5;
    private float _enemiesRemaining;
    private UIManager _uiManager;
    private bool _stopSpawning = false;
    [SerializeField]
    private GameObject _bossPrefab;
    // Start is called before the first frame update
    void Start()
    {
        _enemiesRemaining = _enemyWaveLimit;
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
    }
    
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    //spawn game object enemy every 5 seconds
    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            Debug.Log("hey it worked?");

            // Calculate the total chance of all objects
                float totalChance = 0f;
                for (int w = 0; w < enemySpawnChances.Length; w++)
                {
                    totalChance += enemySpawnChances[w];
                }

                // Choose a random chance
                float randomChance = Random.Range(0f, totalChance);

                // Choose the object corresponding to the random chance
                GameObject randomEnemy = null;
                for (int w = 0; w < Enemies.Length; w++)
                {
                    if (randomChance < enemySpawnChances[w])
                    {
                        randomEnemy = Enemies[w];
                        break;
                    }
                    randomChance -= enemySpawnChances[w];
                }
            
                float randomX = Random.Range(-9f, 9f);
                float randomY = Random.Range(3f, 7f);
                
                Instantiate(randomEnemy, new Vector3(randomX, 6.5f, 0), Quaternion.identity, _enemyContainer);
                _enemyCounter++;
                if(_enemyCounter == _enemyWaveLimit)
                {
                    NewWave();
                    _enemyLimit = true;
                }
                yield return new WaitForSeconds(_spawnRate);
            
        }

        yield return null;
    }
    
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        
        while (_stopSpawning == false)
        {
            // Calculate the total chance of all objects
            float totalChance = 0f;
            for (int i = 0; i < spawnChances.Length; i++)
            {
                totalChance += spawnChances[i];
            }

            // Choose a random chance
            float randomChance = Random.Range(0f, totalChance);

            // Choose the object corresponding to the random chance
            GameObject randomPowerup = null;
            for (int i = 0; i < Powerups.Length; i++)
            {
                if (randomChance < spawnChances[i])
                {
                    randomPowerup = Powerups[i];
                    break;
                }
                randomChance -= spawnChances[i];
            }
            
            float randomX = Random.Range(-9f, 9f);
            float randomY = Random.Range(3f, 7f);
            Instantiate(randomPowerup, new Vector3(randomX, 6.5f, 0), Quaternion.identity);
            yield return new WaitForSeconds(randomY);
        }
    }
    

    public void NewWave()
    {
        _wave++;
        _enemyWaveLimit = (5 * _wave);
        _enemyCounter = 0;
        _spawnRate -= 1.0f;
        //_enemiesRemaining = _enemyWaveLimit;
        _enemyLimit = false;
        _uiManager.WaveCounter(_wave);
        WaitForSeconds wait = new WaitForSeconds(5.0f);
        if (_wave < 5)
        {
            StartCoroutine(SpawnEnemyRoutine());
        }
        else if(_wave == 5)
        {
            Instantiate(_bossPrefab, new Vector3(0, 10f, 0), Quaternion.identity, _enemyContainer);
        }

    }
    
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
