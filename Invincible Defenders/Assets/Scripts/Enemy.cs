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

    [Header("Enemy Stats")]
    [SerializeField] PathCreator pathCreator;
    [SerializeField] float distanceTraveled;
    [SerializeField] Image hpBar;
    GameManager gM;

    [SerializeField]State enemyState;
    enum State
    {
        Path,
        Fight
    }

    public PathCreator PathCreator { get => pathCreator; set => pathCreator = value; }
    public float DistanceTraveled { get => distanceTraveled; set => distanceTraveled = value; }
    public float MaxHp { get => maxHp; set => maxHp = value; }

    void Start()
    {
        currentHp = maxHp;
        enemyState = State.Path;
        gM = GameManager.Instance;
    }

    void Update()
    {
        hpBar.fillAmount = (currentHp * 100 / maxHp) / 100;
        if(enemyState == State.Path)
        {
            FollowPath();
        }
        else if(enemyState == State.Fight)
        {

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
            Debug.Log("armorDamage");
            currentHp = currentHp - (damageToTake * armor);
        }else if(type == "magic")
        {
            Debug.Log("magicDamage");
            currentHp = currentHp - (damageToTake * magicRes);
        }else
        {
            Debug.Log("noTypeDamage");
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
}
