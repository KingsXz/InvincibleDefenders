using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Stats")]
    bool isPlaced;
    [SerializeField] protected int towerLevel = 0;
    [SerializeField] protected float[] towerRange = new float[3];
    [SerializeField] protected int[] towerCost = new int[3];
    [SerializeField] protected int[] towerMinDamage = new int[3];
    [SerializeField] protected int[] towerMaxDamage = new int[3];
    [SerializeField] protected float[] towerShotCooldown = new float[3];

    [Header("Other")]
    [SerializeField] protected StateTower towerState;
    [SerializeField] protected Enemy enemyFocus;
    [SerializeField] protected GameObject shotPre;
    [SerializeField] protected GameObject towerRangeObj;
    GameManager gM;
    bool canShot = true;
    bool canPlace = true;
    GameObject rangeObj;
    Color32 cor;

    protected enum StateTower
    {
        shooting,
        idle
    }

    protected virtual void Start()
    {
        gM = GameManager.Instance;
        isPlaced = false;
        rangeObj = Instantiate(towerRangeObj);
        rangeObj.transform.localScale = new Vector3(towerRange[towerLevel]*2,towerRange[towerLevel]* 2,towerRange[towerLevel]* 2);
        cor = gameObject.GetComponent<SpriteRenderer>().color;
        towerState = StateTower.idle;
        gameObject.GetComponent<SpriteRenderer>().color = new Color32(cor.r, cor.g, cor.b, 100);
    }

    protected virtual void Update()
    {
        if (isPlaced == true)
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
        else
        {
            if (isPlaced == false && Input.GetKeyDown(KeyCode.Mouse0) && gM.PlayerMoney >= towerCost[towerLevel] && canPlace == true)
            {
                gM.PlayerMoney -= towerCost[towerLevel];
                isPlaced = true;
                gameObject.GetComponent<SpriteRenderer>().color = new Color32(cor.r, cor.g, cor.b, 255);
                Destroy(rangeObj);
            }
            if (isPlaced == false)
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
                rangeObj.transform.position = transform.position;
                CheckPlace();
            }
            if (isPlaced == false && Input.GetKeyDown(KeyCode.Mouse1))
            {
                Destroy(rangeObj);
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, towerRange[towerLevel]);
    }

    private void FindEnemyToShoot()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, towerRange[towerLevel], LayerMask.GetMask("Enemy"));
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
            towerState = StateTower.idle;
            return;
        }
        if (Vector2.Distance(transform.position, enemyFocus.transform.position) >= towerRange[towerLevel] + 0.2f)
        {
            enemyFocus = null;
            towerState = StateTower.idle;
        }
        else
        {
            if(canShot == true)
            {
                SpawnShot();
            }
        }
    }

    protected IEnumerator ShootCooldown()
    {
        canShot = false;
        yield return new WaitForSeconds(towerShotCooldown[towerLevel]);
        canShot = true;
    }

    void CheckPlace()
    {
        Collider2D[] paths = Physics2D.OverlapCircleAll(transform.position, 0.8f, LayerMask.GetMask("Path"));
        Collider2D[] towers = Physics2D.OverlapCircleAll(transform.position, 1, LayerMask.GetMask("Tower"));
        if (paths.Length >= 1 || towers.Length >= 2)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            canPlace = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(cor.r, cor.g, cor.b, 100);
            canPlace = true;
        }
    }

    protected virtual void SpawnShot()
    {

    }
}
