using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class P_Bomb : MonoBehaviour
{
    public void BombPickup()
    {
        List<GameObject> gameObjectList = FindObjectsOfType<GameObject>().ToList();

        foreach (GameObject gameObject in gameObjectList)
        {
            if (gameObject.gameObject.tag == "Obstacle")
                Destroy(gameObject);
        }
    }
}
