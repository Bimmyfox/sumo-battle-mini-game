using System.Collections;
using UnityEngine;

public class BossEnemyController : EnemyController
{
    private WaitForSeconds spawnBossEnemiesInterval = new WaitForSeconds(3.0f);
    private SpawnManager spawnManager;
   
    new void Start()
    {
        base.Start();
        StartCoroutine(BossEnemySpawnerRoutine());
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator BossEnemySpawnerRoutine()
    {
        while(true)
        {
            yield return spawnBossEnemiesInterval;
            spawnManager.SpawnEnemyWave(1);
        }
    }
}
