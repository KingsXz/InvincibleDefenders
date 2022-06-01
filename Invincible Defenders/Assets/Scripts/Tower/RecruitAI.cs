using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecruitAI : MonoBehaviour, IDamagable
{
    [Header("Enemy Stats")]
    [SerializeField] int id;
    [SerializeField] int minDamage;
    [SerializeField] int maxDamage;
    [SerializeField] float maxHp;
    [SerializeField] float currentHp;

    [Header("Other")]
    [SerializeField] StateRecruit recruitState;
    [SerializeField] Transform placeToRest;
    [SerializeField] GameObject enemyFocus;
    [SerializeField] GameObject mainTower;
    [SerializeField] Image hpBar;
    float regenHpTimer;

    public float CurrentHp 
    { 
        get => currentHp;
        set => currentHp = Mathf.Clamp(value, 0, maxHp); 
    }

    enum StateRecruit
    {
        resting,
        goToEnemy,
        attacking,
        fighting
    }
    void Start()
    {
        recruitState = StateRecruit.resting;
        CurrentHp = maxHp;
        regenHpTimer = 0;
    }

    
    void Update()
    {
        hpBar.fillAmount = (currentHp * 100 / maxHp) / 100;
        switch (recruitState)
        {
            case StateRecruit.resting:
                Rest();
                LookForEnemies();
                RegenHP();
                break;
            case StateRecruit.goToEnemy:
                goToEnemy();
                CheckEnemy();
                break;
            case StateRecruit.attacking:
                AttackEnemy();
                CheckEnemy();
                break;
            case StateRecruit.fighting:
                CheckEnemy();
                break;
        }
    }

    public void GetRecruitInfoSpawn(int idGet, Transform pos, int hpMax, int minDamageToGet, int maxDamageToGet, GameObject tower) 
    {
        id = idGet;
        placeToRest = pos;
        maxHp = hpMax;
        minDamage = minDamageToGet;
        maxDamage = maxDamageToGet;
        mainTower = tower;
    }

    public void GetRecruitInfo(float hpMax, int minDamageToGet, int maxDamageToGet)
    {
        float hpToHeal = hpMax - maxHp;
        maxHp = hpMax;
        CurrentHp += hpToHeal;
        minDamage = minDamageToGet;
        maxDamage = maxDamageToGet;
    }

    void Rest()
    {
        transform.position = Vector2.MoveTowards(transform.position, placeToRest.position, 1 * Time.deltaTime);
    }

    void LookForEnemies()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(placeToRest.position, 0.8f, LayerMask.GetMask("Enemy"));
        foreach (var item in enemiesInRange)
        {
            if(enemyFocus == null)
            {
                if(item.GetComponent<Enemy>().EnemyState == Enemy.State.Path)
                {
                    enemyFocus = item.gameObject;
                }
            }
            else
            {
                if(item.GetComponent<Enemy>().DistanceTraveled > enemyFocus.GetComponent<Enemy>().DistanceTraveled)
                {
                    enemyFocus = item.gameObject;
                }
            }
        }
        if(enemyFocus != null && enemyFocus.GetComponent<Enemy>().RecruitToFight == null)
        {
            recruitState = StateRecruit.goToEnemy;
            enemyFocus.GetComponent<Enemy>().EnemyState = Enemy.State.Wait;
            enemyFocus.GetComponent<Enemy>().GetRecruit(this.gameObject);
        }
        else
        {
            enemyFocus = null;
        }
    }

    void goToEnemy()
    {
        if(enemyFocus != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyFocus.transform.position, 1 * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyFocus.transform.position) <= 0.5f)
            {
                recruitState = StateRecruit.attacking;
            }
        }
    }

    void CheckEnemy()
    {
        if(enemyFocus == null)
        {
            recruitState = StateRecruit.resting;
            StopAllCoroutines();
        }
    }

    void AttackEnemy()
    {
        StartCoroutine(Attack());
        enemyFocus.GetComponent<Enemy>().EnemyState = Enemy.State.Attack;
        recruitState = StateRecruit.fighting;
    }

    IEnumerator Attack()
    {
        enemyFocus.GetComponent<IDamagable>().TakeDamage(Random.Range(minDamage,maxDamage), "armor");
        yield return new WaitForSeconds(1);
        if(recruitState == StateRecruit.fighting)
        {
            StartCoroutine(Attack());
        }
    }

    public void TakeDamage(float damageToTake, string type)
    {
        CurrentHp -= (int)damageToTake;
        if(CurrentHp <= 0)
        {
            Destroy(gameObject);
            mainTower.GetComponent<RecruitTower>().SpawnRec(5, 0, id);
        }
    }

    void RegenHP() 
    {
        regenHpTimer += Time.deltaTime;
        if(regenHpTimer >= 1)
        {
            CurrentHp += 10;
        }
    }
}
