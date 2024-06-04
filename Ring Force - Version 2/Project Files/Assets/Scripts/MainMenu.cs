using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private PlayerData playerData;
    [SerializeField] Text hsText;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject playerDataSaveObject;

    float highScore = 0;

    private void Awake()
    {
        LoadPlayer();
    }

    private void Start()
    {        
        hsText.text = $"HIGH SCORE: {highScore}";
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("MainLevel");
        AudioManager.Instance.musicSource.Stop();

        int i = UnityEngine.Random.Range(1, 3);
        switch (i)
        {
            case 1:
                AudioManager.Instance.PlayMusic("Run Boi Run", false);
                break;
            case 2:
                AudioManager.Instance.PlayMusic("Watch Out", false);
                break;
        }
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void SettingsButton()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void BackButton()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void ButtonHover()
    {
        AudioManager.Instance.PlaySFX("Btn Hover");
    }

    public void ButtonSelect()
    {
        AudioManager.Instance.PlaySFX("Btn Select");
    }

    public void LoadPlayer()
    {
        if (FindObjectOfType<PlayerData>() == null)
        {
            Instantiate(playerDataSaveObject);
        }
        playerData = FindObjectOfType<PlayerData>();
        playerData.LoadPlayer();
        highScore = playerData.playerHighScore;
    }
}
