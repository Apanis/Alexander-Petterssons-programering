using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeadCanvas : MonoBehaviour
{
    public GameObject DeadUI;
    public TextMeshProUGUI TimerUI;
    private float timeValue = 4;
    void Update()
    {
        TimerUI.text = ((int)timeValue).ToString();
        DeadUI.SetActive(!playerManager.isAlive);
        if(timeValue > 0 && !playerManager.isAlive)
        {
            TimerUI.enabled = true;
            timeValue -= Time.deltaTime;
        }
        else
        {
            TimerUI.enabled = false;
            timeValue = 4;
        }

    }
}
