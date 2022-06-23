using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    bool deployed = false;
    GameManager.AbilityType abilityType;
    [SerializeField] GameObject spikesAnim;
    [SerializeField] GameObject rockAnim;
    Vector3 mousePosition;
    [SerializeField] Image calImg;
    [SerializeField] Image rockImg;
    [SerializeField] Image poisImg;
    GameManager gM;

    void Start()
    {
        gM = GameManager.Instance;
        deployed = false;
    }

    void Update()
    {
        if (deployed == false && Input.GetKeyDown(KeyCode.Mouse0))
        {
            deployed = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(0,0,0,0);
            StartCoroutine(Effect());
            Collider2D[] enemiesDetected = Physics2D.OverlapCircleAll(transform.position, 0.5f, LayerMask.GetMask("Enemy"));
            foreach (var item in enemiesDetected)
            {
                StartCoroutine(DoDamage(item.GetComponent<Enemy>()));
            }
        }
        if (deployed == false)
        {
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }
        if (deployed == false && Input.GetKeyDown(KeyCode.Mouse1))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Effect()
    {
        gM.EnableCooldonw = true;
        switch (abilityType)
        {
            case GameManager.AbilityType.Caltrops:
                GameObject spikeIns = Instantiate(spikesAnim, new Vector3(mousePosition.x, mousePosition.y, transform.position.z), Quaternion.identity);
                Destroy(spikeIns, 2);
                break;
            case GameManager.AbilityType.PoisonBomb:
                gameObject.GetComponent<SpriteRenderer>().color = new Color32(109, 0, 126, 100);

                break;
            case GameManager.AbilityType.RockSlide:
                GameObject rockIns = Instantiate(rockAnim, new Vector3(mousePosition.x, mousePosition.y, transform.position.z), Quaternion.identity);
                Destroy(rockIns, 2);
                break;
        }
       
        yield return new WaitForSeconds(2);
        Collider2D[] enemiesDetected = Physics2D.OverlapCircleAll(transform.position, 0.5f, LayerMask.GetMask("Enemy"));
        foreach (var item in enemiesDetected)
        {
            switch (abilityType)
            {
                case GameManager.AbilityType.Caltrops:
                    //Destroy(spikeIns);
                    item.GetComponent<Enemy>().ApplyDebuf(Enemy.DebufType.Slow, 2);
                    break;
                case GameManager.AbilityType.PoisonBomb:
                    item.GetComponent<Enemy>().ApplyDebuf(Enemy.DebufType.LoseMr, 2);
                    break;
                case GameManager.AbilityType.RockSlide:
                    item.GetComponent<Enemy>().ApplyDebuf(Enemy.DebufType.LoseArmor, 2);
                    break;
            }
        }
        Destroy(gameObject);
    }

    IEnumerator DoDamage(Enemy enem)
    {
        if(enem != null)
        {
            switch (abilityType)
            {
                case GameManager.AbilityType.Caltrops:
                    enem.GetComponent<IDamagable>().TakeDamage(1, "caltrops");
                    break;
                case GameManager.AbilityType.PoisonBomb:
                    enem.GetComponent<IDamagable>().TakeDamage(1, "poison");
                    break;
                case GameManager.AbilityType.RockSlide:
                    enem.GetComponent<IDamagable>().TakeDamage(1, "rock");
                    break;
            }
            yield return new WaitForSeconds(0.5f);
            if (enem != null)
            {
                if (Vector2.Distance(enem.transform.position, transform.position) <= 0.5f)
                {
                    StartCoroutine(DoDamage(enem));
                }
                else
                {
                    switch (abilityType)
                    {
                        case GameManager.AbilityType.Caltrops:
                            enem.ApplyDebuf(Enemy.DebufType.Slow, 2);
                            break;
                        case GameManager.AbilityType.PoisonBomb:
                            enem.ApplyDebuf(Enemy.DebufType.LoseMr, 2);
                            break;
                        case GameManager.AbilityType.RockSlide:
                            enem.ApplyDebuf(Enemy.DebufType.LoseArmor, 2);
                            break;
                    }
                }
            }
        }    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(deployed == true)
        {
            Enemy enemyScript = collision.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                StartCoroutine(DoDamage(enemyScript));
            }
        }
    }

    public void GetAbilityType(GameManager.AbilityType type)
    {
        abilityType = type;
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (deployed == true)
        {
            Enemy enemyScript = collision.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemiesInRange.Remove(enemyScript);
            }
        }
    }*/


}
