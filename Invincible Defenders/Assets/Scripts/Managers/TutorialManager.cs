using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager: MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cartaText;
    [SerializeField] GameObject obj0;
    int clicks;
    void Start()
    {
        clicks = 0;
    }

    void Update()
    {
        
    }

    public void Click()
    {
        if(clicks == 0)
        {
            obj0.SetActive(true);
            ChangeText("You simply have to stop all the demons before they reach <color=blue>our gates</color>.In order to do that you will have to place towers to stop them, you can buy them by clicking on the <color=red>tower icon</color> on your bottom right corner, then chose the tower you want to place on the field.");    
            clicks++;
        }
        else if(clicks == 1)
        {
            obj0.SetActive(false);
        }
    }

    public void ChangeText(string change)
    {
        cartaText.text = change;
    }
}
