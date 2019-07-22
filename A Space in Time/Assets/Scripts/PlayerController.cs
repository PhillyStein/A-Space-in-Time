﻿using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(playerCollider.IsTouching(groundCollider))
        {
            isJumping = false;
        }
        
        //if(!isFalling)
        
        //{
            targetPos = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
            transform.position = targetPos;
        //}

        if(isFalling && isJumping)
        {
            targetPos = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
            transform.position = targetPos;
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

        if(playerRB.velocity.y < 0)
        {
            isFalling = true;
        } else
        {
            isFalling = false;
        }

    }
}
