using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movePower;
    public float jumpPower;
    public float jumpTimeLimit;
    public float jumpEndPower;
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
    float jumpTime = 0;

    bool Shoot = false;
    float shootTime = 0.5f;

    void Start()
    {
        movePower = 6f;
        jumpPower = 550f;
        jumpTimeLimit = 0.3f;
        jumpEndPower = -0.45f;

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
        Debug.Log("onPlatform: " + onPlatform);
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
        
        if (!onPlatform)
        {
            animator.SetBool("OnPlatform", false);
        }

        if (Input.GetButtonDown("Fire1") && shootTime >= 0.5)
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
        Debug.Log("(YMax - YMin)/PlayerHeight: " + (YMax - YMin));
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
                rigid.AddForce((jumpEndPower) * jumpVelocity, ForceMode2D.Force);
                isJumping = false;
            }
        }
    }

    void Shot()
    {
        if (!Shoot)
        {
            if (shootTime < 0.5)
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
                GameObject newSeed = Instantiate(seed, transform.position + (new Vector3(-0.7f, 0, 0)), transform.rotation);
                newSeed.GetComponent<Seed>().bulletDirection = -1;
            }
            else // renderer.flipX == false
            {
                GameObject newSeed = Instantiate(seed, transform.position + (new Vector3(+0.7f, 0, 0)), transform.rotation);
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
