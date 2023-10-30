using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float throwVelocity;
    [SerializeField] private float step;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject arrow;

    private Rigidbody rb;
    private Renderer arrowrend;
    private Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rb = this.GetComponent<Rigidbody>();
        arrowrend = arrow.GetComponent<Renderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            rb.velocity = cam.transform.forward * throwVelocity;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log(cam.transform.forward);
        }

    }
}
