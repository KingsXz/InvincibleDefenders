using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] Image menu;
    [SerializeField] Image audio;

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    } 

    public void OpenMenu()
    {
        menu.gameObject.SetActive(true);
    }

    public void OpenAudio()
    {
        menu.gameObject.SetActive(false);
        audio.gameObject.SetActive(true);
    }

    public void CloseAudio()
    {
        menu.gameObject.SetActive(true);
        audio.gameObject.SetActive(false);
    }

    public void CloseMenu()
    {
        menu.gameObject.SetActive(false);
    }
}
