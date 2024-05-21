using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // set the main camera in the inspector
    [SerializeField] Camera MainCamera;

    // set the sky box camera in the inspector
    [SerializeField] Camera SkyCamera;

    // the additional rotation to add to the skybox
    // can be set during game play or in the inspector
    [SerializeField] float rotationSpeed;

    // Use this for initialization
    void Start()
    {
        if (SkyCamera.depth >= MainCamera.depth)
        {
            Debug.Log("Set skybox camera depth lower " +
                " than main camera depth in inspector");
        }
        if (MainCamera.clearFlags != CameraClearFlags.Nothing)
        {
            Debug.Log("Main camera needs to be set to dont clear" +
                "in the inspector");
        }
    }

    // Update is called once per frame
    void Update()
    {
        SkyCamera.transform.position = MainCamera.transform.position;
        SkyCamera.transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
    }
}
