using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D coll;
    // private BoxCollider2D coll;

    [SerializeField]private LayerMask jumpableGround;

    private float dirX;
    private bool onGround;
    [SerializeField]private float moveSpeed = 9f;
    [SerializeField]private float jumpForce = 14f;

    private enum MovementState
    {
     idle, 
     running, 
     jumping, 
     falling,   
     wallJump,   
     damage,   
    }

    // Start is called before the first frame update
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        // coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        onGround = Isgrounded();    
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rigidBody.velocity = new Vector2(dirX * moveSpeed,rigidBody.velocity.y);

        if (Input.GetButtonDown("Jump") && Isgrounded()) 
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x,jumpForce);
            onGround = false;
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            spriteRenderer.flipX = false;
        } 
        else if (dirX < 0)
        {
            state = MovementState.running;
            spriteRenderer.flipX = true;
        } else {
            state = MovementState.idle;
        }

        if (rigidBody.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        if (rigidBody.velocity.y < -.1f && onGround == false)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("AnimState", (int)state);
    }
    private bool Isgrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
