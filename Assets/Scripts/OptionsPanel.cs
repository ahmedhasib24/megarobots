using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour
{
    [SerializeField] private List<Toggle> GraphicQualityList = new List<Toggle>();

    [SerializeField] private Image SFXImg;
    [SerializeField] private Slider SFXSlider;

    [SerializeField] private Image MusicImg;
    [SerializeField] private Slider MusicSlider;

    // Start is called before the first frame update
    void Start()
    {
        GetGraphic();

        SFXSlider.value = GetSFXVolume();
        MusicSlider.value = GetMusicVolume();
    }

    void Update()
    {
        SetVolume(SFXSlider.value, MusicSlider.value);
    }

    public void GetGraphic()
    {
        for (int i = 0; i < GraphicQualityList.Count; i++)
        {
            if (i == PlayerPrefs.GetInt("Graphic"))
            {
                GraphicQualityList[i].isOn = true;
                QualitySettings.SetQualityLevel(i);
            }
            else
            {
                GraphicQualityList[i].isOn = false;
            }
        }
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat("SFX");
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("Music");
    }

    public void SetVolume(float fSFX, float fMusic)
    {
        PlayerPrefs.SetFloat("SFX", fSFX);
        PlayerPrefs.SetFloat("Music", fMusic);

        OptionManager.instance.SetVolume(GetSFXVolume(), GetMusicVolume());
        SFXImg.fillAmount = GetSFXVolume();
        MusicImg.fillAmount = GetMusicVolume();
    }

    public void OKBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        for (int i = 0; i < GraphicQualityList.Count; i++)
        {

            if (GraphicQualityList[i].isOn)
            {
                PlayerPrefs.SetInt("Graphic", i);
                QualitySettings.SetQualityLevel(i);
            }
        }

        PlayerPrefs.SetFloat("SFX", GetSFXVolume());
        PlayerPrefs.SetFloat("Music", GetMusicVolume());

        MenuUIManager.instance.ShowPanel(0);
    }

    public void CloseBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        MenuUIManager.instance.ShowPanel(0);
    }
}
