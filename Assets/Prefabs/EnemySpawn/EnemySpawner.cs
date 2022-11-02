using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public Enemy EnemyToSpawn;
    public int SpawnCount = 6;
    public float SpawnInterval = 2f;
}
public class EnemySpawner : MonoBehaviour
{

    [SerializeField] Wave[] waves;
    int CurrentWaveIndex = 0;
    [SerializeField] float SpawnIntervalBetweenWaves = 5f;
    Vector3 SpawnStartPos;
    // Start is called before the first frame update
    void Start()
    {
        SpawnStartPos = FindObjectOfType<LevelGenerator>().GetPathWaypoints()[0];
        StartCoroutine(StartSpawnEnemies());
    }
    IEnumerator StartSpawnEnemies()
    {
        Wave currentWave = waves[CurrentWaveIndex];
        while (true)
        {
            if (currentWave.SpawnCount == 0)
            {
                CurrentWaveIndex = CurrentWaveIndex + 1;
                if (CurrentWaveIndex == waves.Length)
                {
                    OnWaveAllFinished();
                    break;
                }
                else
                {
                    currentWave = waves[CurrentWaveIndex];
                    yield return new WaitForSeconds(SpawnIntervalBetweenWaves);
                }
            }
            else
            {
                Instantiate(currentWave.EnemyToSpawn, SpawnStartPos, Quaternion.identity);
                currentWave.SpawnCount--;
                yield return new WaitForSeconds(currentWave.SpawnInterval);
            }
           
        }
    }

    void OnWaveAllFinished()
    {
        StopAllCoroutines();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
