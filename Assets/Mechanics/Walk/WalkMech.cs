using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkMech : MonoBehaviour
{
    [SerializeField] private Rigidbody2D bodyRb;

    private Vector2 _force;
    private float direction = 0;
    private Vector3 m_Velocity=Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        bodyRb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        direction = 0;

        // Is right arrow pressed?
        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction += 1;
        }


        // Is left arrow pressed?
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction -= 1;
        }

    }

    private void FixedUpdate()
    {
        // bodyRb.AddForce(_force);
        
      
    }
}