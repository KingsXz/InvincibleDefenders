using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    [SerializeField] PathCreator pathCreator;
    [SerializeField] float speed;
    [SerializeField] float hp;
    [SerializeField] Image hpBar;
    float distanceTraveled;

    [SerializeField]State enemyState;
    enum State
    {
        Path,
        Fight
    }

    public PathCreator PathCreator { get => pathCreator; set => pathCreator = value; }
    public float DistanceTraveled { get => distanceTraveled; set => distanceTraveled = value; }

    void Start()
    {
        hp = 100;
        enemyState = State.Path;
    }

    void Update()
    {
        hpBar.fillAmount = hp / 100;
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
}
