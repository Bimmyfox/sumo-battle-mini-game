using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private float spawnRange = 7f;

    void Start()
    {  
        Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
    }
    
    private Vector3 GenerateSpawnPosition()
    {
        float positionX = Random.Range(-spawnRange, spawnRange);
        float positionZ = Random.Range(-spawnRange, spawnRange);
        return new Vector3(positionX, 0, positionZ);
    }
}