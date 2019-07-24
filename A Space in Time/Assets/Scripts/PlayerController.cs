using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool canMove,
                canJump,
                isJumping,
                isFalling = false;

    private Vector2 targetPos;

    public Rigidbody2D playerRB;

    public Collider2D playerCollider,
                        groundCollider;

    public float moveSpeed,
                jumpHeight;

    public GameObject ground;

    private bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {

        if(playerRB.transform.position.y < -7.5 && !gameOver)
        {
            gameOver = true;
            GameManager.instance.GameOver();
        }
        
        //if(!isFalling)
        
        //{
            targetPos = new Vector2(ground.transform.position.x - moveSpeed * Time.deltaTime, ground.transform.position.y);
            ground.transform.position = targetPos;
        //}

        if(isFalling && isJumping)
        {
            targetPos = new Vector2(ground.transform.position.x - moveSpeed * Time.deltaTime, ground.transform.position.y);
            ground.transform.position = targetPos;
        }

        if(canJump)
        {
            //canMove = false;
            if (playerRB.velocity.y == 0)
            {
                canJump = false;
                isJumping = true;
                //playerRB.velocity = new Vector2(moveSpeed * jumpHeight * Time.deltaTime, jumpHeight);
               
                playerRB.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            }
            
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
}
