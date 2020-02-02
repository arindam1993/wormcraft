using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public AudioSource MainMusic;

    public AudioMixerGroup MasterGroup;
    public AudioMixer MasterMixer;
    public AudioMixerGroup MusicGroup;

    public AudioMixerGroup SFXGroup;
    public AudioSource MagnetFx;
    public AudioSource ThrowFx;

    public static AudioManager Instance = null;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(gameObject);
        }
        PlayMainMusic();
    }

    public void PlayMainMusic()
    {
        MainMusic.Play();
    }

    public void PlayMagnetFX()
    {
        MagnetFx.Play();
    }

    public void PlayThrowFX()
    {

    }
}
