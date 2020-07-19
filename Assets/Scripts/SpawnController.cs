using System;
using System.Collections;
using System.Collections.Generic;
using Modular;
using UnityEngine;
using Object = UnityEngine.Object;

public class SpawnController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int HealthToSpawn = 10;
    public float SpeedToSpawn = 0.5f;
    
    public MapNodeVariable start;

    public void SpawnEnemy(int health, float speed)
    {
        Enemy enemy = Object.Instantiate(enemyPrefab, start.Value.transform.position, Quaternion.identity)
            .GetComponent<Enemy>();
        enemy.Initialize(health, speed, start.Value);
    }
}