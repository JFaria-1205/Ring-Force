using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test123 : MonoBehaviour
{
    float playerZ = 0f;
    float playerSpeed = 50000f;
    float baseSpeedIncrease = 10f;
    float distanceMultiplier = 6f;

    float playerScore = 0;

    private void Start()
    {
        //Problem();
    }

    void Problem()
    {
        while (playerZ < 1000)
        {
            playerZ += 1f;

            playerSpeed += (baseSpeedIncrease + (distanceMultiplier * Mathf.Log(playerZ + 1f))) * Time.deltaTime;

            playerScore += (baseSpeedIncrease + (2f * Mathf.Log(playerZ + 1f, 5))) * Time.deltaTime;
        }

        Debug.Log($"At score value {playerScore}, player speed is {playerSpeed}");
    }

}
