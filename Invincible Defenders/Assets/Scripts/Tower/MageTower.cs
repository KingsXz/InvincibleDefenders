using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageTower : Tower
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void SpawnShot()
    {
        int damageToDo = Random.Range(towerMinDamage[towerLevel], towerMaxDamage[towerLevel]);
        GameObject shot = Instantiate(shotPre, transform.position, Quaternion.identity);
        shot.GetComponent<Shot>().GetInfo(enemyFocus.gameObject, transform, damageToDo, 0, "magic");
        StartCoroutine(ShootCooldown());
    }
}
