using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    private Text scoreText;
    private Transform player; //this is to track the distance traveled
    private Rigidbody playerRB;

    public bool trackScore = false;

    private float curScore;
    public float highScore;

    public float scoreMultiplier = 1f;
    [SerializeField] float baseScoreIncrease;
    [SerializeField] float distanceMultiplier;


    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            curScore = 0; //setting the current score to 0 when starting the game
            player = GameObject.FindGameObjectWithTag("Player").transform;
            playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
            scoreText = GetComponentInParent<Text>();
            trackScore = true;
        }
        else
            trackScore = false;
    }

    void Update()
    {
        if (trackScore)
            UpdateScore();
    }

    void UpdateScore()
    {
        curScore += (baseScoreIncrease + (distanceMultiplier * Mathf.Log(player.position.z + 1f, 5))) * Time.deltaTime * scoreMultiplier * (Mathf.Log10(playerRB.velocity.z + 1f) / 100f);
        highScore = Mathf.Floor(curScore);
        scoreText.text = highScore.ToString();
    }
}
