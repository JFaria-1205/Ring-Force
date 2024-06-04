using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public float playerHighScore;
    public float musicVolume;
    public float sfxVolume;

    public static PlayerData instane;

    private void Awake()
    {
        instane = this;
        DontDestroyOnLoad(gameObject);
        LoadPlayer();
    }

    public void SavePlayer()
    {
        musicVolume = Mathf.Round(musicVolume * 1000f) / 1000f;
        sfxVolume = Mathf.Round(sfxVolume * 1000f) / 1000f;
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        SaveData saveData = SaveSystem.LoadPlayer();

        if (saveData != null)
        {
            playerHighScore = saveData.playerHighScore;
            musicVolume = saveData.musicVolume;
            sfxVolume = saveData.sfxVolume;
        }
        else
        {
            playerHighScore = 0;
            musicVolume = 1;
            sfxVolume = 1;
        }
            
    }
}
