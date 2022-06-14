using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager: MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cartaText;
    [SerializeField] GameObject obj0;
    [SerializeField] GameObject obj2;
    [SerializeField] GameObject letter1;
    [SerializeField] GameObject letter2;
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
        }
        else if(clicks == 1)
        {
            obj0.SetActive(false);
            letter2.SetActive(true);
        }
        else if (clicks == 2)
        {
            letter2.SetActive(false);
            ChangeText("But they don't come for free, you need <color=#DC9A16> money </color> to buy them. You will get more money just by killing enemies and starting rounds earlier. Just don't let your <color=red>health</color> reach 0 and you'll win.");
            obj2.SetActive(true);
        }
        else if (clicks == 3)
        {
            letter1.SetActive(false);
            obj2.SetActive(false);
            this.gameObject.SetActive(false);
        }
        clicks++;
    }

    public void ChangeText(string change)
    {
        cartaText.text = change;
    }
}
