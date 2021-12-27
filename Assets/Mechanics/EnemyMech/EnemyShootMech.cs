using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootMech : MonoBehaviour
{
    public  float waitBurst = 1;
    public float _angleToPlayer;
    public Vector2 toPlayer;
    public float fireRate = 500f;
    private float shootingTime = 0;
    public Transform shootingPoint;
    public GameObject bulletParent;
    public BulletMechEnemy bulletMechEnemy;
    private int curShoot = 0;

    private bool isFacingLeft = true;
    private bool _isRespawning = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !_isRespawning)
        {
            _angleToPlayer = Vector2.SignedAngle(transform.position, other.transform.position);
            toPlayer = (other.transform.position - transform.position).normalized;
            if (Time.time > shootingTime && curShoot < 3)
            {
                if (toPlayer.x > 0 && isFacingLeft)
                {
                    gameObject.transform.Rotate(0, 180, 0);
                    isFacingLeft = false;
                }

                if (toPlayer.x <= 0 && !isFacingLeft)
                {
                    gameObject.transform.Rotate(0, 180, 0);
                    isFacingLeft = true;
                }

                curShoot += 1;
                shootAtPlayer();
            }
        }
    }

    private void shootAtPlayer()
    {
        shootingTime = Time.time + fireRate / 1000;

        if (curShoot == 3)
        {
            StartCoroutine(waitit());
        }

        BulletMechEnemy bullet = Instantiate(bulletMechEnemy, shootingPoint.position, shootingPoint.rotation,
            bulletParent.transform);
        bullet.direction = _angleToPlayer;
        bullet.enemyPlayerVec = toPlayer;
    }

    private IEnumerator waitit()
    {
        yield return new WaitForSeconds(waitBurst);
        curShoot = 0;
    }

    public void playerRespawn()
    {
        _isRespawning = !_isRespawning;
    }
}