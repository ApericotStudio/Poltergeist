using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool IsOpen = false;
    [SerializeField] private bool _isRotatingDoor = true;
    [SerializeField] private float _speed = 1f;

    [Header("Rotation Configs")]
    [SerializeField] private float _rotationAmount = 90f;
    [SerializeField] private float _forwardDirection = 0;

    private Vector3 _awakeRotation;
    private Vector3 _forward;
    private Transform _pivot;

    private Coroutine _animationCoroutine;

    //remove this, npcs should also interact
    private GameObject _player;

    private void Awake()
    {
        _pivot = transform.parent;
        _awakeRotation = _pivot.rotation.eulerAngles;

        _forward = _pivot.right;
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Use()
    {
        if (IsOpen)
        {
            Close();
        } else
        {
            Open();
        }
    }


    public void Open()
    {
        Vector3 UserPosition = _player.transform.position;
        if (!IsOpen)
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }

            if (_isRotatingDoor)
            {
                float dot = Vector3.Dot(_forward, (UserPosition - _pivot.position).normalized);
                _animationCoroutine = StartCoroutine(DoRotationOpen(dot));
            }
        }
    }

    private IEnumerator DoRotationOpen(float ForwardAmount)
    {
        Quaternion startRotation = _pivot.rotation;
        Quaternion endRotation;
        if (ForwardAmount >= _forwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, _awakeRotation.y - _rotationAmount, 0));
        } else
        {
            endRotation = Quaternion.Euler(new Vector3(0, _awakeRotation.y + _rotationAmount, 0));
        }

        IsOpen = true;

        float time = 0;
        while (time < 1)
        {
            _pivot.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * _speed;
        }
    }

    public void Close()
    {
        if (IsOpen)
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }

            if (_isRotatingDoor)
            {
                _animationCoroutine = StartCoroutine(DoRotationClose());
            }
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRotation = _pivot.rotation;
        Quaternion endRotation = Quaternion.Euler(_awakeRotation);

        IsOpen = false;

        float time = 0;
        while (time < 1)
        {
            _pivot.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * _speed;
        }
    }
}
