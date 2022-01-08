using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
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
    private float _lifeCount = 3; // will decrease  each time player dies
    private float _direction; // player moving left or right (-1,1)
    public bool crouch = false; // check crouch mode
    private RigidbodyConstraints2D _defConstraints;
    private bool _isGrounded = false; // is player on the ground
    private bool _facingRight = true;
    private bool _isRespawning = false; // check if player had been hit and waiting for spawn
    private Vector3 m_Velocity = Vector3.zero; // for movement smothning
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f; // How much to smooth out the movement
    private Transform _shootingPoint; // where the shoot is  being fired from
    private Vector2 originBcSize;
    private int _score = 0;
    
    //shield props
    public float shieldTime = 3; // Player has 3 seconds of shield to begin with
    private Boolean shieldActive = false;
    public GameObject shield;
    
    

    // Animations props
    [SerializeField] Animator animator;
    public int shootDirection = 0;
    public bool isShooting = false;
    private float _recallTime;
    private float _recallShootDelta = .3f;
    private bool airTime = false;
    private bool _isAimingUp = false;
    private bool isFading = false;
    private Vector2 _groundPos;
    private bool onPlatform = false;
    private bool _onWater;
    private bool isNearWaterPlatform = false;
    //audio
    public AudioSource shootSfx;
    private bool lowered;
    public int maxShieldTime;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        playerRb = GetComponent<Rigidbody2D>();
        playerBC = GetComponent<BoxCollider2D>();
        originBcSize = playerBC.size;
        playerRb.freezeRotation = true;
        _shootingPoint = GameObject.Find("shooting point").transform;
        _defConstraints = playerRb.constraints;
        shield.SetActive(false);
        shieldTime = maxShieldTime;
        // animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRb.position.x >= 147)
        {
            gameObject.SetActive(false);
            gameManager.playerWon();
            Invoke("Restart", 2);
        }

        if (_isGrounded)
        {
            _groundPos = transform.position;
        }

        if (Input.GetKey(KeyCode.DownArrow) && !_onWater) // crouch
        {
            crouch = true;
        }

        if (Input.GetKeyUp((KeyCode.DownArrow))) // stop crouch
        {
            crouch = false;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            _isAimingUp = true;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            _isAimingUp = false;
        }

        if (Input.GetKey(KeyCode.Z) && shieldTime > 0)
        {
            shieldTime -= Time.deltaTime;
            shieldActive = true;
            shield.SetActive(true);
            
        }
        else
        {
            shieldActive = false;
            shield.SetActive(false);
        }

        _direction = Input.GetAxisRaw("Horizontal");
        if (_direction != 0) // move left or right
        {
            crouch = false;
        }

        if (Input.GetKeyDown(KeyCode.X) && !isFading) //shoot
        {
            shootSfx.Play();
            doPlayerShoot();
        }
        else
        {
            if (isShooting)
            {
                if (Time.time > _recallTime + _recallShootDelta) // if players runs while shoots (Animation)
                {
                    isShooting = false;
                }
            }
        }


        if ((!isFading) &&
            ((_direction > 0 && !_facingRight) || (_direction < 0 && _facingRight))) // check direction, flip if needed
        {
            _facingRight = !_facingRight;
            transform.Rotate(0, 180, 0);
        }


        if (!crouch && _isGrounded && Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.DownArrow) &&
            !isFading && playerRb.velocity.y == 0) //jump
        {
            if (_onWater)
            {
                if (isNearWaterPlatform)
                {
                    animator.SetTrigger("jumpTrig");
                    playerRb.AddForce(new Vector2(0f, jumpVelocity / 2));
                    airTime = true;
                    StartCoroutine(playerInAir(.4f));
                }
            }
            else
            {
                animator.SetTrigger("jumpTrig");
                playerRb.AddForce(new Vector2(0f, jumpVelocity));
                airTime = true;
                StartCoroutine(playerInAir(.7f));
            }
        }


        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(_direction * speedFactor, playerRb.velocity.y);
        // And then smoothing it out and applying it to the character
        playerRb.velocity =
            Vector3.SmoothDamp(playerRb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        if (crouch && !lowered)
        {
            // transform.position = transform.position + Vector3.down * .5f;
            playerBC.size = new Vector2(0.35f, 0.1f);
            lowered = true;
        }

        if (!crouch)
        {
            if (lowered)
            {
                // transform.position = transform.position + Vector3.up * .5f;
                playerBC.size = originBcSize;
                // playerBC.size = new Vector2(playerBC.size.y, playerBC.size.x);
                lowered = false;
            }
        }
        setAnimationParams();
        
    }

    private void doPlayerShoot()
    {
        
        Bullet b = _bulletPrefab.GetComponent<Bullet>();
        b.isCrouching = crouch;
        isShooting = true;
        b.inWater = _onWater;
        _recallTime = Time.time;
        GameObject bOut = Instantiate(_bulletPrefab, _shootingPoint.position, _shootingPoint.rotation,
            bulletParent);
        shootDirection = bOut.GetComponent<Bullet>().shootDirection;
    }

    private void setAnimationParams()
    {
        if (_direction != 0 && _isGrounded)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        animator.SetBool("isShooting", isShooting);
        animator.SetInteger("shootDirection", shootDirection);
        animator.SetBool("isGrounded", _isGrounded);
        animator.SetBool("isAimingUp", _isAimingUp);
        if (!airTime && onPlatform)
        {
            _isGrounded = true;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            crouch = false;
        }

       


        animator.SetBool("isCrouching", crouch);
        
        animator.SetBool("inWater", _onWater);

        // animator.SetBool("isRespawning",_isRespawning);
    }

    private IEnumerator playerInAir(float t)
    {
        yield return new WaitForSeconds(t);
        airTime = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.gameObject.CompareTag("platform") && airTime == false)
        // {
        //     _isGrounded = true;
        // }
        if (other.gameObject.name == "lower wall")
        {
            // gameObject.SetActive(false);
            onPlatform = false;
            gameObject.transform.position = _groundPos;
            animator.SetTrigger("outOfBounds");
            gameManager.lossLife();
            return;
        }
        else
        {
            if (other.gameObject.CompareTag("platform"))
            {
                onPlatform = true;
                if (other.gameObject.name == "Water surface")
                {
                    _onWater = true;
                }
            }
        }

        if (other.gameObject.tag == "potion")
        {
            Destroy(other.gameObject);
            shieldTime = Math.Min(shieldTime + 3, maxShieldTime);
            
        }


        if (!_isRespawning && other.CompareTag("enemyBullet") && !shieldActive)
        {
            animator.SetTrigger("PlayerHit");
            gameManager.lossLife();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("platform"))
        {
            onPlatform = false;
            _isGrounded = false;
            if (other.gameObject.name == "Water surface")
            {
                _onWater = false;
            }

            if (other.gameObject.name == "Near water surface")
            {
                isNearWaterPlatform = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!_isRespawning)
        {
            if ((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("enemyBullet")) && !shieldActive ||
                other.gameObject.name == "lower wall")
            {
                animator.SetTrigger("PlayerHit");
                gameManager.lossLife();
            }
        }

        if (other.gameObject.name == "Near water surface")
        {
            isNearWaterPlatform = true;
        }
    }

    public void playerHit()
    {
        _isRespawning = true;
        playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
        playerRb.simulated = false;
        _lifeCount--;
        isFading = true;
        if (_lifeCount == 0)
        {

            playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        //todo: set 1 medal to not active
    }

    public void respawn()
    {
        //TODO: add fade blink animation when respawning
        gameObject.SetActive(true);
        playerRb.constraints = _defConstraints;
        playerRb.simulated = true;
        isFading = false;

        StartCoroutine(WaitToResapwn());
        
    }

    private IEnumerator WaitToResapwn()
    {
        yield return new WaitForSeconds(1.5f);
        _isRespawning = false;
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool isCrouching()
    {
        return crouch;
    }

    public bool isOnPlatform()
    {
        return onPlatform;
    }
}