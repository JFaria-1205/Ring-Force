using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFlair : MonoBehaviour
{
    private bool doRotation;
    private float rotateSpeedX;
    private float rotateSpeedY;
    [SerializeField] bool randomlyRotate = true;


    void Start()
    {
        this.transform.Rotate(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), Space.Self);

        if (randomlyRotate)
        {
            if (Random.value >= 0.8f)
            {
                doRotation = true;
                rotateSpeedX = Random.Range(7, 30) * Time.deltaTime;
                rotateSpeedY = Random.Range(7, 30) * Time.deltaTime;
            }
            else
                doRotation = false;
        }
        else
            doRotation = false;
        
    }


    void Update()
    {
        if (doRotation)
        {
            this.transform.Rotate(rotateSpeedX, rotateSpeedY, 0, Space.World);
        }
    }
}
