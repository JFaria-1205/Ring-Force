using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet_BG : MonoBehaviour
{
    private Transform player;
    private Vector3 tempPosition;
    [SerializeField] Vector3 offset;
    [SerializeField] float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tempPosition.z = player.position.z;
        transform.position = tempPosition + offset;

        transform.Rotate(0, rotateSpeed * Time.deltaTime * 0.01f, 0, Space.World);
    }
}
