using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P_Shield : MonoBehaviour
{
    float shieldPickupDuration = 10f;
    [SerializeField] List<GameObject> shieldList;
    [SerializeField] RawImage shieldImage;
    [SerializeField] PlayerMovement playerMovement;
    Color originalColor;

    private void Start()
    {
        originalColor = shieldImage.color;
    }

    public void ShieldPickup()
    {
        CancelInvoke();
        foreach (GameObject gameObject in shieldList)
        {
            gameObject.SetActive(true);
        }
        Invoke("RemoveShieldPickup", shieldPickupDuration);
        GetComponent<PlayerMovement>().hasShieldPickup = true;
        shieldImage.gameObject.SetActive(true);
        StartFading();
    }

    public void RemoveShieldPickup(/*string reasonForRemoval = "None"*/)
    {
        /*
        if (reasonForRemoval != "None")
            Debug.Log($"Hit Object: {reasonForRemoval}");
        else
            Debug.Log("Shield ran out");
        */

        AudioManager.Instance.PlaySFX("Shield");
        CancelInvoke();
        foreach (GameObject gameObject in shieldList)
        {
            gameObject.SetActive(false);
        }
        shieldImage.gameObject.SetActive(false);
        playerMovement.hasShieldPickup = false;
    }

    IEnumerator FadeOut()
    {
        for (float f = shieldPickupDuration - 1f; f >= -0.05f; f -= 0.05f)
        {
            Color c = shieldImage.color;
            c.a = f;
            shieldImage.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void StartFading()
    {
        StopAllCoroutines();
        shieldImage.color = originalColor;
        StartCoroutine("FadeOut");
    }
}
