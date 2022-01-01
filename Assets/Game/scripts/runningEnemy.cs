using System;
using System.Collections;
using System.Collections.Generic;
// using Unity.VisualScripting;
using UnityEngine;

public class runningEnemy : MonoBehaviour
{
    public Rigidbody2D Rigidbody2D;
    private bool isRunning = false;
    public float speed = 10;
    public float jumpVelocity = 10;
    private bool _isStuck = false;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning && Rigidbody2D.velocity == Vector2.zero)
        {
            _isStuck = true;
        }
        else
        {
            _isStuck = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !_isStuck)
        {
            Rigidbody2D.velocity = Vector2.left * speed;
            isRunning = true;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("border"))
        {
            gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("platform") && other.gameObject.name == "Water surface")
        {
            Rigidbody2D.velocity = Vector2.zero;
            animator.SetTrigger("inWater");
            StartCoroutine(fellToWater());
        }


        if (other.gameObject.CompareTag("platform")&& _isStuck)
        {
            Rigidbody2D.velocity = new Vector2(0f, 9);
            // _isStuck = false;
        }

        
    }

    private IEnumerator fellToWater()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
}