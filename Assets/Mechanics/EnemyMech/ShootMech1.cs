using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMech1 : MonoBehaviour
{
    private Transform _shootingPoint;
    public GameObject _bulletPrefab;
    public Transform bulletParent;

    // Start is called before the first frame update
    void Start()
    {
        _shootingPoint = GameObject.Find("shooting point Enemy").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            shoot();
        }
    }

    private void shoot()
    {
        Instantiate(_bulletPrefab, _shootingPoint.position, _shootingPoint.rotation, bulletParent);
    }
}