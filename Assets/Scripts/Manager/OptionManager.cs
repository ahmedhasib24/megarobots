using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shared;

public class OptionManager : MonoBehaviour
{
    public static OptionManager instance;

    public AudioClip login, main, singlePlayer, multiPlayer;

    [SerializeField] private AudioSource SFXAudio;
    [SerializeField] private AudioSource MusicAudio;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start()
    {
        if (!PlayerPrefs.HasKey("Graphic")) PlayerPrefs.SetInt("Graphic", 0);
        if (!PlayerPrefs.HasKey("SFX")) PlayerPrefs.SetFloat("SFX", 1);
        if (!PlayerPrefs.HasKey("Music")) PlayerPrefs.SetFloat("Music", 1);

        SetVolume(PlayerPrefs.GetFloat("SFX"), PlayerPrefs.GetFloat("Music"));

    }

    public void SetVolume(float sfxVol, float musicVol)
    {
        SFXAudio.volume = sfxVol;
        MusicAudio.volume = musicVol;
    }

    public void PlayLoginBG()
    {
        MusicAudio.Stop();
        MusicAudio.clip = login;
        MusicAudio.Play();
    }

    public void PlayMainBG()
    {
        MusicAudio.Stop();
        MusicAudio.clip = main;
        MusicAudio.Play();
    }

    public void PlaySinglePlayerBG()
    {
        MusicAudio.Stop();
        MusicAudio.clip = singlePlayer;
        MusicAudio.Play();
    }

    public void PlayMultiPlayerBG()
    {
        MusicAudio.Stop();
        MusicAudio.clip = multiPlayer;
        MusicAudio.Play();
    }

    public void ButtonPlay()
    {
        if (!SFXAudio.isPlaying)
        {
            SFXAudio.Play();
        }
    }

    public void StopMusic()
    {
        MusicAudio.Stop();
    }
}
