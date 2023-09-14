using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FPS/New Gun")]
public class gunInfo : itemInfo
{
    [HideInInspector]public float AKdamage;
    [HideInInspector]public float PistolDamage;
    public float damage;
    private void Awake()
    {
        AKdamage = PlayerPrefs.GetFloat("AKDAMAGE");
        PistolDamage = PlayerPrefs.GetFloat("PISTOLDAMAGE");
    }
}
