using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMoveMech : MonoBehaviour
{
    [SerializeField] private Rigidbody2D bodyRb;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f; // How much to smooth out the movement

    private bool _facingRight = true;
    private Vector2 _force;
    private Vector3 mVelocity;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float direction = 0;

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

        if ((direction > 0 && !_facingRight) || direction<0 && _facingRight)
        {
            flip();
        }

        const float factor = 10;
        _force = new Vector2(direction * factor, 0);
        Vector3 targetVelocity = new Vector2(direction * factor, bodyRb.velocity.y);
        // And then smoothing it out and applying it to the character
        mVelocity = Vector3.zero;
        bodyRb.velocity =
            Vector3.SmoothDamp(bodyRb.velocity, targetVelocity, ref mVelocity, m_MovementSmoothing);
    }
    

    // private void FixedUpdate()
    // {
    //     bodyRb.AddForce(_force);
    // }

    private void flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(0,180,0);

    }
}
