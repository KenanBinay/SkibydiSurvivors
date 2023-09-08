using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityMovementAI;
using static SkibidiSpawnManager;

public class SkibidiSpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
        public int waveQuota;
        public float spawnInterval;
        public int spawnCount;
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;
        public int spawnCount;
        public GameObject enemyPrefab;
    }

    public List<Wave> waves;
    public int currentWaveCount;

    [Header("Spawner Attributes")]
    float spawnTimer;
    int currentWaveQuota;
    public float waveInterval;
    public int enemiesAlive;
    public int maxEnemiesAllowed;
    public bool maxEnemiesReached = false;
    private bool waveStarted = false;
    private bool allEnemiesDestroyed = false;

    [Header("Spawn Positions")]
    public List<Transform> relativeSpawnPoints;

    Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        CalculateWaveQuota();
    }

    private void Update()
    {
        if (!waveStarted && currentWaveCount < waves.Count
            && waves[currentWaveCount].waveQuota == currentWaveQuota)
        {
            StartCoroutine(BeginNextWave());
            waveStarted = true; // set waveStarted to true to prevent the coroutine from starting multiple times
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }    
    }

    IEnumerator BeginNextWave()
    {
        yield return new WaitForSeconds(waveInterval);

        if (currentWaveCount < waves.Count - 1)
        {
            currentWaveCount++;
            CalculateWaveQuota();
            waveStarted = false;
        }
    }

    void CalculateWaveQuota()
    {
        currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning(currentWaveQuota);
    }

    void SpawnEnemies()
    {
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }

                    GameObject enemy = Instantiate(enemyGroup.enemyPrefab, player.position +
                        relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position
                        , Quaternion.identity, transform);
             
                    enemyController ec = enemy.GetComponent<enemyController>();
                    ec.SetTarget(player);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;
                }
            }
        }

        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
    }

    void firstWave()
    {
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0)
        {
            StartCoroutine(BeginNextWave());
        }

        spawnTimer = 0f;
        SpawnEnemies();
    }
}
