using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movePower;
    public float jumpPower;
    public float jumpTimeLimit;

    Rigidbody2D rigid;
    new SpriteRenderer renderer;
    Animator animator;

    Vector3 movement;
    bool isJumping = false;
    bool onPlatform = false;
    float jumpTime;

    void Start()
    {
        movePower = 2f;
        jumpPower = 200f;
        jumpTimeLimit = 0.32f;
        rigid = gameObject.GetComponent<Rigidbody2D>();
        renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }


    void Update()
    {
        animator.SetFloat("VelocityY", rigid.velocity.y);

        if (onPlatform)
        {
            animator.SetBool("OnPlatform", true);

            if (Input.GetButtonDown("Jump"))
            {
                isJumping = true;
                jumpTime = 0;
            }
        }
        else
        {
            animator.SetBool("OnPlatform", false);
        }

        Debug.Log(onPlatform);
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        bool isWalking = false;
        
        if (Input.GetAxisRaw ("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;
            renderer.flipX = true;
            isWalking = true;
        }
        else if (Input.GetAxisRaw ("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;
            renderer.flipX = false;
            isWalking = true;
        }

        animator.SetBool("Walk", isWalking);
        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    void Jump()
    {
        if (!isJumping)
        {
            return;
        }
        else
        {
            rigid.velocity = Vector2.zero;

            Vector2 jumpVelocity = new Vector2(0, jumpPower);
            
            rigid.AddForce(jumpVelocity, ForceMode2D.Force);
            jumpTime += Time.deltaTime;

            if (!Input.GetButton("Jump") || jumpTime > jumpTimeLimit)
            {
                rigid.AddForce((-0.3f) * jumpVelocity, ForceMode2D.Force);
                isJumping = false;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        onPlatform = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        onPlatform = false;
    }
}
