using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitAI : MonoBehaviour
{
    [SerializeField] int id;

    [SerializeField] StateRecruit recruitState;
    [SerializeField] Transform placeToRest;
    [SerializeField] GameObject enemyFocus;
    int level;

    enum StateRecruit
    {
        resting,
        goToEnemy,
        attacking
    }
    void Start()
    {
        recruitState = StateRecruit.resting;
    }

    
    void Update()
    {
        switch (recruitState)
        {
            case StateRecruit.resting:
                Rest();
                LookForEnemies();
                break;
            case StateRecruit.goToEnemy:
                goToEnemy();
                break;
            case StateRecruit.attacking:

                break;
        }
    }

    public void GetRecruitInfo(int idGet, int levelGet, Transform pos)
    {
        id = idGet;
        level = levelGet;
        placeToRest = pos;
    }

    void Rest()
    {
        transform.position = Vector2.MoveTowards(transform.position, placeToRest.position, 1 * Time.deltaTime);
    }

    void LookForEnemies()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(placeToRest.position, 1, LayerMask.GetMask("Enemy"));
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
        if(enemyFocus != null)
        {
            recruitState = StateRecruit.goToEnemy;
        }
    }

    void goToEnemy()
    {
        transform.position = Vector2.MoveTowards(transform.position, enemyFocus.transform.position, 1 * Time.deltaTime);
        if(Vector2.Distance(transform.position,enemyFocus.transform.position) <= 0.1f)
        {
            recruitState = StateRecruit.attacking;
        }
    }

}
