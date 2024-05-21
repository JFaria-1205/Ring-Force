using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Multiplier : MonoBehaviour
{
    float multiplierPickupDuration = 5f;
    float multiplierIncrease = 10f;
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void MultiplierPickup()
    {
        CancelInvoke();
        gameManager.MultiplierChange(multiplierIncrease);
        Invoke("RemoveMultiplierPickup", multiplierPickupDuration);
    }

    void RemoveMultiplierPickup()
    {
        gameManager.MultiplierChange(1f);
    }
}
