using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject allPauseMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject pauseButton;

    [SerializeField] GameObject shieldIcon;
    [SerializeField] GameObject jumpIcon;

    [SerializeField] GameObject mobileControls;
    [SerializeField] GameObject jumpControl1;
    [SerializeField] GameObject jumpControl2;
    public bool handheldDevice = false;

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerData playerData;

    [SerializeField] GameObject platform;
    [SerializeField] GameObject platformStart;
    [SerializeField] Score score;
    [SerializeField] Text finalScoreText;
    [SerializeField] Text highScoreAchieved;
    public bool gameHasEnded = false;

    [SerializeField] GameObject gameOverObject;
    [SerializeField] Image gameOverBackground;
    [SerializeField] GameObject gameOverButtons;

    private GameObject oldPlatform;
    private GameObject newPlatform;
    private float platformSpawnLocZ = 400f;
    public int obstaclesToSpawn = 1;
    float obstacleSpawnChecker = 200;
    float prevHighScore;
    bool gameIsPaused = false;
    bool canPauseGame = true;

    public bool canSpawnPickup = true;
    float spawnPickupDelay = 10f;

    //player death
    float deathRotateX;
    float deathRotateY;
    float deathRotateZ;

    private void Awake()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            handheldDevice = true;
            mobileControls.SetActive(true);
            playerMovement.handheldDevice = true;
        }
        else
        {
            handheldDevice = false;
            mobileControls.SetActive(false);
            playerMovement.handheldDevice = false;
        }
    }

    private void Start()
    {
        canSpawnPickup = false;
        Invoke("CanSpawnPickupReset", spawnPickupDelay);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (canPauseGame)
            {
                if (gameIsPaused)
                {
                    gameIsPaused = false;
                    ResumeGame();
                }
                else
                {
                    gameIsPaused = true;
                    PauseGame();
                }
            }
        }

        if (gameHasEnded)
        {
            GameObject.FindGameObjectWithTag("Player").transform.Rotate(deathRotateX * Time.deltaTime, deathRotateY * Time.deltaTime, deathRotateZ * Time.deltaTime, Space.Self);
        }
    }

    public void MultiplierChange(float change)
    {
        score.scoreMultiplier = change;
    }

    public void SpawnedPickup()
    {
        canSpawnPickup = false;
        Invoke("CanSpawnPickupReset", spawnPickupDelay);
    }

    void CanSpawnPickupReset()
    {
        canSpawnPickup = true;
    } 

    public void SpawnNewPlatform()
    {
        if (oldPlatform != null && !gameHasEnded)
        {
            oldPlatform.GetComponent<SpawnObstacles>().RemoveSpawnedObstacles();
            Destroy(oldPlatform);
        }
        else if (platformStart != null)
        {
            Destroy(platformStart);
        }
        else
        {
            var platforms = GameObject.FindGameObjectsWithTag("Platform");
            foreach (var item in platforms)
            {
                if (item != oldPlatform || item != newPlatform)
                {
                    if (!gameHasEnded)
                    {
                        item.GetComponent<SpawnObstacles>().RemoveSpawnedObstacles();
                        Destroy(item);
                    }
                    break;
                }
            }
        }

        if (newPlatform != null)
        {
            platformSpawnLocZ += 200f;
            oldPlatform = newPlatform;
        }

        newPlatform = Instantiate(platform, new Vector3(0.0000f, 0.0000f, platformSpawnLocZ), Quaternion.identity);

        //Obstacle count gradual increase
        if ((GameObject.FindFirstObjectByType<Score>().highScore >= obstacleSpawnChecker) && (obstaclesToSpawn < 20))
        {
            if (obstacleSpawnChecker >= 5000)
            {
                obstacleSpawnChecker *= 1.07f;
            }
            else if (obstacleSpawnChecker >= 1000)
            {
                obstacleSpawnChecker = (obstacleSpawnChecker * 1.15f) + 250f;
            }
            else
            {
                obstacleSpawnChecker *= 1.5f;
            }
            Mathf.Round(obstacleSpawnChecker);
            obstaclesToSpawn++;
        }
    }

    public void Endgame()
    {
        if (!gameHasEnded)
        {
            pauseButton.SetActive(false);
            mobileControls.SetActive(false);
            shieldIcon.SetActive(false);
            jumpIcon.SetActive(false);
            AudioManager.Instance.PlaySFX("Slo Mo");
            canPauseGame = false;
            deathRotateX = Random.Range(-70, 70);
            deathRotateY = Random.Range(-70, 70);
            deathRotateZ = Random.Range(-70, 70);
            gameHasEnded = true;
            score.trackScore = false;
            LoadPlayer();
            if (score.highScore > prevHighScore)
            {
                SavePlayer();
                highScoreAchieved.enabled = true;
            }
            Time.timeScale = 0.4f;
            finalScoreText.text = $"Score: {score.highScore}";
            Invoke("ShowGameOverScreen", 1f);
        }
    }

    void ShowGameOverScreen()
    {
        gameOverObject.SetActive(true);
    }

    public void RestartGameBtn()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainLevel");
        canPauseGame = true;
    }

    public void MainMenuBtn()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        CancelInvoke();
        AudioManager.Instance.PlayMusic("Cube Runner", true);
        canPauseGame = true;
    }

    public void SavePlayer()
    {
        playerData.playerHighScore = score.highScore;
        playerData.SavePlayer();
    }

    public void LoadPlayer()
    {
        playerData.LoadPlayer();
        prevHighScore = playerData.playerHighScore;
    }

    public void ToggleJumpButtons(bool active)
    {
        jumpControl1.SetActive(active);
        jumpControl2.SetActive(active);
    }

    public void StartMoveRight()
    {
        playerMovement.moveRight = true;
    }

    public void StopMoveRight()
    {
        playerMovement.moveRight = false;
    }

    public void StartMoveLeft()
    {
        playerMovement.moveLeft = true;
    }

    public void StopMoveLeft()
    {
        playerMovement.moveLeft = false;
    }

    public void Jump()
    {
        playerMovement.Jump();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        allPauseMenu.SetActive(true);

        if (handheldDevice)
        {
            mobileControls.SetActive(false);
            if (playerMovement.hasJumpPickup)
                ToggleJumpButtons(false);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        allPauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        gameIsPaused = false;

        if (handheldDevice)
        {
            mobileControls.SetActive(true);
            if (playerMovement.hasJumpPickup)
                ToggleJumpButtons(true);
        }
    }

    public void MainMenuButton()
    {
        gameHasEnded = true;
        score.trackScore = false;
        LoadPlayer();
        if (score.highScore > prevHighScore)
        {
            SavePlayer();
            highScoreAchieved.enabled = true;
        }
        SceneManager.LoadScene("MainMenu");
        CancelInvoke();
        AudioManager.Instance.PlayMusic("Cube Runner", true);
        canPauseGame = true;
        Time.timeScale = 1f;
    }

    public void SettingsButton()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void BackButton()
    {
        pauseMenu.SetActive(true);
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
}
