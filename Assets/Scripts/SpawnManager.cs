using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject enemyContainer;
    [SerializeField]
    private float enemySpawnDelay = 2f;
    [SerializeField]
    private float powerupSpawnDelay = 10f;
    //[SerializeField]
    //private GameObject TripleShotPowerupPrefab;
    [SerializeField]
    private GameObject[] powerups;
    private bool _stopSpawing = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawing == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-11f, 11f), transform.position.y, 0f);
            GameObject newEnemy = Instantiate(enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(enemySpawnDelay);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(5f);
        while (_stopSpawing == false)
        {
            yield return new WaitForSeconds(powerupSpawnDelay);
            Vector3 posToSpawn = new Vector3(Random.Range(-11f, 11f), transform.position.y, 0f);
            int randIndex = (int)Random.Range(0, powerups.Length);
            GameObject randomPowerup = powerups[randIndex];
            if(randomPowerup != null)
            {
                Instantiate(randomPowerup, posToSpawn, Quaternion.identity);
            }
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawing = true;
    }
}
