using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject bossEnemyPrefab;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] powerupPrefabs;
    private float spawnRange = 7f;


    public void SpawnEnemyWave(int enemiesToSpawn)
    {
        int enemyTypeID;
        for(int i = 0; i < enemiesToSpawn; i++)
        {
            enemyTypeID = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[enemyTypeID], GenerateSpawnPosition(), enemyPrefabs[enemyTypeID].transform.rotation);
        }
    }
    
    public void SpawnPowerupWave(int powerupsToSpawn)
    {
        int powerupTypeID = 0;
        for(int i = 0; i < powerupsToSpawn; i++)
        {
            powerupTypeID = Random.Range(0, powerupPrefabs.Length);
            Instantiate(powerupPrefabs[powerupTypeID], GenerateSpawnPosition(), powerupPrefabs[powerupTypeID].transform.rotation);
        }
    }

    public void SpawnBossEnemy()
    {
        Instantiate(bossEnemyPrefab, GenerateSpawnPosition(), bossEnemyPrefab.transform.rotation);
    }
    
    private Vector3 GenerateSpawnPosition()
    {
        float positionX = Random.Range(-spawnRange, spawnRange);
        float positionZ = Random.Range(-spawnRange, spawnRange);
        return new Vector3(positionX, 0, positionZ);
    }
}