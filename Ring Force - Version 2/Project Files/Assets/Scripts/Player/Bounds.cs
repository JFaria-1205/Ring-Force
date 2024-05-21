using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
    private Transform playerPos;

    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        Physics.IgnoreLayerCollision(10, 8, true);
        Physics.IgnoreLayerCollision(10, 9, true);
    }

    void Update()
    {
        Vector3 setPosition = this.transform.position;
        setPosition.z = playerPos.position.z;
        this.transform.position = setPosition;
    }
}
