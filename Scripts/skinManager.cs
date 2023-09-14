using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class skinManager : MonoBehaviour
{
    public Image button1;
    public Image button2;
    public Image button3;
    public Image RifleButton1;
    public Image RifleButton2;
    public Image RifleButton3;

    public static skinManager instance;

    void Start()
    {
        instance = this;
        if(PlayerPrefs.GetString("Skin") != "PlayerController1" && PlayerPrefs.GetString("Skin") != "PlayerController2" && PlayerPrefs.GetString("Skin") != "PlayerController3")
        {
            Debug.Log("yes");
            PlayerPrefs.SetString("Skin", "PlayerController1");
        }
        if(PlayerPrefs.GetString("Skin") == "PlayerController1")
        {
            button1.color = Color.green;
            button2.color = Color.red;
            button3.color = Color.red;
        }
        if(PlayerPrefs.GetString("Skin") == "PlayerController2")
        {
            button1.color = Color.red;
            button2.color = Color.green;
            button3.color = Color.red;
        }
        if(PlayerPrefs.GetString("Skin") == "PlayerController3")
        {
            button1.color = Color.red;
            button2.color = Color.red;
            button3.color = Color.green;
        }

        if(PlayerPrefs.GetString("Weapon") != "Weapon1" || PlayerPrefs.GetString("Weapon") != "Weapon2" || PlayerPrefs.GetString("Weapon") != "Weapon3")
        {
            PlayerPrefs.SetString("Weapon", "Weapon1");
        }
        if(PlayerPrefs.GetString("Weapon") == "Weapon1")
        {
            RifleButton1.color = Color.green;
            RifleButton2.color = Color.red;
            RifleButton3.color = Color.red;
        }
        if(PlayerPrefs.GetString("Weapon") == "Weapon2")
        {
            RifleButton1.color = Color.red;
            RifleButton2.color = Color.green;
            RifleButton3.color = Color.red;
        }
        if(PlayerPrefs.GetString("Weapon") == "Weapon3")
        {
            RifleButton1.color = Color.red;
            RifleButton2.color = Color.red;
            RifleButton3.color = Color.green;
        }
        

    }
    
    public void RangerSelection1()
    {
        button1.color = Color.green;
        button2.color = Color.red;
        button3.color = Color.red;
        PlayerPrefs.SetString("Skin", "PlayerController1");
    }
    public void RangerSelection2()
    {
        button1.color = Color.red;
        button2.color = Color.green;
        button3.color = Color.red;
        PlayerPrefs.SetString("Skin", "PlayerController2");
    }
    public void RangerSelection3()
    {
        button1.color = Color.red;
        button2.color = Color.red;
        button3.color = Color.green;
        PlayerPrefs.SetString("Skin", "PlayerController3");
    }
    public void WeaponSelection1()
    {
        RifleButton1.color = Color.green;
        RifleButton2.color = Color.red;
        RifleButton3.color = Color.red;
        PlayerPrefs.SetString("Weapon", "Weapon1");
    }
    public void WeaponSelection2()
    {
        RifleButton1.color = Color.red;
        RifleButton2.color = Color.green;
        RifleButton3.color = Color.red;
        PlayerPrefs.SetString("Weapon", "Weapon2");
    }
    public void WeaponSelection3()
    {
        RifleButton1.color = Color.red;
        RifleButton2.color = Color.red;
        RifleButton3.color = Color.green;
        PlayerPrefs.SetString("Weapon", "Weapon3");
    }
}
