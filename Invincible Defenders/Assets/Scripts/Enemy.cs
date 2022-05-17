using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamagable
{

    [SerializeField] PathCreator pathCreator;
    [SerializeField] float speed;
    [SerializeField] float hp;
    [SerializeField] Image hpBar;
    [SerializeField] float distanceTraveled;

    [SerializeField]State enemyState;
    enum State
    {
        Path,
        Fight
    }

    public PathCreator PathCreator { get => pathCreator; set => pathCreator = value; }
    public float DistanceTraveled { get => distanceTraveled; set => distanceTraveled = value; }
    public float Hp { get => hp; set => hp = value; }

    void Start()
    {
        Hp = 100;
        enemyState = State.Path;
    }

    void Update()
    {
        hpBar.fillAmount = Hp / 100;
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
        Hp = Hp - damageToTake;
        if(Hp <= 0)
        {
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
