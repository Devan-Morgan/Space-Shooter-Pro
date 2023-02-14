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
    private float _spawnRate = 5.0f;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] Powerups;
    
    private bool _stopSpawning = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    //spawn game object enemy every 5 seconds
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-9f, 9f);
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(randomX, 6.5f, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnRate);
            
        }
    }
    
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-9f, 9f);
            float randomY = Random.Range(3f, 7f);
            int randomPowerup = Random.Range(0, 3);
            Instantiate(Powerups[randomPowerup], new Vector3(randomX, 6.5f, 0), Quaternion.identity);
            yield return new WaitForSeconds(randomY);
        }
    }
    
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
