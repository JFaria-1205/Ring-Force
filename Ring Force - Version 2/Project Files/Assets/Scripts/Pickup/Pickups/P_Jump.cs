using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P_Jump : MonoBehaviour
{
    float jumpPickupDuration = 6f;
    PlayerMovement playerMovement;
    GameManager gameManager;
    [SerializeField] RawImage jumpImage;
    Color originalColor;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        originalColor = jumpImage.color;
    }

    public void JumpPickup()
    {
        CancelInvoke();
        playerMovement.hasJumpPickup = true;
        gameManager.ToggleJumpButtons(true);
        Invoke("RemoveJumpPickup", jumpPickupDuration);
        jumpImage.gameObject.SetActive(true);
        StartFading();
    }

    void RemoveJumpPickup()
    {
        playerMovement.hasJumpPickup = false;
        jumpImage.gameObject.SetActive(false);
        gameManager.ToggleJumpButtons(false);
    }

    IEnumerator FadeOut()
    {
        for (float f = jumpPickupDuration - 1f; f >= -0.05f; f -= 0.05f)
        {
            Color c = jumpImage.color;
            c.a = f;
            jumpImage.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void StartFading()
    {
        StopAllCoroutines();
        jumpImage.color = originalColor;
        StartCoroutine("FadeOut");
    }
}
