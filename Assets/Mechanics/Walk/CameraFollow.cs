using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;

    [SerializeField] private Transform target;

    private WalkMech _walkMech;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Middle").transform;
    }

    // Update is called once per frame

    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}