using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitTower : MonoBehaviour
{
    [Header("Tower Stats")]
    [SerializeField] int towerLevel = 0;
    [SerializeField] float[] towerRange = new float[3];
    [SerializeField] int[] towerCost = new int[3];
    [SerializeField] int[] recruitMinDamage = new int[3];
    [SerializeField] int[] recruitMaxDamage = new int[3];
    [SerializeField] int[] recruitMaxHp = new int[3];

    [Header("Other")]
    [SerializeField] StateRecruitTower towerStateRec;
    [SerializeField] GameObject towerRangeObj;
    [SerializeField] GameObject rallyObj;
    [SerializeField] Transform[] rallyPos;
    [SerializeField] GameObject[] recruits = new GameObject[3];
    [SerializeField] GameObject recruitPre;

    bool rallyPlacedOnce = false;
    GameManager gM;
    GameObject rangeObj;
    bool canPlace = true;
    Color32 cor;

    enum StateRecruitTower
    {
        placing,
        placingRally,
        done
    }

    void Start()
    {
        gM = GameManager.Instance;
        rangeObj = Instantiate(towerRangeObj);
        rangeObj.transform.localScale = new Vector3(towerRange[towerLevel] * 2, towerRange[towerLevel] * 2, towerRange[towerLevel] * 2);
        cor = gameObject.GetComponent<SpriteRenderer>().color;
        gameObject.GetComponent<SpriteRenderer>().color = new Color32(cor.r, cor.g, cor.b, 100);
    }

    void Update()
    {
        switch (towerStateRec)
        {
            case StateRecruitTower.placing:
                PlacingTower();
                break;
            case StateRecruitTower.placingRally:
                PlacingRally();
                break;
            case StateRecruitTower.done:
                
                break;
        }
    }

    void PlacingTower()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        rangeObj.transform.position = transform.position;
        CheckPlace();
        if (Input.GetKeyDown(KeyCode.Mouse0) && gM.PlayerMoney >= towerCost[towerLevel] && canPlace == true)
        {
            gM.PlayerMoney -= towerCost[towerLevel];
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(cor.r, cor.g, cor.b, 255);
            towerStateRec = StateRecruitTower.placingRally;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Destroy(rangeObj);
            Destroy(gameObject);
        }
    }

    void PlacingRally()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        rallyObj.transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        if (CheckRallyPlace() == true && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (rallyPlacedOnce == false)
            {
                StartCoroutine(SpawnRecruit(0.1f, 3, 0));
                rallyPlacedOnce = true;
            }
            else
            {
                
            }
            Destroy(rangeObj);
            towerStateRec = StateRecruitTower.done;
        }
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

    bool CheckRallyPlace()
    {
        if(Vector2.Distance(transform.position, rallyObj.transform.position) > towerRange[towerLevel])
        {
            rallyObj.GetComponent<SpriteRenderer>().color = Color.red;
            return false;
        }
        else
        {
            rallyObj.GetComponent<SpriteRenderer>().color = Color.white;
            return true;
        }
    }

    public void SpawnRec(float secs, int num, int id)
    {
        StartCoroutine(SpawnRecruit(secs, num, id));
    }

    public IEnumerator SpawnRecruit(float secs, int num, int id) // if you wanna spawn only 1 recruit with a specific id put num = 0
    {
        if (num != 0)
        {
            for (int i = 0; i < num; i++)
            {
                yield return new WaitForSeconds(secs);
                recruits[i] = Instantiate(recruitPre, transform.position, Quaternion.identity);
                recruits[i].GetComponent<RecruitAI>().GetRecruitInfoSpawn(i, rallyPos[i], recruitMaxHp[towerLevel], recruitMinDamage[towerLevel], recruitMaxDamage[towerLevel], this.gameObject);
            }
        }
        else
        {
            Debug.Log(secs);
            yield return new WaitForSeconds(secs);
            Debug.Log("depois");
            recruits[id] = Instantiate(recruitPre, transform.position, Quaternion.identity);
            recruits[id].GetComponent<RecruitAI>().GetRecruitInfoSpawn(id, rallyPos[id] , recruitMaxHp[towerLevel], recruitMinDamage[towerLevel], recruitMaxDamage[towerLevel], this.gameObject);
        }
    }
}
