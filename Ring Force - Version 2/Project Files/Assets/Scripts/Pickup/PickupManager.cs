using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickupManager : MonoBehaviour
{
    public GameObject pickupPrefab;
    public Pickup[] pickupList;
    private GameObject pickupSpawned;

    public void SpawnPickup(Vector3 spawnPos)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager.canSpawnPickup)
        {
            int _index = UnityEngine.Random.Range(0, pickupList.Length);
            string _name = pickupList[_index].pickupName;

            Pickup p = Array.Find(pickupList, x => x.pickupName == _name);

            if (p == null)
            {
                Debug.Log("Pickup Not Found");
            }
            else
            {
                GameObject newPickup = Instantiate(pickupPrefab, spawnPos, Quaternion.identity);
                newPickup.GetComponent<MeshRenderer>().material = p.material;
                newPickup.tag = p.pickupTag;
                pickupSpawned = newPickup;
            }
            gameManager.SpawnedPickup();
            Invoke("DestroyThis", 10f);
        }
        else
        {
            //Debug.Log("Pickup tried to spawn but can't spawn yet");
        }
    }

    private void DestroyThis()
    {
        Destroy(pickupSpawned);
    }
}
