using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject powerupPrefab;
    private int waveNumber = 1;
    private float spawnRange = 7f;
    private int enemyCount;


    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if(enemyCount < 1)
        {
            SpawnEnemyWave(waveNumber);
            SpawnPowerupWave(waveNumber);
            waveNumber++;
        }
    }

    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        for(int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }

    private void SpawnPowerupWave(int powerupsToSpawn)
    {
        for(int i = 0; i < powerupsToSpawn; i++)
        {
            Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
        }
    }
    
    private Vector3 GenerateSpawnPosition()
    {
        float positionX = Random.Range(-spawnRange, spawnRange);
        float positionZ = Random.Range(-spawnRange, spawnRange);
        return new Vector3(positionX, 0, positionZ);
    }
}