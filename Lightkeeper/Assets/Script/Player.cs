using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float moveSpeed = 9f;
    float pusherMoveTime;
    int isPusherMove = 0; // -1: toLeft, 0: none, 1: toRight
    float pusherMoveSpeed = 60f;
    float pusherMoveTimeLimit = 0.25f;

    float jumpSpeed = 20f;
    float jumpTime;
    float jumpTimeMax = 0.23f;
    float jumpTimeMin = 0.06f;
    int defaultCanJumpNum = 1;
    public int canJumpNum; // Player_feet.cs, Jumper.cs, Item_jump.cs can change this variable
    public bool isJumping = false; // Player_head.cs can change this variable
    bool isShortJump = false;
    bool isJumperJump = false;
    float jumperJumpSpeed = 30f;
    float jumperJumpTimeLimit = 0.3f;
    bool jumpKeyPressed = false;

    //float shootDelay = 0.4f;
    //bool Shoot = false;
    //bool shootTime;
    
    float fallingSpeedMax = 25f;

    public GameObject seed;

    Rigidbody2D rb;
    new SpriteRenderer renderer;
    public Animator animator; // Player_feet.cs check animation for avoid 'side platform landing'

    bool live = true;
    float deadTime = 1f;

    void Start()
    {
        pusherMoveTime = pusherMoveTimeLimit;

        canJumpNum = defaultCanJumpNum;
        jumpTime = jumpTimeMax;

        rb = gameObject.GetComponent<Rigidbody2D>();
        renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        if (live)
        {
            Move();

            if (gameObject.GetComponentInChildren<Player_feet>().feetOnPlatform &&
                animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "rise" &&
                animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "fall") // reset canJumpNum when player animation is landing on platform
            {
                canJumpNum = defaultCanJumpNum;
            }
            Jump();

            Shot();
        }
        else
        {
            Dead();
        }

        if (gameObject.GetComponentInChildren<Player_body>().bodyHit)
        {
            live = false;
        }

        if (rb.velocity.y <= -fallingSpeedMax)
        {
            rb.velocity = new Vector2(rb.velocity.x, -fallingSpeedMax);
        }
    }

    void Move()
    {
        if (isPusherMove != 0) // -1(toLeft) or 1(toRight)
        {
            if (pusherMoveTime == 0f)
            {
                rb.velocity = new Vector2(pusherMoveSpeed * isPusherMove, 5f);
                pusherMoveTime += Time.deltaTime;
                return;
            }
            else if (pusherMoveTime < pusherMoveTimeLimit)
            {
                rb.velocity = new Vector2((pusherMoveSpeed - pusherMoveTime * 200f) * isPusherMove, rb.velocity.y);
                pusherMoveTime += Time.deltaTime;
                return;
            }
            else
            {
                isPusherMove = 0; // pusher move end
            }
        }

        if (gameObject.GetComponentInChildren<Player_feet>().feetOnIce) // move on ice
        {
            rb.velocity = new Vector2(rb.velocity.x * 0.99f + Input.GetAxisRaw("Horizontal") * moveSpeed * 0.04f, rb.velocity.y);
            if (Input.GetAxisRaw("Horizontal") * rb.velocity.x < 0) // velocity direction and input direction is opposite
            {
                animator.speed = 2f;
            }
            else
            {
                animator.speed = 1f;
            }
        }
        else // normal move
        {
            animator.speed = 1f;
            if (gameObject.GetComponentInChildren<Player_feet>().feetOnPlatform) // move on platform
            {
                rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb.velocity.y);
            }
            else // move on air
            {
                rb.velocity = new Vector2(rb.velocity.x * 0.9f + Input.GetAxisRaw("Horizontal") * moveSpeed, rb.velocity.y);
            }
        }
        
        if (rb.velocity.x >= moveSpeed)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else if (rb.velocity.x <= -moveSpeed)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }

        if (rb.velocity.x < 0.05f && rb.velocity.x > -0.05f)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        // TODO: Side spring
    }

    void Jump()
    {
        if (isPusherMove != 0) // if player use pusher, jump ends
        {
            isJumping = false;
            isJumperJump = false;
            isShortJump = false;
            return;
        }

        if (isJumperJump) // jump by jumper
        {
            if (jumpTime >= jumperJumpTimeLimit) // jumper jump end
            {
                isJumping = false;
                isJumperJump = false;
                return;
            }
            
            jumpTime += Time.deltaTime;
            rb.velocity = new Vector2(rb.velocity.x, jumperJumpSpeed);
            return;
        }

        if (isJumping) // jump by jump key input
        {
            if (jumpTime >= jumpTimeMin && isShortJump) // short jump end
            {
                isJumping = false;
                isShortJump = false;
                return;
            }

            if (jumpKeyPressed)
            {
                if (jumpTime >= jumpTimeMax) // max jump end 
                {
                    isJumping = false;
                    return;
                }
                else
                {
                    isJumping = true;
                    jumpTime += Time.deltaTime;
                    rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                }
            }
            else // jump key is not pressed
            {
                if (jumpTime >= jumpTimeMin) // normal jump end
                {
                    isJumping = false;
                    return;
                }
                else // jump key released before jumpTimeMin, it is short jump
                {
                    isJumping = true;
                    isShortJump = true;
                    jumpTime += Time.deltaTime;
                    rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                }
            }
        }
    }

    void Shot()
    {

    }

    void Dead()
    {
        
    }

    void Update()
    {
        // change animation
        animator.SetFloat("velocityY", rb.velocity.y);

        if (gameObject.GetComponentInChildren<Player_feet>().feetOnPlatform && !isJumping)
        {
            animator.SetBool("feetOnPlatform", true);
            animator.SetBool("walk", Input.GetAxisRaw("Horizontal") != 0);
        }
        else
        {
            animator.SetBool("feetOnPlatform", false);
            animator.SetBool("walk", false);
        }

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            renderer.flipX = false;
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            renderer.flipX = true;
        }

        // detect jumper (Jumper jump start)
        if (gameObject.GetComponentInChildren<Player_feet>().feetOnJumper) {
            gameObject.GetComponentInChildren<Player_feet>().feetOnJumper = false;
            canJumpNum = defaultCanJumpNum;
            canJumpNum--;
            isJumperJump = true;
            isJumping = true;
            jumpTime = 0f;
        }

        // detect jump key (Jump start)
        if (isPusherMove == 0 && !isJumperJump && Input.GetKeyDown(KeyCode.X) && canJumpNum > 0)
        { // is not using pusher && is not using jumper && canJumpNum is positive
            if (!gameObject.GetComponentInChildren<Player_feet>().feetOnPlatform) // jumping in air decreases canJumpNum
            {
                canJumpNum--;
            }

            isJumping = true;
            isShortJump = false;
            jumpTime = 0f;
        }

        jumpKeyPressed = Input.GetKey(KeyCode.X);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pusher_toRight"))
        {
            isPusherMove = 1;
            pusherMoveTime = 0f;
        }
        else if (collision.gameObject.CompareTag("Pusher_toLeft"))
        {
            isPusherMove = -1;
            pusherMoveTime = 0f;
        }

        // TODO? : coin item will be managed by GameManager
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

}
