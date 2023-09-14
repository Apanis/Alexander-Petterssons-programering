using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class WeaponManager : MonoBehaviour
{
    public Image Rifle1;
    public Image Rifle2;
    public Image Pistol1;
    public Image Pistol2;
    public Image Sniper1;
    public Image Sniper2;

    public GameObject RifleMenu;
    public GameObject PistolMenu;
    public GameObject SniperMenu;

    private void Start()
    {
        RifleMenu.SetActive(true);
        PistolMenu.SetActive(false);
        SniperMenu.SetActive(false);
        if (PlayerPrefs.GetFloat("AKCOOLDOWN") == 0.08f)
        {
            Rifle1.color = Color.green;
            Rifle2.color = Color.red;
        }
        else
        {
            Rifle1.color = Color.red;
            Rifle2.color = Color.green;
        }
        if (PlayerPrefs.GetFloat("PISTOLCOOLDOWN") == 0.25f)
        {
            Pistol1.color = Color.green;
            Pistol2.color = Color.red;
        }
        else
        {
            Pistol1.color = Color.red;
            Pistol2.color = Color.green;
        }

    }
    //RIFLE
    public void RifleSetting1()
    {
        Rifle1.color = Color.green;
        Rifle2.color = Color.red;
        PlayerPrefs.SetFloat("AKCOOLDOWN", 0.08f);
        PlayerPrefs.SetFloat("AKDAMAGE", 15f);
    }
    public void RifleSetting2()
    {
        Rifle1.color = Color.red;
        Rifle2.color = Color.green;
        PlayerPrefs.SetFloat("AKCOOLDOWN", 0.3f);
        PlayerPrefs.SetFloat("AKDAMAGE", 40f);
    }
    //PISTOL
    public void PistolSetting1()
    {
        Pistol1.color = Color.green;
        Pistol2.color = Color.red;
        PlayerPrefs.SetFloat("PISTOLCOOLDOWN", 0.25f);
        PlayerPrefs.SetFloat("PISTOLDAMAGE", 26f);
    }
    public void PistolSetting2()
    {
        Pistol1.color = Color.red;
        Pistol2.color = Color.green;
        PlayerPrefs.SetFloat("PISTOLCOOLDOWN", 0.75f);
        PlayerPrefs.SetFloat("PISTOLDAMAGE", 50);
    }
    //SNIPER
    public void SniperSettings1()
    {
        Sniper1.color = Color.green;
        Sniper2.color = Color.red;
        PlayerPrefs.SetFloat("SNIPERCOOLDOWN", 0.55f);
        PlayerPrefs.SetFloat("SNIPERDAMAGE", 35);
    }
    public void SniperSettings2()
    {
        Sniper1.color = Color.red;
        Sniper2.color = Color.green;
        PlayerPrefs.SetFloat("SNIPERCOOLDOWN", 1.2f);
        PlayerPrefs.SetFloat("SNIPERDAMAGE", 100);
    }
    //WEAPON MENUS
    public void RifleSettingsEnabled()
    {
        RifleMenu.SetActive(true);
        PistolMenu.SetActive(false);
        SniperMenu.SetActive(false);
    }
    public void PistolSettingsEnabled()
    {
        RifleMenu.SetActive(false);
        PistolMenu.SetActive(true);
        SniperMenu.SetActive(false);
    }
    public void SniperSettingsEnabled()
    {
        RifleMenu.SetActive(false);
        PistolMenu.SetActive(false);
        SniperMenu.SetActive(true);
    }
}
