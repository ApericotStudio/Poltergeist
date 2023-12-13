using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaneraCollision : MonoBehaviour
{
    public Transform lookAt;
    public Transform camTransform;

    private float distance = 10.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float sensitivity = 4.0f;

    void Start()
    {
        camTransform = transform;
    }

    void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        RaycastHit hit;
        if (Physics.Raycast(lookAt.position, lookAt.position + rotation * dir, out hit, distance, layerMask))
        {
            camTransform.position = lookAt.position + rotation * new Vector3(0, 0, -hit.distance);
        }
        else
        {
            camTransform.position = lookAt.position + rotation * dir;
        }
        camTransform.LookAt(lookAt.position);
    }
}