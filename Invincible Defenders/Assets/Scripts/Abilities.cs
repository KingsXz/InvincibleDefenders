using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    bool deployed = false;
    [SerializeField] List<Enemy> enemiesInRange = new List<Enemy>();

    void Start()
    {
        deployed = false;
    }

    void Update()
    {
        if(deployed == true)
        {
            
            
        }
        if (deployed == false && Input.GetKeyDown(KeyCode.Mouse0))
        {
            deployed = true;
            StartCoroutine(Effect());
            InvokeRepeating("DoDamage", 0, 0.2f);
            /*Collider2D[] enemiesDetected = Physics2D.OverlapCircleAll(transform.position, 0.5f, LayerMask.GetMask("Enemy"));
            foreach (var item in enemiesDetected)
            {
                enemiesInRange.Add(item.GetComponent<Enemy>());
            }*/
        }
        if (deployed == false)
        {
            Vector3 mousePosition = Input.mousePosition;
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
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    void DoDamage()
    {
        foreach (var item in enemiesInRange)
        {
            item.TakeDamage(1, "");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(deployed == true)
        {
            Enemy enemyScript = collision.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemiesInRange.Add(enemyScript);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (deployed == true)
        {
            Enemy enemyScript = collision.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemiesInRange.Remove(enemyScript);
            }
        }
    }


}
