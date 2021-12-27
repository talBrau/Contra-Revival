using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private Transform _shootingPoint;
    public GameObject bulletPrefab;
    public Transform bulletParent;
    public int directionShoot = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        _shootingPoint = GameObject.Find("shooting point").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.X))
        // {
        //     shoot();
        // }
    }

    private void shoot()
    {
        Instantiate(bulletPrefab, _shootingPoint.position, _shootingPoint.rotation, bulletParent);
        // directionShoot = bul.shootDirection;
    }
}