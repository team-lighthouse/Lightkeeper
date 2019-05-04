﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float moveSpeed = 9f;

    float jumpSpeed = 20f;
    float jumpTime;
    float jumpTimeMax = 0.22f;
    float jumpTimeMin = 0.028f;
    int defaultCanJumpNum = 1;
    public int canJumpNum; // Player_feet can change this variable
    public bool isJumping = false; // Player_head can change this variable
    bool isShortJump = false;
    bool jumpKeyPressed = false;

    float shootDelay = 0.4f;
    //bool Shoot = false;
    //bool shootTime;
    
    float fallingSpeedMax = 20f;

    public GameObject seed;

    Rigidbody2D rb;
    new SpriteRenderer renderer;
    Animator animator;

    bool live = true;
    float deadTime = 1f;

    void Start()
    {
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

            if (gameObject.GetComponentInChildren<Player_feet>().feetOnPlatform & !isJumping) // reset canJumpNum when player land on platform
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
        if (gameObject.GetComponentInChildren<Player_feet_center>().feetOnIce) // move on ice
        {
            rb.velocity = new Vector2(rb.velocity.x + Input.GetAxisRaw("Horizontal") * moveSpeed * 0.05f, rb.velocity.y);

            if (rb.velocity.x >= moveSpeed)
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            }
            else if (rb.velocity.x <= -moveSpeed)
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            }
        }
        else // normal move
        {
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb.velocity.y);
        }

        // TODO: Side spring
    }

    void Jump()
    {
        if (isJumping)
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
                }
                else
                {
                    isJumping = true;
                    jumpTime += Time.deltaTime;
                }
            }
            else // jump key is not pressed
            {
                if (jumpTime >= jumpTimeMin) // normal jump end
                {
                    isJumping = false;
                }
                else // jump key released before jumpTimeMin, it is short jump
                {
                    isJumping = true;
                    isShortJump = true;
                    jumpTime += Time.deltaTime;
                }
            }
        }

        if (isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
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
        Debug.Log("canJumpNum");
        Debug.Log(canJumpNum);
        Debug.Log("isJumping");
        Debug.Log(isJumping);
        Debug.Log("jumpKeyPressed");
        Debug.Log(jumpKeyPressed);

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

        // detect jump key (Jump start)
        if (Input.GetKeyDown(KeyCode.X) && canJumpNum > 0) // canJumpNum check
        {
            if (!gameObject.GetComponentInChildren<Player_feet>().feetOnPlatform) // jumping in air decreases canJumpNum
            {
                canJumpNum--;
            }

            isJumping = true;
            isShortJump = false;
            jumpTime = 0f;
            Debug.Log("jumpTime reset");
        }

        jumpKeyPressed = Input.GetKey(KeyCode.X);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: Side spring
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

}
