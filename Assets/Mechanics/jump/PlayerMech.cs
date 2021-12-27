using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMech : MonoBehaviour
{
    //components and mask
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private BoxCollider2D playerBC;
    [SerializeField] private LayerMask platformsLayerMask;
    //public player fields
    public float jumpVelocity = 10;
    public float speedFactor = 50f; //Decide player move speed
    public GameObject _bulletPrefab;
    public Transform bulletParent;
    
    //private player characteristics 
    // private float _lifeCount = 3; // will decrease  each time player dies
    private float _direction; // player moving left or right (-1,1)
    [SerializeField] private bool crouch = false; // check crouch mode
    private RigidbodyConstraints2D _defConstraints;
    private bool _isGrounded = true; // is player on the ground
    private bool _facingRight = true;
    private bool _isRespawning = false; // check if player had been hit and waiting for spawn
    private Vector3 m_Velocity = Vector3.zero; // for movement smothning
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f; // How much to smooth out the movement
    private Transform _shootingPoint;// where the shoot is  being fired from
    
    // private RaycastHit2D rc;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerBC = GetComponent<BoxCollider2D>();
        playerRb.freezeRotation = true;
        _shootingPoint = GameObject.Find("shooting point").transform;
        _defConstraints = playerRb.constraints;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow)) // crouch
        {
            crouch = true;
        }

        if (Input.GetKeyUp((KeyCode.DownArrow))) // stop crouch
        {
            crouch = false;
        }

        _direction = Input.GetAxisRaw("Horizontal");
        if (_direction != 0) // move left or right
        {
            crouch = false;
        }

        if (Input.GetKeyDown(KeyCode.X)) //shoot
        {
            _bulletPrefab.GetComponent<BulletMech>().isCrouching = crouch;
            Instantiate(_bulletPrefab, _shootingPoint.position, _shootingPoint.rotation, bulletParent);
        }

        if (((_direction > 0 && !_facingRight) || (_direction < 0 && _facingRight)) &&
            !_isRespawning) // check direction, flip if needed
        {
            _facingRight = !_facingRight;
            transform.Rotate(0, 180, 0);
        }

        if (!crouch && _isGrounded && Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.DownArrow) &&
            !_isRespawning) //jump
        {
            // _playerRB.velocity = Vector2.up * jumpVelocity;
            playerRb.AddForce(new Vector2(0f, jumpVelocity));

        }


        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(_direction * speedFactor, playerRb.velocity.y);
        // And then smoothing it out and applying it to the character
        playerRb.velocity =
            Vector3.SmoothDamp(playerRb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("platform"))
        {
            _isGrounded = true;
        }

        // if (other.CompareTag("enemyBullet"))
        // {
        //     gameManager.lossLife();
        // }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("platform"))
        {
            _isGrounded = false;
        }
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.CompareTag("Enemy"))
    //     {
    //         gameManager.lossLife();
    //
    //     }
    // }
    //
    // public void playerHit()
    // {
    //     _isRespawning = true;
    //     playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
    //     playerRb.simulated = false;
    //     // StartCoroutine(respawn());
    //     _lifeCount--;
    //     if (_lifeCount == 0)
    //     {
    //         //TODO: change to game manager function to display game over etc
    //         print("Game over");
    //         Invoke("Restart", 2f);
    //     }
    //     //todo: set 1 medal to not active
    // }
    //
    // public void respawn()
    // {
    //     //TODO: add fade blink animation when respawning
    //     playerRb.constraints = _defConstraints;
    //     playerRb.simulated = true;
    //     _isRespawning = false;
    // }
    //
    // void Restart()
    // {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    // }
}