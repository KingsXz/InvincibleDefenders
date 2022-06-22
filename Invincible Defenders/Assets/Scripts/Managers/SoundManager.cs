using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager InstanceSound;
    private static AudioClip musicMainMenu, inGameMusic, bomSound, starSound, posionSound, bigStarSound, spikesSound, foot1Sound, foot2Sound, waterSound;
    [SerializeField] static AudioSource audioSrc;
    private float soundFxVolume = 1f;

    private void Awake()
    {
        if (InstanceSound != null)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceSound = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();

        musicMainMenu = Resources.Load<AudioClip>("Music/musicMainMenu");
        inGameMusic = Resources.Load<AudioClip>("Music/game1Music");
        bomSound = Resources.Load<AudioClip>("Bom");
        starSound = Resources.Load<AudioClip>("Star");
        posionSound = Resources.Load<AudioClip>("Posion");
        bigStarSound = Resources.Load<AudioClip>("BigStar");
        spikesSound = Resources.Load<AudioClip>("Spikes");
        foot1Sound = Resources.Load<AudioClip>("Foot1");
        foot2Sound = Resources.Load<AudioClip>("Foot2");
        waterSound = Resources.Load<AudioClip>("Water");

    }
    public static void PlaySound (string clip)
    {
        audioSrc.Stop();
        switch (clip)
        {

            case "Bom":
                audioSrc.PlayOneShot(bomSound);
                break;
        
            case "Star":
                audioSrc.PlayOneShot(starSound);
                break;
        
            case "BigStar":
                audioSrc.PlayOneShot(bigStarSound);
                break;
               
            case "Posion":
                audioSrc.PlayOneShot(posionSound);
                break;
       
            case "Spikes":
                audioSrc.PlayOneShot(spikesSound);
                break;
        
            case ("Foot1"):
                audioSrc.PlayOneShot(foot1Sound);
                break;
        
            case ("Foot2"):
                audioSrc.PlayOneShot(foot2Sound);
                break;
       
            case ("Water"):
                audioSrc.PlayOneShot(waterSound);
                break;
        }
    }

    public void UpdateReturnFxVolume(float volume)
    {
        soundFxVolume = volume;
    }

}
