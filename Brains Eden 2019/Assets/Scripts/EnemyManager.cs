using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private const int DEFAULT_WAVE_SIZE = 6;

    public GameObject EnemyPrefab;
    [Space]
    public BuildingManager BuildingManager;

    private Camera Camera;

    [Space]
    [Range(1, 100)] public float SpawnRange = 1f;
    [Range(1, 1000)] public float SpawnInterval = 1f;

    [Range(1, 20)] public int WaveCountIncrease = 1;

    private float ElapsedTime = 0;
    private int CurrentWave = 0;

    private List<Enemy> EnemyList = new List<Enemy>();

    void Start() {
        //When application is starting seed the random number generation with the current time.
        Random.InitState(System.DateTime.Now.GetHashCode());
        Camera = Camera.main;
    }

    void Update() {
        //Spawn Enemy At required Intervals.
        if(ElapsedTime >= SpawnInterval) {
            ElapsedTime = 0;
            SpawnWave();
        }
        else {
            ElapsedTime += Time.deltaTime;
        }

        RemoveDeadEnemies();
        CheckForSingleEnemy();

    }

    public void SpawnEnemy()
    {
        var pointInCircle = Random.insideUnitCircle;
        pointInCircle = pointInCircle.normalized * SpawnRange;

        var spawnPoint = new Vector3(pointInCircle.x, 0, pointInCircle.y);

        var enemy = Instantiate(EnemyPrefab, spawnPoint, Quaternion.identity).GetComponent<Enemy>();
        enemy.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        enemy.Initialize(BuildingManager);
        EnemyList.Add(enemy);

    }

    public void SpawnWave()
    {
        for(int i = 0; i < DEFAULT_WAVE_SIZE + WaveCountIncrease * CurrentWave; i++) {
            SpawnEnemy();
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

    public void CheckForSingleEnemy()
    {
        if(EnemyList.Count == 1) {
            SpawnEnemy();
        }
    }
}
