using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    [Header("Throw Controls")]
    [SerializeField] public bool throwMode;
    [SerializeField] private float throwForce;
    [SerializeField] [Tooltip("Extra sensitivity on y-axis for easier throwing")] private float ySense = 1;
    [SerializeField] float rotationSpeed = 10;
    private Vector3 releasePosition;

    [Header("Display Controls")]
    private LineRenderer lineRenderer;
    [SerializeField] [Range(10, 100)] private int linePoints = 25;
    [SerializeField] [Range(0.01f, 0.25f)] private float timeBetweenPoints = 0.1f;
    private LayerMask throwLayerMask;

    private Camera cam;
    private Rigidbody rb;
    private Vector3 aim;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
        rb = this.GetComponent<Rigidbody>();
        cam = Camera.main;

        int throwLayer = this.gameObject.layer;
        for (int i = 0; i < 32; i++)
        {
            if (!Physics.GetIgnoreLayerCollision(throwLayer, i))
            {
                throwLayerMask |= 1 << i;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        aim = cam.transform.forward;
        aim.y = aim.y * ySense;
        if (throwMode)
        {
            float playerRotate = rotationSpeed * Input.GetAxis("Mouse X");
            transform.Rotate(0, playerRotate, 0);
            if (Input.GetKey(KeyCode.Mouse0))
            {
                DrawProjection();
            } else
            {
                lineRenderer.enabled = false;
            }
            

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                ThrowObject();
            }
        }



    }
    private void ThrowObject()
    {
        rb.AddForce(aim * throwForce, ForceMode.Impulse);
    }

    private void DrawProjection()
    {
        releasePosition = transform.position;
        lineRenderer.enabled = true;
        lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
        Vector3 startPosition = releasePosition;
        Vector3 startVelocity = throwForce * aim / rb.mass;
        int i = 0;
        lineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < linePoints; time += timeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;

            //Trajectory formula here
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            lineRenderer.SetPosition(i, point);

            Vector3 lastPosition = lineRenderer.GetPosition(i - 1);

            if (Physics.Raycast(lastPosition, (point - lastPosition).normalized, out RaycastHit hit, (point - lastPosition).magnitude, throwLayerMask))
            {
                lineRenderer.SetPosition(i, hit.point);
                lineRenderer.positionCount = i + 1;
                return;
            }
        }
    }
}
