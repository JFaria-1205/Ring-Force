using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObstacle : MonoBehaviour
{
    public void ObstacleWasHit(float playerspeed)
    {
        this.GetComponent<BoxCollider>().enabled = false;

        float sideForceToAdd;
        if (Random.value > 0.5f)
            sideForceToAdd = 700f;
        else
            sideForceToAdd = -700f;

        this.GetComponent<Rigidbody>().AddForce(sideForceToAdd, 1000, playerspeed * 1.1f);

        Invoke("DestroyThis", 5f);
    }

    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
