using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movePower;
    public float jumpPower;
    public float jumpTimeMax;
    public float jumpTimeMin;
    public float jumpEndPower;
    public float shootTime;
    public GameObject seed;

    //for calculating max jump height (Debug)
    float YMax;
    float YMin;
    //

    Rigidbody2D rigid;
    new SpriteRenderer renderer;
    Animator animator;

    Vector3 movement;
    bool isJumping = false;
    bool onPlatform = false;
    float jumpTime = 0.9f;
    bool jumpMin = false;

    bool Shoot = false;

    void Start()
    {
        movePower = 11f;
        jumpPower = 1000f;
        jumpTimeMax = 0.35f;
        jumpTimeMin = 0.15f;
        jumpEndPower = -0.45f;
        shootTime = 0.1f;

        //
        YMax = transform.position.y;
        YMin = transform.position.y;
        //

        rigid = gameObject.GetComponent<Rigidbody2D>();
        renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }


    void Update()
    {
        //Debug.Log("onPlatform: " + onPlatform);
        animator.SetFloat("VelocityY", rigid.velocity.y);

        if (onPlatform)
        {
            animator.SetBool("OnPlatform", true);

            if (Input.GetButtonDown("Jump"))
            {
                isJumping = true;
                jumpMin = false;
                jumpTime = 0;
            }
        }
        
        if (!onPlatform)
        {
            animator.SetBool("OnPlatform", false);
        }

        if (Input.GetButtonDown("Fire1") && shootTime >= 0.1)
        {
            Shoot = true;
            shootTime = 0;
        }

        //
        if (transform.position.y > YMax)
        {
            YMax = transform.position.y;
        }
        if (transform.position.y < YMin)
        {
            YMin = transform.position.y;
        }
        //Debug.Log("(YMax - YMin)/PlayerHeight: " + (YMax - YMin)/2);
        //
    }

    void FixedUpdate()
    {
        Move();
        Jump();
        Shot();
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
        Vector2 jumpVelocity = new Vector2(0, jumpPower);
        jumpTime += Time.deltaTime;

        if (jumpMin && (jumpTime > jumpTimeMin))
        {
            jumpMin = false;
            rigid.AddForce((jumpEndPower) * jumpVelocity, ForceMode2D.Force);
            Debug.Log("-min");
            return;
        }
        if (!isJumping && (jumpTime > jumpTimeMin))
        {
            return;
        }
        else
        {
            rigid.velocity = Vector2.zero;
            
            rigid.AddForce(jumpVelocity, ForceMode2D.Force);

            if (jumpTime > jumpTimeMax)
            {
                rigid.AddForce((jumpEndPower) * jumpVelocity, ForceMode2D.Force);
                Debug.Log("-max");
                isJumping = false;
                return;
            }
            if (!Input.GetButton("Jump"))
            {   
                if (jumpTime < jumpTimeMin)
                {
                    jumpMin = true;
                }
                else
                {
                    rigid.AddForce((jumpEndPower) * jumpVelocity, ForceMode2D.Force);
                    Debug.Log("-timing");
                }
                isJumping = false;
            }
        }
    }

    void Shot()
    {
        if (!Shoot)
        {
            if (shootTime < 0.1)
            {
                shootTime += Time.deltaTime;
            }
            return;
        }
        else
        {
            Shoot = false;
            if (renderer.flipX == true)
            {
                GameObject newSeed = Instantiate(seed, transform.position + Vector3.down * 0.25f, transform.rotation);
                newSeed.GetComponent<Seed>().bulletDirection = -1;
            }
            else // renderer.flipX == false
            {
                GameObject newSeed = Instantiate(seed, transform.position + Vector3.down * 0.25f, transform.rotation);
                newSeed.GetComponent<Seed>().bulletDirection = 1;
            }
        }
    }
    
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == 8) // layer 8: platform
        {
            onPlatform = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 8) // layer 8: platform
        {
            onPlatform = false;
        }
    }
}
