using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    GameObject target;
    Transform initialPos;
    [SerializeField] float shotSpeed;
    int elevation;
    float shotDamage;
    float interpolateAmount;
    string shotType;
    Vector2 goToPos;
    Vector2 pos1;
    Vector2 pos2;

    void Update()
    {
        if(target != null)
        {
            goToPos = target.transform.position;
        }
        interpolateAmount = interpolateAmount + Time.deltaTime * shotSpeed;
        pos1 = Vector2.Lerp(initialPos.position, new Vector2((initialPos.position.x + goToPos.x) / 2, (initialPos.position.y + goToPos.y) / 2) + new Vector2(0,elevation), interpolateAmount);
        pos2 = Vector2.Lerp(new Vector2((initialPos.position.x + goToPos.x) / 2, (initialPos.position.y + goToPos.y) / 2) + new Vector2(0, elevation), goToPos, interpolateAmount);
        transform.position = Vector2.Lerp(pos1, pos2, interpolateAmount);
        transform.up = new Vector2(transform.position.x - pos2.x, transform.position.y - pos2.y);
        if (interpolateAmount >= 1)
        {
            if(target != null)
            {
                if(shotType == "art")
                {
                    target.GetComponent<IDamagable>().TakeDamage(shotDamage/2, "armor");
                }
                else
                {
                    target.GetComponent<IDamagable>().TakeDamage(shotDamage, shotType);
                }
                
            }
            if(shotType == "art")
            {
                Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, 1f, LayerMask.GetMask("Enemy"));
                foreach (var item in enemiesInRange)
                {
                    item.GetComponent<IDamagable>().TakeDamage(shotDamage/2, "");
                } 
            }
            Destroy(gameObject);
        }
    }

    public void GetInfo(GameObject targetFunc, Transform initialPosFunc, float towerDamage, int elevationRecieved, string type)
    {
        target = targetFunc;
        initialPos = initialPosFunc;
        shotDamage = towerDamage;
        elevation = elevationRecieved;
        shotType = type;
        if(type == "art")
        {
            shotSpeed = 1f;
        }
        else
        {
            shotSpeed = 2f;
        }
    }
}
