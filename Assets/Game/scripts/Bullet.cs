using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D bulletRb;
    public float speed = 20f;
    private float _direction; // direction of shooting
    public bool isCrouching = false;
    public Animator animator;
    public int shootDirection;
    public bool inWater = false;
    // public AudioSource boom;
    
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
                shootDirection = 1;
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftArrow)) // shooting up left
                {
                    newVeloc = new Vector2(-1, 1).normalized * speed;
                    shootDirection = 1;
                }
                else // player shooting up
                {
                    newVeloc = transform.up * speed;
                    shootDirection = 2;
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
                    shootDirection = -1;
                }
                else
                {
                    if (Input.GetKey(KeyCode.LeftArrow)) // shooting down left
                    {
                        newVeloc = new Vector2(-1, -1).normalized * speed;
                        shootDirection = -1;
                    }
                    else
                    {
                        newVeloc = transform.right * speed;
                        shootDirection = 0;
                    }
                }
            }
            else // shoot forward
            {
                newVeloc = transform.right * speed;
                shootDirection = 0;

                // _shared.setShootDirection(0);
            }
        }

        if (shootDirection < 0 && inWater)
        {
            newVeloc = transform.right * speed;
            shootDirection = 0;
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
            animator.SetTrigger("EnemyHit");
            gameManager.playSoundEnemyHit();
            gameManager.increaseScore();
            bulletRb.velocity = Vector2.zero;
            other.gameObject.SetActive(false);
            StartCoroutine(explode());
            EnemyShoot enemyShoot = other.gameObject.GetComponent<EnemyShoot>();
            if (enemyShoot!=null && !enemyShoot.isCanon)
            {
                gameManager.instantiatePotion(other.transform);
            }
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

    private IEnumerator explode()
    {
        yield return new WaitForSeconds(0.2f);
        
        
        Destroy(gameObject);
    }
}