using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSlide : MonoBehaviour
{
    float timer;
    Vector3 startPos;
    float cos;
    void Start()
    {
        timer = 0.683f;
        startPos = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        cos = timer / 0.683f;
        transform.position = new Vector3(startPos.x, startPos.y + cos, startPos.z);
        if(timer <= 0)
        {
            timer = 0.683f;
        }

    }
}
