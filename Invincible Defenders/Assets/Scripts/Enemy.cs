using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamagable
{
    [Header("Enemy Stats")]
    [SerializeField] float maxHp;
    [SerializeField] float currentHp;
    [SerializeField] float armor;
    [SerializeField] float magicRes;
    [SerializeField] int minDamage;
    [SerializeField] int maxDamage;
    [SerializeField] float attackSpeed;
    [SerializeField] float speed;
    [SerializeField] int moneyItGives;

    [Header("Other")]
    [SerializeField] PathCreator pathCreator;
    [SerializeField] float distanceTraveled;
    [SerializeField] Image hpBar;
    [SerializeField] GameObject recruitToFight;
    GameManager gM;

    [SerializeField] State enemyState;
    public enum State
    {
        Path,
        Wait,
        Attack,
        Fight
    }

    public PathCreator PathCreator { get => pathCreator; set => pathCreator = value; }
    public float DistanceTraveled { get => distanceTraveled; set => distanceTraveled = value; }
    public float MaxHp { get => maxHp; set => maxHp = value; }
    public State EnemyState { get => enemyState; set => enemyState = value; }
    public GameObject RecruitToFight { get => recruitToFight; set => recruitToFight = value; }

    void Start()
    {
        currentHp = maxHp;
        EnemyState = State.Path;
        gM = GameManager.Instance;
    }

    void Update()
    {
        hpBar.fillAmount = (currentHp * 100 / maxHp) / 100;
        switch (enemyState)
        {
            case State.Path:
                FollowPath();
                break;
            case State.Wait:
                CheckRecruit();
                break;
            case State.Attack:
                CheckRecruit();
                AttackRecruit();
                break;
            case State.Fight:
                CheckRecruit();
                break;
        }
    }

    void FollowPath()
    {
        DistanceTraveled += speed * Time.deltaTime;
        transform.position = PathCreator.path.GetPointAtDistance(DistanceTraveled);
    }

    public void TakeDamage(float damageToTake, string type)
    {
        if(type == "armor")
        {
            currentHp = currentHp - (damageToTake * armor);
        }
        else if(type == "magic")
        {
            currentHp = currentHp - (damageToTake * magicRes);
        }
        else
        {
            currentHp = currentHp - damageToTake;
        }
        if(currentHp <= 0)
        {
            gM.PlayerMoney += moneyItGives;
            gM.EnemiesAlive--;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Base")
        {
            collision.gameObject.GetComponent<IDamagable>().TakeDamage(1f, "");
            gM.EnemiesAlive--;
            Destroy(gameObject);
        }
    }

    public void GetRecruit(GameObject rec)
    {
        RecruitToFight = rec;
    }

    void CheckRecruit()
    {
        if(recruitToFight == null)
        {
            enemyState = State.Path;
        }
    }

    void AttackRecruit()
    {
        StartCoroutine(Attack());
        enemyState = State.Fight;
    }

    IEnumerator Attack()
    {
        recruitToFight.GetComponent<IDamagable>().TakeDamage(Random.Range(minDamage, maxDamage), "");
        yield return new WaitForSeconds(1);
        if (enemyState == State.Fight)
        {
            StartCoroutine(Attack());
        }
    }
}
