using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject powerupPrefab;
    private int waveNumber = 1;
    private float spawnRange = 7f;
    private int enemyCount;


    void Update()
    {
        enemyCount = FindObjectsOfType<EnemyController>().Length;
        if(enemyCount < 1)
        {
            SpawnEnemyWave(waveNumber);
            SpawnPowerupWave(waveNumber);
            waveNumber++;
        }
    }

    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        int enemyTypeID;
        for(int i = 0; i < enemiesToSpawn; i++)
        {
            enemyTypeID = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[enemyTypeID], GenerateSpawnPosition(), enemyPrefabs[enemyTypeID].transform.rotation);
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