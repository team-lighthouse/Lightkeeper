using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float moveSpeed = 9f;
    float pusherMoveTime;
    int isPusherMove = 0; // -1: toLeft, 0: none, 1: toRight
    float pusherMoveSpeed = 30f;
    float pusherMoveTimeLimit = 0.36f;

    float jumpSpeed = 20f;
    float jumpTime;
    float jumpTimeMax = 0.23f;
    float jumpTimeMin = 0.06f;
    int defaultCanJumpNum = 1;
    public int canJumpNum; // Player_feet.cs, Jumper.cs, Item_jump.cs can change this variable, can_jump.cs check this variable
    public bool isJumping = false; // Player_head.cs can change this variable
    public bool isShortJump = false; // Player_head.cs can change this variable
    public bool isJumperJump = false; // Player_head.cs can change this variable
    float jumperJumpSpeed = 30f;
    float jumperJumpTimeLimit = 0.3f;
    bool jumpKeyPressed = false;

    float shootDelay = 0.4f;
    bool Shoot = false;
    float shootTime;
    public GameObject seed;

    float fallingSpeedMax = 25f;
    
    Rigidbody2D rb;
    new SpriteRenderer renderer;
    public Animator animator; // Player_feet.cs check animation for avoid 'side platform landing'
    InStageManager ISM;
    
    public bool live = true; // Enemies' scripts check this variable
    float deadTime = 0;
    public Sprite deadSprite;

    void Start()
    {
        pusherMoveTime = pusherMoveTimeLimit;

        canJumpNum = defaultCanJumpNum;
        jumpTime = jumpTimeMax;

        rb = gameObject.GetComponent<Rigidbody2D>();
        renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        animator = gameObject.GetComponentInChildren<Animator>();
        ISM = GameObject.FindGameObjectWithTag("Managers").GetComponent<InStageManager>();
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
            if (pusherMoveTime == 0)
            {
                rb.velocity = new Vector2(pusherMoveSpeed * isPusherMove, 20f);
                pusherMoveTime += Time.deltaTime;
                return;
            }
            else if (pusherMoveTime < pusherMoveTimeLimit * 0.9f)
            {
                rb.velocity = new Vector2(pusherMoveSpeed * isPusherMove, rb.velocity.y);
                pusherMoveTime += Time.deltaTime;
                return;
            }
            else if (pusherMoveTime < pusherMoveTimeLimit)
            {
                rb.velocity = new Vector2((pusherMoveSpeed - (pusherMoveTime - (pusherMoveTimeLimit * 0.9f)) * (pusherMoveSpeed - 10f) / (pusherMoveTimeLimit * 0.1f)) * isPusherMove, rb.velocity.y);
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
            rb.velocity = new Vector2(rb.velocity.x * 0.99f + Input.GetAxisRaw("Horizontal") * moveSpeed * 0.02f, rb.velocity.y);
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
                if (Input.GetAxisRaw("Horizontal") == 0)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
                else if (Input.GetAxisRaw("Horizontal") == 1f)
                {
                    if (rb.velocity.x < 0) // turn right
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                    else if (rb.velocity.x < moveSpeed) // accelerate to right
                    {
                        rb.velocity = new Vector2(rb.velocity.x + moveSpeed * 7f * Time.deltaTime, rb.velocity.y);
                    }
                    else // uniform move to right
                    {
                        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                    }
                }
                else // Input.GetAxisRaw("Horizontal") == -1f
                {
                    if (rb.velocity.x > 0) // turn left
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                    else if (rb.velocity.x > -moveSpeed) // accelerate to left
                    {
                        rb.velocity = new Vector2(rb.velocity.x - moveSpeed * 7f * Time.deltaTime, rb.velocity.y);
                    }
                    else // uniform move to left
                    {
                        rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                    }
                }
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

            GameObject newSeed = Instantiate(seed, transform.position + Vector3.down * 0.25f, Quaternion.identity);
            if (renderer.flipX == false) // see left
            {
                newSeed.GetComponent<Seed>().seedDirection = -1;
                newSeed.GetComponent<SpriteRenderer>().flipX = false;
            }
            else // renderer.flipX == true // see right
            {
                newSeed.GetComponent<Seed>().seedDirection = 1;
                newSeed.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    void Dead()
    {
        if (deadTime < 2f)
        {
            deadTime += Time.deltaTime;
            if (deadTime >= 1f)
            {
                animator.enabled = false;
                renderer.sprite = deadSprite;
            }
        }
        else // respawn
        {
            ISM.removeTrees();
            ISM.returnCheckPoint();
            rb.gravityScale = 1;
            rb.bodyType = RigidbodyType2D.Dynamic;
            animator.enabled = true;
            live = true;
            deadTime = 0;

            pusherMoveTime = pusherMoveTimeLimit;

            canJumpNum = 0;
            jumpTime = jumpTimeMax;
            isJumping = false;
            isJumperJump = false;
            isShortJump = false;

            gameObject.GetComponentInChildren<Player_body>().bodyHit = false;
        }
    }

    void Update()
    {
        if (!live)
        {

        }
        else
        {
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
                renderer.flipX = false; // see left
            }
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                renderer.flipX = true; // see right
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

            // detect shot key
            if (Input.GetKeyDown(KeyCode.Z) && shootTime >= shootDelay)
            {
                Shoot = true;
                shootTime = 0;
            }

            // detect dead
            if (gameObject.GetComponentInChildren<Player_body>().bodyHit)
            {
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
                rb.bodyType = RigidbodyType2D.Static;
                live = false;
                animator.SetBool("walk", false);
                deadTime = 0;
                // after 1s, sprite changes to 'dead'
            }
        }
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
        
        if (collision.gameObject.tag == "CheckPoint")
        {
            char sp = '_';
            string[] splited = collision.gameObject.name.Split(sp);
            int newIndex = int.Parse(splited[2]);
            InStageManager ISM = GameObject.Find("Managers").GetComponent<InStageManager>();

            if (PlayerPrefs.HasKey("CheckPoint_" + GameManager.sceneIndex.ToString()))
            {
                int storedIndex = PlayerPrefs.GetInt("CheckPoint_" + GameManager.sceneIndex.ToString());
                if (storedIndex != newIndex) // 다른 체크포인트일 경우 기존 올라온 깃발을 다시 내린다.
                {
                    Debug.Log("Check Point Renewed :" + " 0: " + splited[0] + " 1: " + splited[1] + " 2: " + splited[2]);
                    GameObject go = GameObject.Find("CheckPoint_" + GameManager.sceneIndex.ToString() + "_" + storedIndex.ToString()); // 기존 저장된 정보 찾는다.
                    SpriteRenderer srOld = go.GetComponent<SpriteRenderer>();
                    Sprite oldFlag = Resources.Load<Sprite>("Sprite/flag_off");
                    srOld.sprite = oldFlag;
                }
                else
                {
                    ISM.saveCoin();
                    return;
                }
            }

            PlayerPrefs.SetInt("CheckPoint_" + GameManager.sceneIndex, newIndex);
            InStageManager.StartingPos = collision.gameObject.transform.position;

            /// 아래는 새 이미지로 교체.
            SpriteRenderer srNew = collision.gameObject.GetComponent<SpriteRenderer>();
            Sprite newFlag = Resources.Load<Sprite>("Sprite/flag_on");
            srNew.sprite = newFlag;
            
            ISM.saveCoin();
        }

        if (collision.gameObject.tag == "Coin")
        {
            InStageManager ISM = GameObject.Find("Managers").GetComponent<InStageManager>();
            ISM.handleCoin(collision.gameObject);
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
