using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow3D : MonoBehaviour
{
    [SerializeField] private Transform targetToFollow;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public Vector3 velocity = Vector3.zero;

    void Awake()
    {
    }

    void LateUpdate()
    {
        // https://www.youtube.com/watch?v=gqU1t1jpmDw
        var desiredPosition = targetToFollow.position + offset;

        var smoothDampedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed  * Time.deltaTime);
        transform.position = smoothDampedPosition;
    }
}
