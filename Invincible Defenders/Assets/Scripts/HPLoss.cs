using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPLoss : MonoBehaviour, IDamagable
{
    GameManager gM;

    private void Start()
    {
        gM = GameManager.Instance;    
    }

    public void TakeDamage(float damageToTake)
    {
        gM.PlayerHp -= damageToTake;
    }
}
