using UnityEngine;

public class Mushroom : MonoBehaviour
{
    Animator animator;
    [Range(1, 25)] public float mushroomJumpVelocity;
    [Range(1, 25)] public float mushroomPushVelocity;
    public float LengthOfPush;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        mushroomJumpVelocity = 16f;
        mushroomPushVelocity = 24f;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

    }

    //Detects collision with the Avatar then applies force according to the Mushroom's Angle
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Avatar"))
        {
            animator.SetBool("Collision", true);
            GameObject Avatar = collision.gameObject;
            //Needs to fix the Avatar X Velocity and Y Fall Multiplier Issues before this can fully work
            switch (transform.eulerAngles.z)
            {
                //Up
                case 0:
                    Avatar.GetComponent<Animator>().SetBool("inAir", true);
                    Avatar.GetComponent<Rigidbody2D>().velocity = new Vector2(Avatar.GetComponent<Rigidbody2D>().velocity.x, mushroomJumpVelocity);
                    audioSource.Play();
                    break;
                //Down
                case 180:
                    Avatar.GetComponent<Animator>().SetBool("inAir", true);
                    Avatar.GetComponent<Rigidbody2D>().velocity = new Vector2(Avatar.GetComponent<Rigidbody2D>().velocity.x, -mushroomJumpVelocity);
                    audioSource.Play();
                    break;
                //Left
                case 90:
                    Avatar.GetComponent<AvatarMovement>().MushroomTimer = LengthOfPush;
                    Avatar.GetComponent<AvatarMovement>().isAccelerating = true;
                    if (Avatar.GetComponent<AvatarMovement>().isFacingRight) { Avatar.GetComponent<Rigidbody2D>().velocity = new Vector2(-mushroomPushVelocity , Avatar.GetComponent<Rigidbody2D>().velocity.y); }
                    else if (!Avatar.GetComponent<AvatarMovement>().isFacingRight) { Avatar.GetComponent<Rigidbody2D>().velocity = new Vector2(-mushroomPushVelocity - 5f, Avatar.GetComponent<Rigidbody2D>().velocity.y); }
                    audioSource.Play();
                    break;
                //Right
                case 270:
                    Avatar.GetComponent<AvatarMovement>().MushroomTimer = LengthOfPush;
                    Avatar.GetComponent<AvatarMovement>().isAccelerating = true;
                    if (Avatar.GetComponent<AvatarMovement>().isFacingRight) { Avatar.GetComponent<Rigidbody2D>().velocity = new Vector2(mushroomPushVelocity + 5f, Avatar.GetComponent<Rigidbody2D>().velocity.y); }
                    else if (!Avatar.GetComponent<AvatarMovement>().isFacingRight) { Avatar.GetComponent<Rigidbody2D>().velocity = new Vector2(mushroomPushVelocity, Avatar.GetComponent<Rigidbody2D>().velocity.y); }
                    audioSource.Play();
                    break;               
            }
        }
    }  
}
