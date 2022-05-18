using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public static UiManager InstanceUi;
    GameManager gM;
    [SerializeField] Transform buttMenu;
    [SerializeField] Text hp;
    [SerializeField] Button startLevelButt;
    [SerializeField] Text money;
    public enum TowerType
    {
        archer,
        bomber,
        canon
    }

    private void Awake()
    {
        if (InstanceUi != null)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceUi = this;
        }
    }

    void Start()
    {
        gM = GameManager.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        money.text = ""+gM.PlayerMoney;
    }

    public void UpdateHP(int currentHp)
    {
        hp.text = ""+currentHp;
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

    public void LevelStart()
    {
        startLevelButt.gameObject.SetActive(false);
        gM.ManageGameState(GameManager.GameStates.WaveTime);
    }

    public void ShowLevelStart()
    {
        startLevelButt.gameObject.SetActive(true);
    }
}
