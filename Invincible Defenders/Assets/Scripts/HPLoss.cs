using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPLoss : MonoBehaviour, IDamagable
{
    GameManager gM;
    UiManager uI;

    private void Start()
    {
        gM = GameManager.Instance;
        uI = UiManager.InstanceUi;
    }

    public void TakeDamage(float damageToTake)
    {
        gM.PlayerHp -= (int)damageToTake;
        UiManager.InstanceUi.UpdateHP(gM.PlayerHp);
        if(gM.PlayerHp <= 0)
        {
            Time.timeScale = 0;
            uI.ActivateLoseUi();
        }
    }
}
