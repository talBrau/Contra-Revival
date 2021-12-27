using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMech : MonoBehaviour
{
    public Rigidbody2D bulletRb;
    public float speed = 20f;
    private float _direction; // direction of shooting
    public bool isCrouching = false;

    // Start is called before the first frame update
    void Awake()
    {
        ShootInDirection();
    }

    private void ShootInDirection()
    {
        Vector2 newVeloc;
        if (isCrouching) // if crouching shoot forward 
        {
            newVeloc = transform.right * speed;
            bulletRb.velocity = newVeloc;
            return;

        }
        
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (Input.GetKey(KeyCode.RightArrow)) // up right shoot
            {
                newVeloc = new Vector2(1, 1).normalized * speed;
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftArrow)) // shooting up left
                {
                    newVeloc = new Vector2(-1, 1).normalized * speed;
                }
                else // player shooting up
                {
                    newVeloc = transform.up * speed;
                }
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.DownArrow)) //shooting down
            {
                if (Input.GetKey(KeyCode.RightArrow)) // down right shoot
                {
                    newVeloc = new Vector2(1, -1).normalized * speed;
                }
                else
                {
                    if (Input.GetKey(KeyCode.LeftArrow)) // shooting down left
                    {
                        newVeloc = new Vector2(-1, -1).normalized * speed;
                    }
                    else 
                    {
                        newVeloc = transform.right * speed;

                    }
                }
            }
            else // shoot forward
            {
                newVeloc = transform.right * speed;
            }

           
        }


        bulletRb.velocity = newVeloc;
    }


    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" && !other.isTrigger) 
        {
            print("EnemyHit");
            other.gameObject.SetActive(false);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "border") // if bullet out of frame
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}