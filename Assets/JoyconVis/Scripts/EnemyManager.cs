using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    public List<EnemyData> enemyPool;
    public EnemyData bossEnemy;
    public GameObject enemyPrefab;
    public Transform player;

    [Header("Wave Settings")]
    public int totalWaves = 5;
    public int enemiesPerWave = 5;
    public float initialSpawnInterval = 2f;
    public float intervalReductionPerWave = 0.3f;
    public float spawnZ = 10f;
    public Vector2 spawnXRange = new Vector2(-5f, 5f);
    public float timeBetweenWaves = 3f;
    public TextMeshProUGUI waveTimerText;
    public TextMeshProUGUI waveNum;

    private int currentWave = 0;
    private float waveTimer;

    public void StartWaves()
    {
        StartCoroutine(WaveRoutine());
    }

    IEnumerator WaveRoutine()
    {
        while (currentWave < totalWaves)
        {
            currentWave++;
            float spawnInterval = Mathf.Max(0.5f, initialSpawnInterval - intervalReductionPerWave * currentWave);
            waveNum.text = "Wave: " + currentWave.ToString();

            waveTimer = enemiesPerWave * spawnInterval;
            StartCoroutine(WaveCountdown());

            yield return StartCoroutine(SpawnWave(enemiesPerWave, spawnInterval));
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        Debug.Log("Boss time");
        SpawnBoss();
    }

    IEnumerator SpawnWave(int count, float interval)
    {
        for (int i = 0; i < count; i++)
        {
            if (!PlayerHealth.isDead)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(interval);
            }
        }
    }

    IEnumerator WaveCountdown()
    {
        while (waveTimer > 0)
        {
            waveTimer -= Time.deltaTime;
                waveTimerText.text = $"Wave ends in: {waveTimer:F1}s";
            yield return null;
        }
            waveTimerText.text = "Zombies outside are taking a quick break";
    }

    void SpawnEnemy()
    {
        if (enemyPool.Count == 0)
            return;

        int allowedTier = currentWave;

        List<EnemyData> availableEnemies = new List<EnemyData>();
        foreach (EnemyData enemy in enemyPool)
        {
            if (enemy.tier <= allowedTier)
                availableEnemies.Add(enemy);
        }

        if (availableEnemies.Count == 0)
            availableEnemies.AddRange(enemyPool);

        EnemyData chosen = availableEnemies[Random.Range(0, availableEnemies.Count)];
        float x = Random.Range(spawnXRange.x, spawnXRange.y);
        Vector3 pos = new Vector3(x, 0.8f, spawnZ);

        GameObject obj = Instantiate(enemyPrefab, pos, Quaternion.identity);
        Enemy enemyScript = obj.GetComponent<Enemy>();
        enemyScript.Initialize(chosen, player);
    }

    void SpawnBoss()
    {
        Vector3 pos = new Vector3(0f, 0.8f, spawnZ);
        GameObject boss = Instantiate(enemyPrefab, pos, Quaternion.identity);
        Enemy enemyScript = boss.GetComponent<Enemy>();
        enemyScript.Initialize(bossEnemy, player);
    }
}
