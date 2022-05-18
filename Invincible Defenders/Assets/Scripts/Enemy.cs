using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamagable
{

    [SerializeField] PathCreator pathCreator;
    [SerializeField] float speed;
    [SerializeField] float maxHp;
    [SerializeField] float currentHp;
    [SerializeField] Image hpBar;
    [SerializeField] float distanceTraveled;
    [SerializeField] int moneyItGives;
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

    public void TakeDamage(float damageToTake)
    {
        currentHp = currentHp - damageToTake;
        if(currentHp <= 0)
        {
            gM.PlayerMoney += moneyItGives;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("col");
        if (collision.gameObject.tag == "Base")
        {
            collision.gameObject.GetComponent<IDamagable>().TakeDamage(1f);
            Destroy(gameObject);
        }
    }
}
