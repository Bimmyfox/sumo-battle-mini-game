using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    private float spawnRange = 7f;

    void Start()
    {  
        Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
    }

    void Update()
    {
    }
    
    private Vector3 GenerateSpawnPosition()
    {
        float positionX = Random.Range(-spawnRange, spawnRange);
        float positionZ = Random.Range(-spawnRange, spawnRange);
        return new Vector3(positionX, 0, positionZ);
    }
}