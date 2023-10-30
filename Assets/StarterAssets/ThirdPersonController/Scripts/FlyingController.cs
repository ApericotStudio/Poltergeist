using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingController : MonoBehaviour
{
    public float flySpeed;

    public float minFloat;

    public float maxFloat;

    public Camera freeLookCamera;

    public float currentHeight;

    private StarterAssetsInputs _input;

    // Start is called before the first frame update
    void Start()
    {
        currentHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        currentHeight = Mathf.Clamp(transform.position.y, currentHeight, maxFloat);

        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        if (_input.flying)
        {
            MoveCharacter();
        }
    }

    private void MoveCharacter()
    {
        Vector3 forward = freeLookCamera.transform.forward;
        Vector3 flyDirection = new Vector3(_input.fly.x, 0.0f, _input.move.y).normalized;

        currentHeight -= flyDirection.y * flySpeed * Time.deltaTime;
        currentHeight = Mathf.Clamp(currentHeight, minFloat, maxFloat);

        transform.position += flyDirection * flySpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);


    }
}
