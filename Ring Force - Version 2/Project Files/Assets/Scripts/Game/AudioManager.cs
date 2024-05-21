using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    string currentSong;
    bool checkIfSongIsPlaying = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            PlayMusic("Cube Runner", true);
        }
    }

    public void PlayMusic(string name, bool loop)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            currentSong = name;
            musicSource.clip = s.clip;
            musicSource.Play();
            musicSource.loop = loop;

            if (!loop)
                checkIfSongIsPlaying = true;
        }
    }

    private void Update()
    {
        if (checkIfSongIsPlaying)
        {
            if (!musicSource.isPlaying)
            {
                PlayNextSong();
            }
        }
    }

    public void PlayNextSong()
    {
        Sound[] tempMusicList = new Sound[musicSounds.Length-2];
        int musicIndex = 0;
        for (int i = 0; i < musicSounds.Length; i++)
        {
            if (musicSounds[i].name != "Cube Runner" && musicSounds[i].name != currentSong)
            {
                tempMusicList.SetValue(musicSounds[i], musicIndex);
                musicIndex += 1;
            }
        }

        PlayMusic(tempMusicList[UnityEngine.Random.Range(0, tempMusicList.Length)].name, false);
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
