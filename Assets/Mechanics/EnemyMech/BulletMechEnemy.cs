using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMechEnemy : MonoBehaviour
{
    public Rigidbody2D bulletRb;
    public float speed = 5f;
    public float direction;
    public Vector2 enemyPlayerVec; // vector between enemy ti player

    // Start is called before the first frame update
    void Start()
    {
        ShootInDirection();
    }

    public void ShootInDirection()
    {
        float targerSpeedX, targerSpeedY;
        targerSpeedX = calcCord(enemyPlayerVec.x);
        targerSpeedY = calcCord(enemyPlayerVec.y);
        print(targerSpeedX + "," + targerSpeedY);

     
        bulletRb.velocity = new Vector2(targerSpeedX, targerSpeedY).normalized;
        bulletRb.velocity *= speed;
    }

    private float calcCord(float cord)
    {
        float targerSpeed;
        if (cord < 0) // player to the left of enemy
        {
            if (cord > -0.25)
                targerSpeed = 0;
            else
            {
                if (cord < -0.75)
                    targerSpeed = -1;
                else
                {
                    targerSpeed = (float) (-1 / Math.Sqrt(2));
                }
            }
        }
        else // player to the right of enemy
        {
            if (cord < 0.25)
                targerSpeed = 0;
            else
            {
                if (cord > 0.75)
                    targerSpeed = 1;
                else
                {
                    targerSpeed = (float) (1 / Math.Sqrt(2));
                }
            }
        }

        return targerSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("hit player mech");
            gameObject.SetActive(false);
        }

        // if (other.gameObject.tag == "border")
        // {
        //     print("invisible enemy bullet");
        //     Destroy(gameObject);
        // }
    }

    private void OnBecameInvisible()
    {
        print("invisible enemy bullet");
        Destroy(gameObject);
    }
}