using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPScounter : MonoBehaviour
{
    public TMP_Text fpsText;
    public float timer, refresh, avgFramerate;
    public string display = "{0} FPS";
    void Start()
    {
        
    }
    void Update()
    {
        if(PlayerPrefs.GetString("FPS") == "True")
            fpsText.enabled = true;
        else
            fpsText.enabled = false;

        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;

        if(timer <=0) avgFramerate = (int) (1f / timelapse);

        fpsText.text = string.Format(display, avgFramerate.ToString());
    }
   
}
