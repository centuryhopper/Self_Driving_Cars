using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow3D : MonoBehaviour
{
    [SerializeField] private Transform targetToFollow;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void Awake()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = targetToFollow.position + offset;
    }
}
