using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float enemySpawnRate;
    private const float fasterSpawnRateTimer = 30f;
    public GameObject firstEnemy;
    public GameObject secondEnemy;
    public GameObject thirdEnemy;
    private List<GameObject> enemies;

    private void SpawnEnemy() {
        Instantiate(enemies[Random.Range(0, enemies.Count - 1)], transform.position + new Vector3(Random.Range(-3, 4) * 3, 0f, 0f), Quaternion.identity);
    }

    private void FasterSpawnRate() {
        if (enemySpawnRate > 1) {
            enemySpawnRate -= 0.4f;
            if (enemySpawnRate > 3f) {
                enemies.Add(secondEnemy);
            } else {
                enemies.Add(thirdEnemy);
            }
        } else {
            enemySpawnRate /= 2;
            enemies.Add(thirdEnemy);
        }
        CancelInvoke(nameof(SpawnEnemy));
        InvokeRepeating(nameof(SpawnEnemy), enemySpawnRate, enemySpawnRate);
    }

    void Awake()
    {
        enemySpawnRate = 5f;
        enemies = new List<GameObject> {firstEnemy, firstEnemy, firstEnemy};

        CancelInvoke();
        InvokeRepeating(nameof(SpawnEnemy), enemySpawnRate, enemySpawnRate);
        InvokeRepeating(nameof(FasterSpawnRate), fasterSpawnRateTimer, fasterSpawnRateTimer);
    }
}
