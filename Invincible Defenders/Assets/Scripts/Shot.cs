using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    GameObject target;
    Transform initialPos;
    [SerializeField] float shotSpeed = 1.2f;
    float shotDamage;
    float interpolateAmount;
    Vector2 goToPos;

    void Start()
    {
        
    }

    
    void Update()
    {
        if(target != null)
        {
            goToPos = target.transform.position;
        }
        interpolateAmount = interpolateAmount + Time.deltaTime * shotSpeed;
        transform.position = Vector2.Lerp(initialPos.position, goToPos, interpolateAmount);
        if(interpolateAmount >= 1)
        {
            if(target != null)
            {
                target.GetComponent<IDamagable>().TakeDamage(shotDamage);
            }
            Destroy(gameObject);
        }
    }

    public void GetInfo(GameObject targetFunc, Transform initialPosFunc, float towerDamage)
    {
        target = targetFunc;
        initialPos = initialPosFunc;
        shotDamage = towerDamage;
    }
}
