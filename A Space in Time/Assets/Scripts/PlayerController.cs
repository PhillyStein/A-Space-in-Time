using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool canMove,
                canJump,
                isJumping,
                isFalling = false;

    private int numOfJumps;

    private Vector2 targetPos;

    public Rigidbody2D playerRB;

    public Collider2D playerCollider,
                        groundCollider;

    public float moveSpeed,
                jumpHeight,
                playerLag;

    public GameObject ground;

    private bool gameOver;

    private Vector2 startPos;

    public static PlayerController instance;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        gameOver = false;
        
        playerLag = 0.0001f;

        startPos = ground.transform.position;

        /*
        if (GameManager.instance.gameStarted)
        {
            anim.SetBool("playerIdle", true);
            anim.SetBool("playerRunning", false);
            anim.SetBool("playerJumping", false);
        }
        else
        {
            anim.SetBool("playerIdle", false);
            anim.SetBool("playerRunning", true);
            anim.SetBool("playerJumping", false);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (isJumping)
        {
            //anim.SetBool("playerIdle", false);
            //anim.SetBool("playerRunning", false);
            anim.SetBool("playerJumping", true);
            anim.SetBool("playerJumping", false);
        }

        playerLag = (GameManager.instance.level - 2) * 0.0001f;

        if (playerRB.transform.position.x > -4.8f)
        {
            playerRB.transform.position = new Vector2(-4.8f, playerRB.transform.position.y);
        }

        if (!Input.anyKey && GameManager.instance.fogMoving && !GameManager.instance.isPaused)
        {
            this.transform.position = new Vector2(this.transform.position.x - playerLag, this.transform.position.y);
        }

        if ((playerRB.transform.position.y < -7.5 || playerRB.transform.position.x < -8.5) && !gameOver)
        {
            gameOver = true;
            GameManager.instance.GameOver();
        }

        if (!gameOver && !GameManager.instance.winState && GameManager.instance.gameStarted && !GameManager.instance.isPaused)
        {
            targetPos = new Vector2(ground.transform.position.x - moveSpeed * Time.deltaTime, ground.transform.position.y);
            ground.transform.position = targetPos;
        }

        if (isFalling && isJumping)
        {
            targetPos = new Vector2(ground.transform.position.x - moveSpeed * Time.deltaTime, ground.transform.position.y);
            ground.transform.position = targetPos;
        }

        if (canJump)
        {
            //canMove = false;
            if (playerRB.velocity.y == 0)
            {
                numOfJumps = 0;
                isJumping = true;
                //playerRB.velocity = new Vector2(moveSpeed * jumpHeight * Time.deltaTime, jumpHeight);

                playerRB.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                numOfJumps++;
                canJump = false;
            }
            else if (isJumping && numOfJumps < 2)
            {
                if (playerRB.velocity.y > 0)
                {
                    playerRB.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                } else
                {
                    playerRB.AddForce(Vector2.up * (jumpHeight + -playerRB.velocity.y), ForceMode2D.Impulse);
                }
                isJumping = true;
                numOfJumps++;
                canJump = false;
                
            }
            else
            {
                canJump = false;
                isJumping = false;
                numOfJumps = 0;

            }
            anim.SetBool("playerRunning", true);
            anim.SetBool("playerJumping", false);
            //playerRB.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }

        if(playerRB.velocity.y < 0 && playerRB.position.y < -1.5f)
        {
            isFalling = true;
        } else
        {
            isFalling = false;
        }
    
    }

    public void restartPlatforms()
    {
        ground.transform.position = new Vector2(playerRB.transform.position.x, startPos.y);
    }
}
