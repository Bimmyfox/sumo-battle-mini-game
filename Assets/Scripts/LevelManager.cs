using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int bossLevelNumber = 4;
    private PlayerController playerController;
    private int levelNumber = 0;
    private SpawnManager spawnManager;


    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    void Update()
    {
        if(playerController.IsPlayerOutOfArena())
        {
            RestartLevel();
        }
        else if(GameObject.FindGameObjectsWithTag("Enemy").Length < 1)
        {
            NextLevel();
        }
    }

    private void RestartLevel()
    {
        DestroyAllBoosters();
        DestroyAllEnemies();
        playerController.ResetPlayer();
        SetUpEnemies();
    }

    private void NextLevel()
    {
        DestroyAllBoosters();
        levelNumber++;
        SetUpEnemies();
    }

    private void SetUpEnemies()
    {
        if(levelNumber >= bossLevelNumber)
        {
            spawnManager.SpawnBossEnemy();
            spawnManager.SpawnPowerupWave(bossLevelNumber);
            return;
        }
        spawnManager.SpawnEnemyWave(levelNumber);
        spawnManager.SpawnPowerupWave(levelNumber);
    }

    private void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");  
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }  
    }

    //TODO: refactor using object pool pattern
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
