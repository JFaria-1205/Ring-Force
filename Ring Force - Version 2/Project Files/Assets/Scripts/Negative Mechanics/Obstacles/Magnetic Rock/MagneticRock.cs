using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticRock : MonoBehaviour
{
    GameObject secondRock;
    private bool magnetize = false;
    private bool horizontal;

    [SerializeField] float moveRate = 10f; //moveAmount multiplier
    private float moveAmount; //the amount at which the rocks move toward each other

    private float rock1StartLoc; //rock 1's starting location
    private float rock1EndLoc; //rock 1's ending location

    private float rock2StartLoc; //rock 2's starting location
    private float rock2EndLoc; //rock 1's ending location

    public bool playerCheck = true;

    private void Start()
    {
        Physics.IgnoreLayerCollision(8, 8, true);
    }

    void Update()
    {
        if (magnetize)
            MagnetizeRocks();
    }

    public void SpawnSecondRock(bool _horizontal, Vector3 secondRockLoc, out GameObject _secondRock)
    {
        _secondRock = Instantiate(this.gameObject, secondRockLoc, Quaternion.identity);
        secondRock = _secondRock;

        horizontal = _horizontal;
        if (horizontal)
        {
            rock1StartLoc = this.transform.position.x;
            rock1EndLoc = this.transform.position.x + 3.5f;

            rock2StartLoc = secondRock.transform.position.x;
            rock2EndLoc = secondRock.transform.position.x - 3.5f;

            moveAmount = 3.5f / 100f * moveRate;
        }
        else
        {
            rock1StartLoc = this.transform.position.y;
            rock1EndLoc = this.transform.position.y + 1.75f;

            rock2StartLoc = secondRock.transform.position.y;
            rock2EndLoc = secondRock.transform.position.y - 1.75f;

            moveAmount = 1.75f / 100f * moveRate;
        }

        secondRock.GetComponent<MagneticRock>().enabled = false;
    }

    private void MagnetizeRocks()
    {
        var rock1TempLoc = this.transform.position;
        var rock2TempLoc = secondRock.transform.position;

        if (horizontal)
        {
            //move both rocks horizontally towards each other 3.5 units on x both ways (rock 1 += 3.5 | rock 2 -= 3.5)
            rock1TempLoc.x += moveAmount * Time.deltaTime;
            rock2TempLoc.x -= moveAmount * Time.deltaTime;

            Mathf.Clamp(rock1TempLoc.x, rock1StartLoc, rock1EndLoc);
            Mathf.Clamp(rock2TempLoc.x, rock2EndLoc, rock2StartLoc);

            this.transform.position = rock1TempLoc;
            secondRock.transform.position = rock2TempLoc;

            if (rock1TempLoc.x >= rock1EndLoc && rock2TempLoc.x <= rock2EndLoc)
                this.enabled = false;

        }
        else
        {
            //move both rocks vertically towards each other 1.75 units on y both ways (rock 1 += 1.75 | rock 2 -= 1.75)
            rock1TempLoc.y += moveAmount * Time.deltaTime;
            rock2TempLoc.y -= moveAmount * Time.deltaTime;

            Mathf.Clamp(rock1TempLoc.y, rock1StartLoc, rock1EndLoc);
            Mathf.Clamp(rock2TempLoc.y, rock2EndLoc, rock2StartLoc);

            this.transform.position = rock1TempLoc;
            secondRock.transform.position = rock2TempLoc;

            if (rock1TempLoc.y >= rock1EndLoc && rock2TempLoc.y <= rock2EndLoc)
                this.enabled = false;
        }

        moveAmount *= 1.1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerCheck)
        {
            if (other.gameObject.tag == "Player")
            {
                magnetize = true;
                playerCheck = false;
            }
        }
    }
}
