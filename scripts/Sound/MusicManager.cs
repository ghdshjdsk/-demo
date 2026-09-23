using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance {get; private set;}

    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    private AudioSource audioSource;
    private float volume = 0.5f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME);
    }

    public void ChangeVolume(float value)
    {
        volume = value;
        audioSource.volume = volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME,volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
