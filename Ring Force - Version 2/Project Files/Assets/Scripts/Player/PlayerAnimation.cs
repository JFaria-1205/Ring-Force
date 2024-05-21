using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] float animationRotationSpeed;
    private Vector3 rotation;
    public bool isMovingRight;
    public bool isMovingLeft;

    void Start()
    {
        rotation = new Vector3(0, 0, 0.1f);
    }

    private void Update()
    {
        if ((isMovingRight && isMovingLeft) /*if moving right AND left*/ || (!isMovingRight && !isMovingLeft)/*if not moving right or left*/)
        {
            if (!(transform.rotation.eulerAngles.z < 2) && !(transform.rotation.eulerAngles.z > 358))
            {
                //rotate to the middle
                if (transform.rotation.z >= 0)
                {
                    //rotate right to get to middle
                    transform.Rotate(-rotation * animationRotationSpeed * Time.deltaTime, Space.Self);
                }
                else
                {
                    //rotate left to get to middle
                    transform.Rotate(rotation * animationRotationSpeed * Time.deltaTime, Space.Self);
                }
            }
            else
            {
                //dont rotate at all
            }
        }
        else if ((isMovingRight) && ((transform.rotation.eulerAngles.z < 135) || (transform.rotation.eulerAngles.z > 330))) //if moving right AND between the two numbers
        {
            //rotate right
            transform.Rotate(-rotation * animationRotationSpeed * Time.deltaTime, Space.Self);
        }
        else if ((isMovingLeft) && ((transform.rotation.eulerAngles.z < 30) || (transform.rotation.eulerAngles.z > 225))) //if moving left AND between the two numbers
        {
            //rotate left
            transform.Rotate(rotation * animationRotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}
