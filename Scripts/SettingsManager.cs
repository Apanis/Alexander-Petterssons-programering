using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public Toggle fpsToggle;
    public Toggle NamesToggle;
    public Toggle CrosshairToggle;
    public Toggle FullscreneToggle;
    public Slider FovSliderUI;
    public Slider SensSlider;
    public TMP_Text FovNumbertext;
    public TMP_Text SensNumberText;
    public GameObject ColorSystem;
    
    void Start()
    {
        if (PlayerPrefs.GetString("FULLSCREEN") == "True")
            FullscreneToggle.isOn = true;
        else
            FullscreneToggle.isOn = false;
        if (FullscreneToggle.isOn)
            PlayerPrefs.SetString("FULLSCREEN", "True");
        else
            PlayerPrefs.SetString("FULLSCREEN", "False");
        if (PlayerPrefs.GetString("FULLSCREEN") == "True")
            Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        else
            Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);

        if (CrosshairToggle.isOn)
            PlayerPrefs.SetString("CROSSHAIR", "True");
        else
            PlayerPrefs.SetString("CROSSHAIR", "False");

        if(NamesToggle.isOn)
            PlayerPrefs.SetString("NAMES", "True");
        else
            PlayerPrefs.SetString("NAMES", "False");
        if(NamesToggle.isOn)
            PlayerPrefs.SetString("NAMES", "True");
        else
            PlayerPrefs.SetString("NAMES", "False");
        ColorSystem.SetActive(false);
        SensSlider.value = PlayerPrefs.GetFloat("SENSITIVITY");
        FovSliderUI.value = PlayerPrefs.GetFloat("FOV");
        if(PlayerPrefs.GetString("FPS") == "True")
            fpsToggle.isOn = true;
        else
            fpsToggle.isOn = false;

        if(PlayerPrefs.GetString("NAMES") == "True")
            NamesToggle.isOn = true;
        else
            NamesToggle.isOn = false;
        if(PlayerPrefs.GetString("CROSSHAIR") == "True")
            CrosshairToggle.isOn = true;
        else
            CrosshairToggle.isOn = false;
    }

    public void FpsToggle(bool FpsOn)
    {
        //Debug.Log(FpsOn);
        PlayerPrefs.SetString("FPS", FpsOn.ToString());
    }

    public void NameToggle(bool NamesOn)
    {
        //Debug.Log(NamesOn);
        PlayerPrefs.SetString("NAMES", NamesOn.ToString());
    }
    public void CrosshairToggleMethod(bool CrosshairOn)
    {
        //Debug.Log(CrosshairOn);
        PlayerPrefs.SetString("CROSSHAIR", CrosshairOn.ToString());
    }
    public void FovSlider(float Fov)
    {
        //Debug.Log(Fov);
        PlayerPrefs.SetFloat("FOV", Fov);
        FovNumbertext.text = FovSliderUI.value.ToString() + " Field of view";
    }
    public void SensitivitySlider(float Sens)
    {
        //Debug.Log(Sens);
        PlayerPrefs.SetFloat("SENSITIVITY", Sens);
        SensNumberText.text = SensSlider.value.ToString() + " Mouse sensitivity";
    }
    public void ChangeColorButton()
    {
        if(ColorSystem.activeSelf == true)
        {
            ColorSystem.SetActive(false);
        }
        else
        {
            ColorSystem.SetActive(true);
        }
    }
    public void SetFullscreen(bool _IsFullScreen)
    {
        if (_IsFullScreen == true)
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
            PlayerPrefs.SetString("FULLSCREEN", "True");
        }
        else
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
            PlayerPrefs.SetString("FULLSCREEN", "False");
        }
            
    }
}
