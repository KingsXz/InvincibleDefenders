using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] LevelStructure lvlStruct;
    [SerializeField] float playerHp = 100;
    int waveNumber;


    public float PlayerHp { get => playerHp; set => playerHp = value; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        Instantiate(lvlStruct);
        StartWave(waveNumber);
    }

    void Update()
    {
        
    }

    void StartWave(int waveNumber)
    {
        for (int i = 0; i < lvlStruct.Wave[waveNumber].EnemySetPerLane.Length; i++) // por lane
        {
            CheckLanes(i);
        }
    }

    public void CheckLanes(int i)
    {
        PathCreator[] paths = new PathCreator[3];
        int numb=0;
        foreach (Transform child in lvlStruct.Wave[waveNumber].EnemySetPerLane[i].LaneSet.transform)
        {
            paths[numb] = child.gameObject.GetComponent<PathCreator>();
            numb++;
        }
        StartCoroutine(SpawnWave(i, paths));
    }

    IEnumerator SpawnWave(int setNumber, PathCreator[] paths)
    {
        for (int i = 0; i < lvlStruct.Wave[waveNumber].EnemySetPerLane[setNumber].Enemies.Length; i++)
        {
            if (lvlStruct.Wave[waveNumber].EnemySetPerLane[setNumber].Enemies[i].SpawnAfter == false)
            {
                StartCoroutine(StartDelayedEnemyType(i, setNumber, paths, lvlStruct.Wave[waveNumber].EnemySetPerLane[setNumber].Enemies[i].DelaySpawn));
            }
        }
        for (int i = 0; i < lvlStruct.Wave[waveNumber].EnemySetPerLane[setNumber].Enemies.Length; i++)
        {
            if(lvlStruct.Wave[waveNumber].EnemySetPerLane[setNumber].Enemies[i].SpawnAfter == true)
            {
                for (int i2 = 0; i2 < lvlStruct.Wave[waveNumber].EnemySetPerLane[setNumber].Enemies[i].Quantity; i2++)
                {
                    if(i2 == 0)
                    {
                        yield return new WaitForSeconds(lvlStruct.Wave[waveNumber].EnemySetPerLane[setNumber].Enemies[i].DelaySpawn);
                    }
                    Enemy enemySpawned = Instantiate(lvlStruct.Wave[waveNumber].EnemySetPerLane[setNumber].Enemies[i].EnemyType);
                    int rand = Random.Range(0, 2);
                    enemySpawned.PathCreator = paths[rand];
                    if(lvlStruct.Wave[waveNumber].EnemySetPerLane[setNumber].Enemies[i].DelayBetweenEnemies == 0)
                    {
                        yield return new WaitForSeconds(1);
                    }
                    else
                    {
                        yield return new WaitForSeconds(lvlStruct.Wave[waveNumber].EnemySetPerLane[setNumber].Enemies[i].DelayBetweenEnemies);
                    }
                    
                }
            }
        }   
    }

    IEnumerator StartDelayedEnemyType(int i, int setNumber, PathCreator[] paths, float timer)
    {
        yield return new WaitForSeconds(timer);
        for (int i2 = 0; i2 < lvlStruct.Wave[waveNumber].EnemySetPerLane[setNumber].Enemies[i].Quantity; i2++)
        {
            Enemy enemySpawned = Instantiate(lvlStruct.Wave[waveNumber].EnemySetPerLane[setNumber].Enemies[i].EnemyType);
            enemySpawned.PathCreator = paths[0];
            yield return new WaitForSeconds(1);
        }
    }
}
