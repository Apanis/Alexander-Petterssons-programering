using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Statistics : MonoBehaviour
{
    //Counters
    public TMP_Text DeathCounter;
    

    void Start()
    {
        DeathCounter.text = PlayerPrefs.GetInt("DEATHS").ToString();
    }

    
    void Update()
    {
        
    }
}
