using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    GameObject target;
    Transform initialPos;
    [SerializeField] float shotSpeed;
    float shotDamage;
    float interpolateAmount;
    Vector2 goToPos;
    Vector2 pos1;
    Vector2 pos2;

    void Start()
    {
        shotSpeed = 1.2f;
    }

    
    void Update()
    {
        if(target != null)
        {
            goToPos = target.transform.position;
        }
        interpolateAmount = interpolateAmount + Time.deltaTime * shotSpeed;
        pos1 = Vector2.Lerp(initialPos.position, new Vector2((initialPos.position.x + goToPos.x) / 2, (initialPos.position.y + goToPos.y) / 2) + new Vector2(0,1), interpolateAmount);
        pos2 = Vector2.Lerp(new Vector2((initialPos.position.x + goToPos.x) / 2, (initialPos.position.y + goToPos.y) / 2) + new Vector2(0, 1), goToPos, interpolateAmount);
        transform.position = Vector2.Lerp(pos1, pos2, interpolateAmount);
        transform.up = new Vector2(transform.position.x - pos2.x, transform.position.y - pos2.y);
        if (interpolateAmount >= 1)
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
