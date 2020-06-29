using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class AvatarMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    public float moveSpeed;
    [Range(1, 10)] public float jumpVelocity;
    public float fallMultiplier;
    public bool isAccelerating;
    private float yVelocity;
    public bool isFacingRight;
    public float MushroomTimer;

    [Header("Component Variables")]
    public LayerMask GroundLayerMask;
    public Rigidbody2D avatarRb;
    public Animator avatarAnimator;
    public CapsuleCollider2D capsuleCollider2d;
    public Transform avatarTransform;
    public CinemachineVirtualCamera DynamicCamera;
    public SoundManager soundManager;

    void Start()
    {
        isFacingRight = true;
        isAccelerating = true;
    }

    void Update()
    {
        JumpAnimationSet();
        IsGrounded();
        if(MushroomTimer >= 0)
        {
            ActivateMushroomTimer();
        }
    }

    void FixedUpdate()
    {
        if (!isAccelerating)
        {
            Run();
        }
        else
        {
            Accelerate();
        }
        SlowFall();
    }
    
   //Increases the Avatar's movement speed up to the Run() velocity
    private void Accelerate()
    {
        if (isFacingRight) 
        {
            avatarRb.AddForce(new Vector2(moveSpeed + 5f, 0));
            if (avatarRb.velocity.x >= 5.7f)
            {
                isAccelerating = false;
            }
        }
        else if (!isFacingRight)
        {
            avatarRb.AddForce(new Vector2(-moveSpeed - 5f, 0));
            if (avatarRb.velocity.x <= -5.7f)
            {
                isAccelerating = false;
            }
        }
    }
   
    //Controls the base mechanics of the movement
    private void Run()
    {
        isAccelerating = false;
        if (GetComponent<DashMove>().dashDirection == 1 || GetComponent<DashMove>().dashDirection == 2 || GetComponent<DashMove>().isClinging || MushroomTimer >= 0)
        {
            return;
        }
        else if (GetComponent<DashMove>().dashDirection == 0)
        {
            if (isFacingRight) { avatarRb.velocity = new Vector2(moveSpeed, avatarRb.velocity.y); }
            else if (!isFacingRight) { avatarRb.velocity = new Vector2(-moveSpeed, avatarRb.velocity.y); }
        }
    }

    //Slows the Avatar's fall speed when Y velocity is negative
    private void SlowFall()
    {
        if (avatarRb.velocity.y < 0 && avatarRb.velocity.y >= -20)
        {
            avatarRb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void JumpAnimationSet()
    {
        if (avatarRb.velocity.y == 0)
        {
            avatarAnimator.SetBool("inAir", false);
        }
    }

    
    //Flips the character's LocalScale & makes him Run the opposite direction
    public void Flip()
    {
        if (isFacingRight)
        {           
            avatarTransform.localScale = new Vector3(-1, 1, 1);
            isFacingRight = false;
            if (DynamicCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX == 0.75f)
            {
                DynamicCamera.GetComponent<SmartCamera>().FlippingCamera = true;
            }
        }
        else if (!isFacingRight)
        {
            avatarTransform.localScale = new Vector3(1, 1, 1);
            isFacingRight = true;
            if (DynamicCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX == 0.10f)
            {
                DynamicCamera.GetComponent<SmartCamera>().FlippingCamera = true;
            }
        }
    }

    //Checks if the Avatar is currently on the Ground LayerMask
    public bool IsGrounded()
    {
        float extraHeight = 0.3f;
        Vector3 extraRayLength = new Vector3(0.6f, 0,0); 
        if (isFacingRight) 
        { 
            RaycastHit2D raycasthit = Physics2D.Raycast(capsuleCollider2d.bounds.center + extraRayLength, Vector2.down, capsuleCollider2d.bounds.extents.y + extraHeight, GroundLayerMask);
            return raycasthit.collider != null;
        }
        else if (!isFacingRight) 
        { 
            RaycastHit2D raycasthit = Physics2D.Raycast(capsuleCollider2d.bounds.center - extraRayLength, Vector2.down, capsuleCollider2d.bounds.extents.y + extraHeight, GroundLayerMask);
            return raycasthit.collider != null;
        }
        return false;
    }
    
    //Dictates when the Avatar should Jump
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!avatarAnimator.GetBool("inAir"))
        {
            if (collision.gameObject.CompareTag("Ground") && IsGrounded() == false)
            {
                Jump();
            }
            else if (collision.gameObject.CompareTag("MovementY") && IsGrounded() == false)
            {
                Jump();
            }
            else if (collision.gameObject.CompareTag("MovementX") && IsGrounded() == false)
            {
                Jump();
            }
            else if (collision.gameObject.CompareTag("MovementXY") && IsGrounded() == false)
            {
                Jump();
            }
        }       
    }

    public void Jump()
    {
        avatarRb.velocity = Vector2.up * jumpVelocity;
        avatarAnimator.SetBool("inAir", true);
        soundManager.PlaySound("Jump");
    }

    //Dictates what should happen when the Avatar collides with different GameObjects
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spikes"))
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }       
    }
    public void ActivateMushroomTimer()
    {
        MushroomTimer -= Time.deltaTime;
    }
}
