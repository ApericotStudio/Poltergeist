using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AimMode : MonoBehaviour
{
    private CinemachineVirtualCamera aimCam;
    [SerializeField] private float _rotationSpeed = 5;
    private bool _aimmode;
    private CinemachineVirtualCamera defaultCam;
    // Start is called before the first frame update
    private void Awake()
    {
        aimCam = this.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        if (this.gameObject.CompareTag("Player"))
        {
            defaultCam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        }
        else
        {
            defaultCam = this.gameObject.GetComponent<CinemachineVirtualCamera>();
        }
    }

    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (_aimmode)
        {
            float playerRotate = _rotationSpeed * Input.GetAxis("Mouse X");
            transform.Rotate(0, playerRotate, 0);
        }
    }

    public void OnEnable()
    {
        _aimmode = true;
        this.aimCam.Priority = 1;
        this.defaultCam.Priority = 0;
    }

    public void OnDisable()
    {
        _aimmode = false;
        this.aimCam.Priority = 0;
        this.defaultCam.Priority = 1;
    }
}
