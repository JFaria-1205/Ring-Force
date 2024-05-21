using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] Transform aoeTransform;
    public float aoeChangeRate = 1f;
    private float aoeStart = 0.25f;
    private float aoeFinalX = 7f;
    private float aoeFinalY = 4f;
    private float aoeFinalZ = 4f;
    private bool changeAoe = false;

    private void Start()
    {
        aoeTransform.localScale = new Vector3(aoeStart, aoeStart, aoeStart);
        Invoke("AOEChangeStart", Random.Range(0.5f, 1f));
    }

    private void Update()
    {
        if (changeAoe)
            AOEChange();
    }

    void AOEChange()
    {
        float xRate = aoeFinalX / aoeChangeRate;
        float yRate = aoeFinalY / aoeChangeRate;
        float zRate = aoeFinalZ / aoeChangeRate;

        var tempScale = aoeTransform.localScale;

        tempScale.x += xRate * Time.deltaTime;
        tempScale.y += yRate * Time.deltaTime;
        tempScale.z += zRate * Time.deltaTime;

        Mathf.Clamp(tempScale.x, aoeStart, aoeFinalX);
        Mathf.Clamp(tempScale.y, aoeStart, aoeFinalY);
        Mathf.Clamp(tempScale.z, aoeStart, aoeFinalZ);

        aoeTransform.localScale = tempScale;

        if (tempScale.x >= aoeFinalX && tempScale.y >= aoeFinalY && tempScale.z >= aoeFinalZ)
        {
            changeAoe = false;
            Invoke("AOEChangeStart", Random.Range(0.5f, 1f));
        }
    }

    void AOEChangeStart()
    {
        aoeTransform.localScale = new Vector3(aoeStart, aoeStart, aoeStart);
        changeAoe = true;
    }

    public void ExplodeMine()
    {
        Debug.Log("EXPLODE");
        Destroy(this.gameObject);
    }
}
