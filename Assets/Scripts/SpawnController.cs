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

    public MapNodeListVariable startNodeListVariable;

    private int index = 0;
    public void SpawnEnemy(int health, float speed)
    {    
        index++;
        if (index > startNodeListVariable.Value.Count-1)
            index = 0;

        Enemy enemy = Object.Instantiate(enemyPrefab, startNodeListVariable.Value[index].transform.position, Quaternion.identity)
            .GetComponent<Enemy>();
        enemy.Initialize(health, speed, startNodeListVariable.Value[index]);
    }
}