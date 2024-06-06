using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float enemySpawnRate = 5f;
    public GameObject enemy;

    private void SpawnEnemy() {
        int rand = Random.Range(-3, 4);
        Instantiate(enemy, transform.position + new Vector3(rand * 3, 0f, 0f), Quaternion.identity);
    }

    void Awake()
    {
        InvokeRepeating("SpawnEnemy", 1f, enemySpawnRate);
    }
}
