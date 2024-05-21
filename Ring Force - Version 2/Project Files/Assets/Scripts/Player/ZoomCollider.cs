using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCollider : MonoBehaviour
{
    float zoomCooldown;
    bool zoomReady;

    private void Start()
    {
        zoomCooldown = 1f;
        zoomReady = true;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (zoomReady)
        {
            if (collider.gameObject.tag == "Obstacle" && !(FindObjectOfType<GameManager>().gameHasEnded))
            {
                AudioManager.Instance.PlaySFX("Zoom");
                zoomReady = false;
                Invoke("CooldownReset", zoomCooldown);
            }
        }
    }

    void CooldownReset()
    {
        zoomReady = true;
    }
}
