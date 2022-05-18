using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    bool isPlaced;
    [SerializeField] float towerRange = 2;
    [SerializeField] GameObject towerRangeObj;
    [SerializeField] float towerDamage = 35;
    GameObject rangeObj;
    Color32 cor;
    [SerializeField] StateTower towerState;
    [SerializeField] Enemy enemyFocus;
    [SerializeField] GameObject shotPre;
    [SerializeField] int towerCost;
    GameManager gM;
    float timer = 0;
    bool canShot = true;

    enum StateTower
    {
        shooting,
        idle
    }

    void Start()
    {
        gM = GameManager.Instance;
        isPlaced = false;
        rangeObj = Instantiate(towerRangeObj);
        rangeObj.transform.localScale = new Vector3(towerRange*2,towerRange*2,towerRange*2);
        cor = gameObject.GetComponent<SpriteRenderer>().color;
        towerState = StateTower.idle;
    }

    void Update()
    {
        if(isPlaced == false && Input.GetKeyDown(KeyCode.Mouse0) && gM.PlayerMoney >= towerCost)
        {
            gM.PlayerMoney -= towerCost;
            isPlaced = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(cor.r, cor.g, cor.b, 255);
            Destroy(rangeObj);
        }
        if(isPlaced == false)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(cor.r, cor.g, cor.b, 100);
            transform.position = new Vector3(mousePosition.x,mousePosition.y,transform.position.z);
            rangeObj.transform.position = transform.position;
        }
        if(isPlaced == false && Input.GetKeyDown(KeyCode.Mouse1))
        {
            Destroy(rangeObj);
            Destroy(gameObject);
        }
        if(isPlaced == true)
        {
            switch (towerState)
            {
                case StateTower.idle:
                    FindEnemyToShoot();
                    break;
                case StateTower.shooting:
                    ShootTarget();
                    break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, towerRange);
    }

    private void FindEnemyToShoot()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, towerRange, LayerMask.GetMask("Enemy"));
        if(enemiesInRange.Length == 1)
        {
            enemyFocus = enemiesInRange[0].GetComponent<Enemy>();
            towerState = StateTower.shooting;
        }
        if(enemiesInRange.Length > 1)
        {
            enemyFocus = enemiesInRange[0].GetComponent<Enemy>();
            for (int i = 1; i < enemiesInRange.Length; i++)
            {
                if (enemiesInRange[i].GetComponent<Enemy>().DistanceTraveled > enemyFocus.DistanceTraveled)
                    enemyFocus = enemiesInRange[i].GetComponent<Enemy>();
            }
            towerState = StateTower.shooting;
        }
    }

    private void ShootTarget()
    {
        if(enemyFocus == null)
        {
            Debug.Log("dead");
            towerState = StateTower.idle;
            return;
        }
        if (Vector2.Distance(transform.position, enemyFocus.transform.position) >= towerRange + 0.2f)
        {
            enemyFocus = null;
            towerState = StateTower.idle;
        }
        else
        {
            if(canShot == true)
            {
                GameObject shot = Instantiate(shotPre, transform.position, Quaternion.identity);
                shot.GetComponent<Shot>().GetInfo(enemyFocus.gameObject, transform, towerDamage);
                StartCoroutine(ShootCooldown());
            }
        }
    }

    IEnumerator ShootCooldown()
    {
        canShot = false;
        yield return new WaitForSeconds(1);
        canShot = true;
    }
}
