using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public float fallMultiplier;
    [Range(1, 10)] public float jumpVelocity;
    public Rigidbody2D avatarRb;
    public Animator avatarAnimator;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (avatarRb.velocity.y < 0)
        {
            avatarRb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        avatarRb.velocity = new Vector2(moveSpeed, avatarRb.velocity.y);

        if (avatarRb.velocity.y == 0)
        {
            avatarAnimator.SetBool("inAir", false);
        }

    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (avatarRb.velocity.y == 0)
        {
            switch (col.tag)
            {
                case "Ground":
                    avatarRb.velocity = Vector2.up * jumpVelocity;
                    avatarAnimator.SetBool("inAir", true);
                    break;
            }
        }
    }
}
