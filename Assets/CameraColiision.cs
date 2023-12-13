using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColiision : MonoBehaviour
{
    public float collisionRadius = 0.5f;

    void Update()
    {
        RaycastHit hit;
        Vector3 offset = transform.forward * 0.1f;

        if (Physics.Raycast(transform.position + offset, transform.forward, out hit, Mathf.Infinity))
        {

            transform.position = hit.point - offset;
        }
    }
}
