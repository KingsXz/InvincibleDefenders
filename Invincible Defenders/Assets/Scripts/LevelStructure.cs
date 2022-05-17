using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;


[CreateAssetMenu(menuName = "ScriptableObjects/Wave")]
public class LevelStructure : ScriptableObject
{
    [SerializeField] WaveInfo[] wave;


    public WaveInfo[] Wave { get => wave; set => wave = value; }
}

[System.Serializable]
public struct WaveInfo
{
    [SerializeField] EnemySetPerLane[] enemySetPerLane;
    [SerializeField] int giveMoney;
    [SerializeField] RoundType typeOfRound;

    public EnemySetPerLane[] EnemySetPerLane { get => enemySetPerLane; set => enemySetPerLane = value; }
    public int GiveMoney { get => giveMoney; set => giveMoney = value; }
    private RoundType TypeOfRound { get => typeOfRound; set => typeOfRound = value; }

    enum RoundType
    {
        NormalRound,
        MoneyRound,
        BossRound
    }
}

[System.Serializable]
public struct EnemySetPerLane
{
    [SerializeField] GameObject laneSet;
    [SerializeField] EnemySet[] enemies;

    public GameObject LaneSet { get => laneSet; set => laneSet = value; }
    public EnemySet[] Enemies { get => enemies; set => enemies = value; }
}

[System.Serializable]
public struct EnemySet
{
    [SerializeField] Enemy enemyType;
    [SerializeField] int quantity;
    [SerializeField] float delaySpawn;
    [SerializeField] bool spawnAfter;
    [SerializeField] float delayBetweenEnemies;

    public Enemy EnemyType { get => enemyType; set => enemyType = value; }
    public int Quantity { get => quantity; set => quantity = value; }
    public float DelaySpawn { get => delaySpawn; set => delaySpawn = value; }
    public bool SpawnAfter { get => spawnAfter; set => spawnAfter = value; }
    public float DelayBetweenEnemies { get => delayBetweenEnemies; set => delayBetweenEnemies = value; }
}