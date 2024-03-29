﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private const int DEFAULT_WAVE_SIZE = 6;

    public GameObject EnemyPrefab;
    public List<GameObject> ChainBreakerPrefabs = new List<GameObject>();
    public int EnemyAwardingScore;
    public int ChainBreakerAwardingScore;
    [Space]
    public BuildingManager BuildingManager;
    public ChainManager ChainManager;

    private Camera Camera;

    [Space]
    [Range(1, 100)] public float SpawnRange = 1f;
    [Range(1, 1000)] public float SpawnInterval = 1f;

    [Range(1, 20)] public int WaveCountIncrease = 1;

    public float ElapsedTime = 0;
    private int CurrentWave = 0;

    private List<Enemy> EnemyList = new List<Enemy>();

    public bool WavesEnabled = true;
    private bool FirstRun = true;
    public int EnemyCapLimit = 50;

    void Start() {
        //When application is starting seed the random number generation with the current time.
        Random.InitState(System.DateTime.Now.GetHashCode());
        Camera = Camera.main;
    }

    void Update() {
        EnemyList.RemoveAll(x => x == null);

        if ((EnemyList.Count == 0) && (!FirstRun))
        {
            if (ElapsedTime < SpawnInterval - 5.0f)
            {
                ElapsedTime = SpawnInterval - 5.0f;
            }
        }

        if (WavesEnabled && EnemyList.Count < EnemyCapLimit) {
            if (FirstRun) {
                SpawnWave();
                FirstRun = false;
            }
            //Spawn Enemy At required Intervals.
            if (ElapsedTime >= SpawnInterval) {
                ElapsedTime = 0;
                SpawnWave();
            }
            else {
                ElapsedTime += Time.deltaTime;
            }
        }
        RemoveDeadEnemies();
    }

    public void SpawnEnemy()
    {
        var pointInCircle = Random.insideUnitCircle;
        pointInCircle = pointInCircle.normalized * SpawnRange;

        var spawnPoint = new Vector3(pointInCircle.x, 0, pointInCircle.y) + transform.position;

        var enemy = Instantiate(EnemyPrefab, spawnPoint, Quaternion.identity).GetComponent<Enemy>();
        enemy.transform.SetParent(transform);
        enemy.ScoreGiven = EnemyAwardingScore;
        //enemy.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        enemy.Initialize(BuildingManager, ChainManager);
        EnemyList.Add(enemy);
    }

    public void SpawnChainBreaker()
    {
        var pointInCircle = Random.insideUnitCircle;
        pointInCircle = pointInCircle.normalized * SpawnRange;

        var spawnPoint = new Vector3(pointInCircle.x, 0, pointInCircle.y) + transform.position;

        var enemy = Instantiate(ChainBreakerPrefabs.Random(), spawnPoint, Quaternion.identity).GetComponent<Enemy>();
        enemy.transform.SetParent(transform);
        enemy.ScoreGiven = ChainBreakerAwardingScore;
        enemy.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        enemy.Initialize(BuildingManager, ChainManager);
        EnemyList.Add(enemy);
    }

    public void SpawnWave()
    {
        for(int i = 0; i < DEFAULT_WAVE_SIZE + WaveCountIncrease * CurrentWave; i++) {
            SpawnEnemy();
        }
        for(int i = 0; i <= CurrentWave; i++) {
            SpawnChainBreaker();
        }
        CurrentWave++;
    }

    public void ResetWaveCount() {
        CurrentWave = 0;
    }

    public void RemoveDeadEnemies()
    {
        EnemyList.RemoveAll(delegate (Enemy e) { return e.IsDead; });
    }

}
