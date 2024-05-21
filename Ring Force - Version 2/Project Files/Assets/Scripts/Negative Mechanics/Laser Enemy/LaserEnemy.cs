using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : MonoBehaviour
{
    [SerializeField] Transform laserBeam;
    [SerializeField] MeshRenderer laserBeamMesh;
    [SerializeField] Material laserBeamFinalColor;
    public float laserChangeRate = 1f;
    private float laserStart = 0.1f;
    private float laserFinalX = 0.6f;
    private float laserFinalZ = 0.6f;
    private bool changeLaser = false;
    private bool playerHasInteracted = false;

    private float laserBlinkMin;
    private bool doLaserBlink = false;
    private bool blinkIncrease = false;

    void Start()
    {
        playerHasInteracted = false;
        blinkIncrease = false;
    }

    void Update()
    {
        if (changeLaser)
            LaserOutlineInitialChange();

        if (doLaserBlink)
            LaserBlink();
    }

    public void SetDirection(float direction)
    {
        Vector3 rotation = new Vector3(0f, 0f, direction);
        this.transform.Rotate(rotation, Space.Self);

        if (direction == 0f || direction == 180f) //right or left
            laserBeam.localScale = new Vector3(laserStart, laserBeam.localScale.y, laserStart);
        else //up or down
        {
            laserBeam.localScale = new Vector3(laserStart, 4f, laserStart);
            laserBeam.localPosition = new Vector3(3.5f, 0f, 0f);
        }
    }

    void LaserOutlineInitialChange()
    {
        float xRate = laserFinalX / laserChangeRate;
        float zRate = laserFinalZ / laserChangeRate;

        var tempScale = laserBeam.localScale;

        tempScale.x += xRate * Time.deltaTime;
        tempScale.z += zRate * Time.deltaTime;

        Mathf.Clamp(tempScale.x, laserStart, laserFinalX);
        Mathf.Clamp(tempScale.z, laserStart, laserFinalZ);

        laserBeam.localScale = tempScale;

        if (tempScale.x >= laserFinalX && tempScale.z >= laserFinalZ)
        {
            changeLaser = false;
            laserBeamMesh.material = laserBeamFinalColor;
            ChangeLaserBlinkMin();
            doLaserBlink = true;
        }
    }

    void LaserBlink()
    {
        float xRate = laserFinalX / laserChangeRate;
        float zRate = laserFinalZ / laserChangeRate;

        var tempScale = laserBeam.localScale;

        if (blinkIncrease)
        {
            tempScale.x += xRate * Time.deltaTime;
            tempScale.z += zRate * Time.deltaTime;

            Mathf.Clamp(tempScale.x, laserBlinkMin, laserFinalX);
            Mathf.Clamp(tempScale.z, laserBlinkMin, laserFinalZ);

            laserBeam.localScale = tempScale;

            if (tempScale.x >= laserFinalX && tempScale.z >= laserFinalZ)
            {
                blinkIncrease = false;
                ChangeLaserBlinkMin();
            }
        }
        else
        {
            tempScale.x -= xRate * Time.deltaTime;
            tempScale.z -= zRate * Time.deltaTime;

            Mathf.Clamp(tempScale.x, laserBlinkMin, laserFinalX);
            Mathf.Clamp(tempScale.z, laserBlinkMin, laserFinalZ);

            laserBeam.localScale = tempScale;

            if (tempScale.x <= laserBlinkMin || tempScale.z <= laserBlinkMin)
            {
                blinkIncrease = true;
                ChangeLaserBlinkMin();
            }
        }
    }

    void ChangeLaserBlinkMin()
    {
        laserBlinkMin = Random.Range(0.3f, 0.55f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !playerHasInteracted)
        {
            changeLaser = true;
            playerHasInteracted = true;
        }
    }
}
