using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] Transform buttMenu;
    public enum TowerType
    {
        archer,
        bomber,
        canon
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenTowerMenu()
    {
        foreach(Transform child in buttMenu)
        {
            Button but = child.GetComponent<Button>();
            but.gameObject.SetActive(true);
        }
    }

    public void ButtonSelected(string towerName)
    {
        foreach (Transform child in buttMenu)
        {
            Button but = child.GetComponent<Button>();
            but.gameObject.SetActive(false);
        }
        GameObject tower = Resources.Load("PreFabs/Towers/"+towerName) as GameObject;
        Instantiate(tower);
    }
}
