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
    public float shootDelay;
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
    float shootTime;

    void Start()
    {
        movePower = 11f;
        jumpPower = 1000f;
        jumpTimeMax = 0.35f;
        jumpTimeMin = 0.15f;
        jumpEndPower = -0.45f;
        shootDelay = 0.1f;
        shootTime = shootDelay + 1;

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

        if (Input.GetButtonDown("Fire1") && shootTime >= shootDelay)
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
                }
                isJumping = false;
            }
        }
    }

    void Shot()
    {
        if (!Shoot)
        {
            if (shootTime < shootDelay)
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
                Debug.Log("fire");
                GameObject newSeed = Instantiate(seed, transform.position + Vector3.down * 0.25f, Quaternion.identity);
                newSeed.GetComponent<Seed>().bulletDirection = -1;
            }
            else // renderer.flipX == false
            {
                Debug.Log("fire");
                GameObject newSeed = Instantiate(seed, transform.position + Vector3.down * 0.25f, Quaternion.identity);
                newSeed.GetComponent<Seed>().bulletDirection = 1;
            }
        }
    }

    /// <summary>
    /// 현재는 EndPoint와 충돌 할 때 게임 종료 함수 호출한다.
    /// </summary>
    /// <param name="col">End Point</param>

    void OnTriggerEnter2D (Collider2D col)
    {
        if(col.gameObject.tag == "EndPoint")
        {
            GameManager.EndGame();
        }
        
        if (col.gameObject.layer == 8 || col.gameObject.layer == 11) // layer 8: platform, layer 11: platformTree
        {
            onPlatform = true;
        }
    }
    
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == 8 || col.gameObject.layer == 11) // layer 8: platform, layer 11: platformTree
        {
            onPlatform = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 8 || col.gameObject.layer == 11) // layer 8: platform, layer 11: platformTree
        {
            onPlatform = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        int cnt = 0;
        foreach (ContactPoint2D contact in col.contacts)
        {
            cnt++;
            // Visualize the contact point
            Debug.DrawRay(contact.point, contact.normal, Color.white);
            Debug.Log(contact.point.y + " " + cnt);
        }
    }
    void OnCollisionStay2D(Collision2D col)
    {
        int cnt = 0;
        foreach (ContactPoint2D contact in col.contacts)
        {
            cnt++;
            // Visualize the contact point
            Debug.DrawRay(contact.point, contact.normal, Color.white);
            Debug.Log(contact.point.y + " " + cnt);
        }
    }

}
