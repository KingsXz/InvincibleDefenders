using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Player Stats")]
    [SerializeField] int playerHp = 20;
    [SerializeField] int playerMoney;
    [Header("Game Info")]
    [SerializeField] int wave;
    [SerializeField] GameStates gameState;
    [SerializeField] bool canCountdown;
    [SerializeField] float antiPrepTimer;
    [Header("")]
    [SerializeField] LevelStructure lvlStruct;
    UiManager uI;

    public enum GameStates
    {
        PreparationTime,
        WaveTime,
        AntiPreparationTime,
    }

    public int PlayerHp { get => playerHp; set => playerHp = value; }
    public int PlayerMoney { get => playerMoney; set => playerMoney = value; }

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
        ManageGameState(GameStates.PreparationTime);
        uI = UiManager.InstanceUi;
        playerMoney = 400;
    }

    void Update()
    {
        if(canCountdown == true)
        {
            antiPrepTimer -= Time.deltaTime;
            if(antiPrepTimer <= 0)
            {
                ManageGameState(GameStates.WaveTime);
                canCountdown = false;
            }
        }
    }

    public void ManageGameState(GameStates stateToBe)
    {
        gameState = stateToBe;
        switch (gameState)
        {
            case GameStates.PreparationTime:
                break;
            case GameStates.AntiPreparationTime:
                uI.ShowLevelStart();
                antiPrepTimer = 20;
                wave++;
                canCountdown = true;
                break;
            case GameStates.WaveTime:
                if(canCountdown == true)
                {
                    float conta = lvlStruct.Wave[wave - 1].GiveMoney * antiPrepTimer / 20;
                    playerMoney += (int)conta;
                    canCountdown = false;
                }
                
                StartWave(wave);
                break;
        }
    }

    void StartWave(int waveNumber)
    {
        for (int i = 0; i < lvlStruct.Wave[waveNumber].EnemySetPerLane.Length; i++) // por lane
        {
            CheckLanes(i, waveNumber);
        }
    }

    public void CheckLanes(int i, int waveNumber)
    {
        PathCreator[] paths = new PathCreator[3];
        int numb=0;
        foreach (Transform child in lvlStruct.Wave[waveNumber].EnemySetPerLane[i].LaneSet.transform)
        {
            paths[numb] = child.gameObject.GetComponent<PathCreator>();
            numb++;
        }
        StartCoroutine(SpawnWave(i, paths, waveNumber));
    }

    IEnumerator SpawnWave(int setNumber, PathCreator[] paths, int waveNumber)
    {
        for (int i = 0; i < lvlStruct.Wave[waveNumber].EnemySetPerLane[setNumber].Enemies.Length; i++)
        {
            if (lvlStruct.Wave[waveNumber].EnemySetPerLane[setNumber].Enemies[i].SpawnAfter == false)
            {
                StartCoroutine(StartDelayedEnemyType(i, setNumber, paths, lvlStruct.Wave[waveNumber].EnemySetPerLane[setNumber].Enemies[i].DelaySpawn, waveNumber));
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
        ManageGameState(GameStates.AntiPreparationTime);
    }

    IEnumerator StartDelayedEnemyType(int i, int setNumber, PathCreator[] paths, float timer, int waveNumber)
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
