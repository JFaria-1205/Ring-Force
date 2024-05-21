using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float playerHighScore;
    public float musicVolume;
    public float sfxVolume;

    public SaveData(PlayerData data)
    {
        playerHighScore = data.playerHighScore;
        musicVolume = data.musicVolume;
        sfxVolume = data.sfxVolume;
    }
}
