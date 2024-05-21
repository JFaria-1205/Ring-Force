using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObtainPickup : MonoBehaviour
{
    [SerializeField] P_Jump jump;
    [SerializeField] P_Shield shield;
    [SerializeField] P_Bomb bomb;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "P_Jump")
        {
            Destroy(collider.gameObject);
            ObtainedPickup("Jump");
        }
        else if (collider.gameObject.tag == "P_Shield")
        {
            Destroy(collider.gameObject);
            ObtainedPickup("Shield");
        }
        else if (collider.gameObject.tag == "P_Bomb")
        {
            Destroy(collider.gameObject);
            Debug.Log("BOMB");
            ObtainedPickup("Bomb");
        }
    }

    void ObtainedPickup(string name)
    {
        AudioManager.Instance.PlaySFX("Pickup");

        switch (name)
        {
            case "Jump":
                jump.JumpPickup();
                break;
            case "Shield":
                shield.ShieldPickup();
                break;
            case "Bomb":
                bomb.BombPickup();
                break;
        }
    }

    

    
}
