using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] protected GameObject canvasUi;
    [SerializeField] protected Text towerCostUi;
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
            CheckClicked();
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
        Collider2D[] paths = Physics2D.OverlapCircleAll(transform.position, 0.5f, LayerMask.GetMask("Path"));
        Collider2D[] towers = Physics2D.OverlapCircleAll(transform.position, 1, LayerMask.GetMask("Tower"));
        if (paths.Length >= 1 || towers.Length >= 2)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            rangeObj.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 100);
            canPlace = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(cor.r, cor.g, cor.b, 100);
            rangeObj.GetComponent<SpriteRenderer>().color = new Color32(69, 255, 0, 100);
            canPlace = true;
        }
    }

    protected virtual void SpawnShot()
    {

    }

    void CheckClicked()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        if (Input.GetKeyDown(KeyCode.Mouse0) && Vector2.Distance(mousePosition, transform.position) < 0.5f)
        {
            canvasUi.SetActive(true);
            towerCostUi.text = ""+towerCost[towerLevel + 1];
        }

        if(Input.GetKeyDown(KeyCode.Mouse1) && canvasUi.activeSelf == true)
        {
            canvasUi.SetActive(false);
        }
    }

    public void LevelUpTower()
    {
        if(towerLevel < 2)
        {
            if (gM.PlayerMoney >= towerCost[towerLevel + 1])
            {
                gM.PlayerMoney -= towerCost[towerLevel + 1];
                towerLevel += 1;
                string name = gameObject.GetComponent<SpriteRenderer>().sprite.name;
                char[] myChars = name.ToCharArray();
                foreach (char item in name)
                {
                    if(item == 1)
                    {

                    }
                }

                for (int i = 0; i < myChars.Length; i++)
                {
                    if(myChars[i] == '1')
                    {
                        myChars[i] = '2';
                    }else
                    if (myChars[i] == '2')
                    {
                        myChars[i] = '3';
                    }
                }
                string nameFinal = new string(myChars);
                gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/InGame/" + nameFinal);
                canvasUi.SetActive(false);
            }
        }
    }

    public void DeActivateUi()
    {
        canvasUi.SetActive(false);
    }
}
