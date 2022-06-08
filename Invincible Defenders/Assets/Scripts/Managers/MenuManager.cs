using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] Image menu;

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    } 

    public void OpenMenu()
    {
        menu.gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        menu.gameObject.SetActive(false);
    }
}
