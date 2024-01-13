using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{

    public List<EnemyController> Spawns = new List<EnemyController>();
    public List<EnemyController> SpawnsBoss = new List<EnemyController>();

    public int enemiesPerWave = 3;
    public float spawnInterval = 1f;
    public float spawnDelay = 2f;

    public int bossPerWave = 0;
    public float spawnBossInterval = 3f;
    public float spawnBossDelay = 6f;

    public int enemyCount = 0;

    private int enemiesSpawnedForWave;
    private int bossesSpawnedForWave;

    public bool isSpawningWave = false;

    private int xPos;
    private int yPos;
    private int enemyType;

    EnemyController GetEnemySpawn(List<EnemyController> CheckSpawn) {

        int randomNumber = Random.Range(0, 101);

        List<EnemyController> possibleEnemies = new List<EnemyController>();

        foreach(EnemyController enemy in CheckSpawn) {

            if (randomNumber <= enemy.enemyChanceSpawn) {

                possibleEnemies.Add(enemy);

            }

        }

        if (possibleEnemies.Count > 0) {

            EnemyController enemySpawned = possibleEnemies[Random.Range(0, possibleEnemies.Count)];
            return enemySpawned;

        }

        return null;

    }

    public IEnumerator SpawnWave()
    {

        isSpawningWave = true;

        if (bossPerWave == 0)
        {

            yield return new WaitForSeconds(spawnDelay);

            while (enemiesSpawnedForWave < enemiesPerWave)
            {

                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);

            }

        }
        else
        {

            yield return new WaitForSeconds(spawnBossDelay);

            while (bossesSpawnedForWave < bossPerWave)
            {

                SpawnBoss();
                yield return new WaitForSeconds(spawnBossInterval);

            }

        }

        isSpawningWave = false;
        bossPerWave = 0;

    }

    void SpawnEnemy()
    {

        xPos = Random.Range(-20, 21);
        yPos = Random.Range(-20, 21);
        enemyType = Random.Range(1, 8);

        EnemyController enemySpawned = GetEnemySpawn(Spawns);

        Instantiate(enemySpawned, new Vector3(xPos, 2, yPos), Quaternion.identity);

        enemyCount++;
        enemiesSpawnedForWave++;

    }

    void SpawnBoss()
    {

        yPos = Random.Range(-20, 21);
        xPos = Random.Range(-20, 21);
        enemyType = Random.Range(1, 3);

        EnemyController bossSpawned = GetEnemySpawn(SpawnsBoss);

        Instantiate(bossSpawned, new Vector3(xPos, 2, yPos), Quaternion.identity);

        enemyCount++;
        bossesSpawnedForWave++;

    }

    public void ResetEnemiesSpawnedForWave()
    {

        enemiesSpawnedForWave = 0;
        bossesSpawnedForWave = 0;

    }
}