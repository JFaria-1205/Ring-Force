using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public bool dataExists;

    public float playerHighScore;
    public float musicVolume;
    public float sfxVolume;
 

    private void Awake()
    {
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
            dataExists = true;
            playerHighScore = saveData.playerHighScore;
            musicVolume = saveData.musicVolume;
            sfxVolume = saveData.sfxVolume;
        }
        else
            dataExists = false;
    }
}
