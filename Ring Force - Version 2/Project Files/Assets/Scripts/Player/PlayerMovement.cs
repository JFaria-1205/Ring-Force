using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    PlayerAnimation playerAnimation;
    private Rigidbody rb;

    public float playerSpeed = 50000f;
    [SerializeField] float baseSpeedIncrease;
    [SerializeField] float distanceMultiplier;

    [SerializeField] float strafeSpeed = 300f;
    [SerializeField] float jumpForce = 500f;
    bool canJump = true;
    bool doJump = false;
    float jumpCooldown = 2f;
    public bool hasJumpPickup = false;
    public bool hasShieldPickup = false;

    public bool handheldDevice = false;
    public bool moveRight = false;
    public bool moveLeft = false;
    public bool moveUp = false;
    public bool moveDown = false;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        doJump = false;
    }

    void FixedUpdate()
    {
        #region Movement / Animations
        rb.AddForce(0, 0, playerSpeed * Time.deltaTime);

        if (!handheldDevice) // PC Controls
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                rb.AddForce(strafeSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
                playerAnimation.isMovingRight = true;
            }
            else
                playerAnimation.isMovingRight = false;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddForce(-strafeSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
                playerAnimation.isMovingLeft = true;
            }
            else
                playerAnimation.isMovingLeft = false;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                rb.AddForce(0, strafeSpeed * Time.deltaTime, 0, ForceMode.VelocityChange);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                rb.AddForce(0, -strafeSpeed * Time.deltaTime, 0, ForceMode.VelocityChange);
            }
            if (Input.GetKey(KeyCode.Space))
            {
                if (hasJumpPickup)
                {
                    if (canJump)
                    {
                        AudioManager.Instance.PlaySFX("Jump");
                        rb.AddForce(0, jumpForce * Time.deltaTime, 0, ForceMode.VelocityChange);
                        canJump = false;
                        Invoke("ResetJump", jumpCooldown);
                    }
                }
            }
        }
        else // Mobile Controls
        {
            if (moveRight)
            {
                rb.AddForce(strafeSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
                playerAnimation.isMovingRight = true;
            }
            else
                playerAnimation.isMovingRight = false;
            if (moveLeft)
            {
                rb.AddForce(-strafeSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
                playerAnimation.isMovingLeft = true;
            }
            else
                playerAnimation.isMovingLeft = false;
            if (moveUp)
            {
                rb.AddForce(0, strafeSpeed * Time.deltaTime, 0, ForceMode.VelocityChange);
            }
            if (moveDown)
            {
                rb.AddForce(0, -strafeSpeed * Time.deltaTime, 0, ForceMode.VelocityChange);
            }
            if (doJump)
            {
                doJump = false;

                if (hasJumpPickup)
                {
                    if (canJump)
                    {
                        AudioManager.Instance.PlaySFX("Jump");
                        rb.AddForce(0, jumpForce * Time.deltaTime, 0, ForceMode.VelocityChange);
                        canJump = false;
                        Invoke("ResetJump", jumpCooldown);
                    }
                }
            }
        }

        Mathf.Clamp(this.transform.position.y, 1.0000f, 7.6000f);
        #endregion

        //Gradual player speed increase based on distance traveled
        playerSpeed += (baseSpeedIncrease + (distanceMultiplier * Mathf.Log(transform.position.z + 1f))) * Time.deltaTime;
    }

    public void Jump()
    {
        doJump = true;
    }

    public void ResetJump()
    {
        canJump = true;
    }

    public void HitObject()
    {
        rb.mass = 0.65f;
        rb.drag = 2f;
        rb.AddForce(Random.Range(-1200, 1200), Random.Range(-1200, 1200), Random.Range(-1500, -1000));
        TriggerEndgame();
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        var tag = collisionInfo.gameObject.tag;

        if (hasShieldPickup)
        {
            if (tag == "Obstacle")
            {
                collisionInfo.gameObject.GetComponent<HitObstacle>().ObstacleWasHit(playerSpeed);
                ShieldHit();
            }
            else if (tag == "Mine")
            {
                collisionInfo.gameObject.GetComponentInParent<Mine>().ExplodeMine(); //collider is in a child component
                ShieldHit();
            }
            else if (tag == "LaserEnemy")
            {
                //laser enemy doesnt explode
                ShieldHit();
            }
            else if (tag == "MagneticRock")
            {
                collisionInfo.gameObject.GetComponent<HitObstacle>().ObstacleWasHit(playerSpeed);
                ShieldHit();
            }
        }
        else
        {
            if (tag == "Obstacle")
                HitObject();
            else if (tag == "Mine")
            {
                HitObject();
                collisionInfo.gameObject.GetComponentInParent<Mine>().ExplodeMine();
            }
            else if (tag == "LaserEnemy")
            {
                HitObject();
            }
            else if (tag == "MagneticRock")
            {
                HitObject();
            }
        }
    }

    void ShieldHit()
    {
        hasShieldPickup = false;
        GetComponent<P_Shield>().RemoveShieldPickup();
        Physics.IgnoreLayerCollision(7, 8, true);
        Invoke("EnableCollisionsAfterShieldHit", 1f);
    }

    void EnableCollisionsAfterShieldHit()
    {
        Physics.IgnoreLayerCollision(7, 8, false);
    }

    public void TriggerEndgame()
    {
        FindObjectOfType<GameManager>().Endgame();
        this.enabled = false;
    }
}
