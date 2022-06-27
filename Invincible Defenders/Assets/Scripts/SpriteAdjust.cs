using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAdjust : MonoBehaviour
{
    [SerializeField] float posX;
    [SerializeField] float posY;
    float rand;
    void Start()
    {
        rand = Random.Range(0,100);
        rand = rand / 100;
    }

    // Update is called once per frame
    void Update()
    {   
        transform.position = new Vector3(transform.position.x , transform.position.y, rand);      
    }
}
