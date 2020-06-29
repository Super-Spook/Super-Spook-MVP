using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMove : MonoBehaviour
{
    public float dashForceX;
    public float dashForceYup;
    public float dashForceYdown;
    public float dashTime;
    public float startDashTime;
    public int dashDirection;
    public int dashCount;

    public bool isClinging;
    public Vector2 initialClingPos;
    GameObject CurrentMovingPlatform;
    public float fallSpeed;

    //Alternative Dash options Variables
    [Header("Alternative")]
    public float dashSpeedY;
    public float dashSpeedX;
    public float dashDistance;
    public float smoothDashTime;
    public float maxDashSpeed;

    private Rigidbody2D avatarRb;
    private Animator avatarAnimator;
    public SoundManager soundManager;

    void Start()
    {
        avatarRb = GetComponent<Rigidbody2D>();
        avatarAnimator = GetComponent<Animator>();
        dashTime = startDashTime;
        dashDirection = 0;
        dashCount = 1;
    }

    private void Update()
    {
        if (isClinging)
        {
            transform.position = new Vector2(transform.position.x, CurrentMovingPlatform.transform.position.y - fallSpeed);
            if(fallSpeed < 0.15f)
            {
                fallSpeed += Time.deltaTime * 0.03f;
            }
            else if(fallSpeed > 0.15f)
            {
                fallSpeed += Time.deltaTime * 0.6f;
            }

            if (transform.position.y < CurrentMovingPlatform.transform.position.y - 0.5f)
            {
                avatarAnimator.SetBool("isClinging", false);
                GetComponent<AvatarMovement>().Flip();
                GetComponent<AvatarMovement>().Jump();
                isClinging = false;
            }
        }
    }

    void FixedUpdate()
    {
        Dash();
    }
    
    //Handles the Dash direction to dictate the angle of the move
    private void Dash() {
        if (dashDirection == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetKeyDown(KeyCode.A)) && dashCount == 1)
            {
                if (dashCount == 1)
                {
                    dashDirection = 1;
                    dashCount = 0;
                    soundManager.PlaySound("Dash");
                    if (isClinging)
                    {
                        isClinging = false;
                        avatarAnimator.SetBool("isClinging", false);
                        GetComponent<AvatarMovement>().Flip();
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetKeyDown(KeyCode.D)) && dashCount == 1)
            {
                if (dashCount == 1)
                {
                    dashDirection = 2;
                    dashCount = 0;
                    soundManager.PlaySound("Dash");
                    if (isClinging)
                    {
                        isClinging = false;
                        avatarAnimator.SetBool("isClinging", false);
                        GetComponent<AvatarMovement>().Flip();
                    }
                }

            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetKeyDown(KeyCode.W)) && dashCount == 1 && !GetComponent<AvatarMovement>().IsGrounded())
            {
                if (dashCount == 1)
                {
                    dashDirection = 3;
                    dashCount = 0;
                    soundManager.PlaySound("Dash");
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetKeyDown(KeyCode.S)) && dashCount == 1 && !GetComponent<AvatarMovement>().IsGrounded())
            {
                if (dashCount == 1)
                {
                    dashDirection = 4;
                    dashCount = 0;
                    soundManager.PlaySound("Dash");
                }
            }           
        }
        else
        {
            if (dashTime <= 0)
            {
                dashDirection = 0;
                dashTime = startDashTime;
                avatarRb.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.deltaTime;
                GetComponent<AvatarMovement>().DynamicCamera.GetComponent<SmartCamera>().ShakingCamera = true;
                if (dashDirection == 1)
                {                   
                    avatarRb.AddForce(Vector2.left * dashForceX);
                    avatarRb.velocity = new Vector2(avatarRb.velocity.x, 0);                   
                }
                else if (dashDirection == 2)
                {
                    avatarRb.AddForce(Vector2.right * dashForceX);
                    avatarRb.velocity = new Vector2(avatarRb.velocity.x, 0); 
                   
                    //Below are alternative options for dashing feel
                    //avatarRb.velocity = Vector2.right * dashSpeedX;                   
                    //float newPosition = Mathf.SmoothDamp(transform.position.x, transform.position.x + dashDistance, ref xVelocity, smoothDashTime, maxDashSpeed);
                    //transform.position = new Vector2(newPosition, transform.position.y);
                    
                }
                else if (dashDirection == 3 && GetComponent<Animator>().GetBool("inAir"))
                {
                    avatarRb.velocity = Vector2.up * dashForceYup;
                }
                else if (dashDirection == 4 )
                {
                    avatarRb.velocity = Vector2.down * dashForceYdown;
                }
            }

        }
    }

    //Sets DashCount back to 1 when the player touches the Ground
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("MovementX") || collision.gameObject.CompareTag("MovementY") || collision.gameObject.CompareTag("MovementXY") || collision.gameObject.CompareTag("Mushroom"))
        {
            dashCount = 1;
        }
    }

    
    //Cling to moving platforms
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(dashDirection == 1 || dashDirection == 2 && collision.gameObject.CompareTag("MovementY") || collision.gameObject.CompareTag("MovementX") || collision.gameObject.CompareTag("MovementXY"))
        {
            if (collision.gameObject.GetComponent<MovingPlatform>().Clingable)
            {
                avatarAnimator.SetBool("isClinging", true);
                isClinging = true;
                CurrentMovingPlatform = collision.gameObject;
                initialClingPos = transform.position;
                if (GetComponent<AvatarMovement>().isFacingRight)
                {
                    GetComponent<AvatarMovement>().DynamicCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.75f;
                }
                else if (!GetComponent<AvatarMovement>().isFacingRight)
                {
                    GetComponent<AvatarMovement>().DynamicCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.10f;
                }
            }
        }
    }
}

