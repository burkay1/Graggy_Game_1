using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform player;        
    public float smoothSpeed = 0.125f;
    public Vector3 offset;          

    void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;
        targetPosition.z = offset.z;  

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
