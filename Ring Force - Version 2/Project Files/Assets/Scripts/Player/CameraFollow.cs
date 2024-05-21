using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    [SerializeField] Vector2 xyOffset; //0, 1
    [SerializeField] float zOffset; //-5
    public float smoothSpeed = 0.125f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        Vector2 desiredPosition = new Vector2(player.position.x, player.position.y) + xyOffset; //position you want to snap to
        Vector2 smoothedPosition = Vector2.Lerp(new Vector2(transform.position.x, transform.position.y), desiredPosition, smoothSpeed); //get closer to that position
        Vector3 setPosition = transform.position;
        setPosition.x = Mathf.Clamp(smoothedPosition.x, -5, 5);
        setPosition.y = Mathf.Clamp(smoothedPosition.y, -10, 7);
        setPosition.z = player.position.z + zOffset;
        transform.position = setPosition;
    }
}
