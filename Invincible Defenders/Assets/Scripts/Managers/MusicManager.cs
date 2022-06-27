using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager InstanceMusic;
    private static AudioClip musicMainMenu, inGameMusic;
    static AudioSource musicSrc;
    private float musicVolume = 1f;

    private void Awake()
    {
        if (InstanceMusic != null)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceMusic = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }

    void Start()
    {
        musicSrc = GetComponent<AudioSource>();

        musicMainMenu = Resources.Load<AudioClip>("Sounds/Music");
        inGameMusic = Resources.Load<AudioClip>("Sounds/InGameMusic");

        PlayMusic("MenuMusic");
    }

    private void Update()
    {
        musicSrc.volume = musicVolume;

    }

    public void PlayMusic(string clip)
    {
        musicSrc.Stop();
        switch (clip)
        {
            case "MenuMusic":
                musicSrc.PlayOneShot(musicMainMenu);
                musicSrc.loop = true;
                break;
            case "InGameMusic":
                musicSrc.PlayOneShot(inGameMusic);
                musicSrc.loop = true;
                break;
        }
    }

    public void GetMusicVolume(float volume)
    {
        musicVolume = volume;
    }

}
