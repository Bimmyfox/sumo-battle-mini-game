using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject powerupPrefab;
    private PlayerController playerController;
    private int waveNumber = 1;
    private float spawnRange = 7f;
    private int enemiesCount;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if(playerController.IsPlayerOutOfArena())
        {
            RestartLevel();
        }
        else
        {
            enemiesCount = FindObjectsOfType<EnemyController>().Length;
            if(enemiesCount < 1)
            {
                NextLevel();
            }
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

    private void RestartLevel()
    {
        DestroyAllBoosters();
        DestroyAllEnemies();
        SpawnEnemyWave(waveNumber);
        SpawnPowerupWave(waveNumber);
        playerController.ResetPlayer();
    }

    private void NextLevel()
    {
        DestroyAllBoosters();
        SpawnEnemyWave(waveNumber);
        SpawnPowerupWave(waveNumber);
        waveNumber++;
    }

    private void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");  
         foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }  
    }

    private void DestroyAllBoosters()
    {
        GameObject[] powerUpBoosters = GameObject.FindGameObjectsWithTag("PowerUpBooster");   
        GameObject[] shootBoosters = GameObject.FindGameObjectsWithTag("ShootBooster");   
        GameObject[] smashBoosters = GameObject.FindGameObjectsWithTag("SmashBooster");   
       
        foreach (GameObject booster in powerUpBoosters)
        {
            Destroy(booster);
        } 
        
        foreach (GameObject booster in shootBoosters)
        {
            Destroy(booster);
        } 
        
        foreach (GameObject booster in smashBoosters)
        {
            Destroy(booster);
        }
    }
}