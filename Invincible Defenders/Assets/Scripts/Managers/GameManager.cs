using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.UI;

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
    [SerializeField] int coroutinesNotFinished;
    [SerializeField] int enemiesAlive;

    [Header("Other")]
    [SerializeField] Button rockBtt;
    [SerializeField] Button poisBtt;
    [SerializeField] Button calBtt;
    [SerializeField] LevelStructure lvlStruct;
    [SerializeField] GameObject abilityRangeObj;
    AbilityType abilityType;
    GameObject rangeObj;
    UiManager uI;
    MusicManager mM;
    bool enableCooldonw;
    float abilityTimer;
    bool isPaused;

    public enum GameStates
    {
        PreparationTime,
        WaveTime,
        AntiPreparationTime,
        WaitUntilEnd
    }

    public enum AbilityType
    {
        RockSlide,
        PoisonBomb,
        Caltrops
    }


    public int PlayerHp { get => playerHp; set => playerHp = value; }
    public int PlayerMoney { get => playerMoney; set => playerMoney = value; }
    public int EnemiesAlive { get => enemiesAlive; set => enemiesAlive = value; }
    public bool EnableCooldonw { get => enableCooldonw; set => enableCooldonw = value; }

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
        mM = MusicManager.InstanceMusic;
        playerMoney = 400;
        abilityTimer = 0;
        isPaused = false;
        mM.PlayMusic("InGameMusic");
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if(isPaused == true)
            {
                Time.timeScale = 0;
                GetComponent<MenuManager>().OpenMenu();
            }
            if (isPaused == false)
            {
                Time.timeScale = 1;
                GetComponent<MenuManager>().CloseAudio();
                GetComponent<MenuManager>().CloseMenu();
            }
        }
        if(EnableCooldonw == true)
        {
            AbilitiesCoolwn();
        }
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

    public void AbilitiesCoolwn()
    {
        abilityTimer += Time.deltaTime;
        calBtt.GetComponent<Image>().fillAmount = abilityTimer / 10;
        poisBtt.GetComponent<Image>().fillAmount = abilityTimer / 10;
        rockBtt.GetComponent<Image>().fillAmount = abilityTimer / 10;
        poisBtt.interactable = false;
        rockBtt.interactable = false;
        calBtt.interactable = false;
        if(abilityTimer >= 10)
        {
            abilityTimer = 0;
            rockBtt.interactable = true;
            poisBtt.interactable = true;
            calBtt.interactable = true;
            EnableCooldonw = false;
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
                wave++;
                if (wave >= lvlStruct.Wave.Length)
                {
                    ManageGameState(GameStates.WaitUntilEnd);
                    break;
                }
                uI.ShowLevelStart();
                antiPrepTimer = 40;
                canCountdown = true;
                break;
            case GameStates.WaveTime:
                if(canCountdown == true)
                {
                    float conta = lvlStruct.Wave[wave - 1].GiveMoney * antiPrepTimer / 40;
                    playerMoney += (int)conta;
                    canCountdown = false;
                }
                StartWave(wave);
                break;
            case GameStates.WaitUntilEnd:
                StartCoroutine(CheckEnemiesAlive());
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
                coroutinesNotFinished++;
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
                    enemySpawned.GetComponent<SpriteRenderer>().sortingOrder = rand + 1;
                    enemySpawned.PathCreator = paths[rand];
                    EnemiesAlive++;
                    if (lvlStruct.Wave[waveNumber].EnemySetPerLane[setNumber].Enemies[i].DelayBetweenEnemies == 0)
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
        yield return new WaitUntil(WaitUntilGenerationComplete);
        ManageGameState(GameStates.AntiPreparationTime);
    }

    bool WaitUntilGenerationComplete()
    {
        if(coroutinesNotFinished == 0)
        {
            return true;
        }
        return false;
    }

    IEnumerator StartDelayedEnemyType(int i, int setNumber, PathCreator[] paths, float timer, int waveNumber)
    {
        yield return new WaitForSeconds(timer);
        for (int i2 = 0; i2 < lvlStruct.Wave[waveNumber].EnemySetPerLane[setNumber].Enemies[i].Quantity; i2++)
        {
            Enemy enemySpawned = Instantiate(lvlStruct.Wave[waveNumber].EnemySetPerLane[setNumber].Enemies[i].EnemyType);
            int rand = Random.Range(0, 2);
            enemySpawned.GetComponent<SpriteRenderer>().sortingOrder = rand + 1;
            enemySpawned.PathCreator = paths[rand];
            EnemiesAlive++;
            yield return new WaitForSeconds(lvlStruct.Wave[waveNumber].EnemySetPerLane[setNumber].Enemies[i].DelayBetweenEnemies);
        }
        coroutinesNotFinished--;
    }

    IEnumerator CheckEnemiesAlive()
    {
        yield return new WaitUntil(WaitUntilAllEnemiesDie);
        if(playerHp > 0)
        {
            Time.timeScale = 0;
            uI.ActivateWinUi();
        }
    }

    bool WaitUntilAllEnemiesDie()
    {
        if(EnemiesAlive > 0)
        {
            return false;
        }
        return true;
    }

    public void ActivateAbility(string ab)
    {
        if(ab == "rock")
        {
            StartCoroutine(SpawnAbility(AbilityType.RockSlide));
        }
        if(ab == "poison")
        {
            StartCoroutine(SpawnAbility(AbilityType.PoisonBomb));
        }
        if (ab == "caltrops")
        {
            StartCoroutine(SpawnAbility(AbilityType.Caltrops));
        }
    }

    IEnumerator SpawnAbility(AbilityType ab)
    {
        yield return new WaitForEndOfFrame();
        rangeObj = Instantiate(abilityRangeObj);
        rangeObj.transform.localScale = new Vector3(1, 1, 1);
        rangeObj.GetComponent<Abilities>().GetAbilityType(ab);
    }

}
