using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{

    PlayerData playerData;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    private void Start()
    {
        playerData = FindObjectOfType<PlayerData>();
        LoadPlayer();
    }

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(musicSlider.value);
        playerData.musicVolume = musicSlider.value;
        playerData.SavePlayer();
    }

    public void SfxVolume()
    {
        AudioManager.Instance.SfxVolume(sfxSlider.value);
        playerData.sfxVolume = sfxSlider.value;
        playerData.SavePlayer();
    }

    public void LoadPlayer()
    {
        playerData.LoadPlayer();

        musicSlider.value = playerData.musicVolume;
        sfxSlider.value = playerData.sfxVolume;

        AudioManager.Instance.MusicVolume(playerData.musicVolume);
        AudioManager.Instance.SfxVolume(playerData.sfxVolume);
    }
}
