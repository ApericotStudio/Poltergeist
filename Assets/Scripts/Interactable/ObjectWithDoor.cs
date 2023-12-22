using System.Collections;
using UnityEngine;

public class ObjectWithDoor : MonoBehaviour
{
    private bool _isOpen = false;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private bool _startOpen = false;

    [Header("Rotation Configs")]
    [SerializeField] private float _rotationAmount = 90f;
    [SerializeField] private RotationDirection _rotationDirection = RotationDirection.LeftOutward;
    [SerializeField] private RotationAxis _rotationAxis = RotationAxis.y;
    [SerializeField] private GameObject _pivotObject;

    private Vector3 _awakeRotation;
    private Transform _pivot;

    private Coroutine _animationCoroutine;

    private void Awake()
    {
        _pivot = _pivotObject.transform;
        _awakeRotation = _pivot.localRotation.eulerAngles;
        if (_startOpen) { _pivot.localRotation = GetRotation(); _isOpen = true; }
    }

    public void Use()
    {
        if (_isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }


    public void Open()
    {
        if (!_isOpen)
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }
            //This commented line is for the dynamic door
            //float dot = Vector3.Dot(_forward, (UserPosition - _pivot.position).normalized);

            if (_rotationDirection == RotationDirection.LeftInward)
            {
                _animationCoroutine = StartCoroutine(DoRotationLeftInward());
            }
            else
            {
                _animationCoroutine = StartCoroutine(DoRotationLeftOutward());
            }
        }
    }

    //Opens inwards if the doorhandle is on the left
    private IEnumerator DoRotationLeftInward()
    {
        Quaternion startRotation = _pivot.localRotation;
        Quaternion endRotation;

        endRotation = GetRotation();

        _isOpen = true;

        float time = 0;
        while (time < 1)
        {
            _pivot.localRotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * _speed;
        }
    }

    //Opens outwards if the doorhandle is on the left
    private IEnumerator DoRotationLeftOutward()
    {
        Quaternion startRotation = _pivot.localRotation;
        Quaternion endRotation;

        endRotation = GetRotation();


        _isOpen = true;

        float time = 0;
        while (time < 1)
        {
            _pivot.localRotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * _speed;
        }
    }

    public void Close()
    {
        if (_isOpen)
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }
            _animationCoroutine = StartCoroutine(DoRotationClose());
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRotation = _pivot.localRotation;
        Quaternion endRotation = Quaternion.Euler(_awakeRotation);

        _isOpen = false;

        float time = 0;
        while (time < 1)
        {
            _pivot.localRotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * _speed;
        }
    }

    private Quaternion GetRotation()
    {
        Quaternion rotation = _pivot.rotation;
        float _rotationWithDirection = _rotationAmount;
        if (_rotationDirection is RotationDirection.LeftInward)
        {
            _rotationWithDirection *= -1;
        }
        switch (_rotationAxis)
        {
            case RotationAxis.x:
                rotation = Quaternion.Euler(new Vector3(_awakeRotation.x + _rotationWithDirection, 0, 0));
                break;
            case RotationAxis.y:
                rotation = Quaternion.Euler(new Vector3(0, _awakeRotation.y + _rotationWithDirection, 0));
                break;
            case RotationAxis.z:
                rotation = Quaternion.Euler(new Vector3(0, 0, _awakeRotation.z + _rotationWithDirection));
                break;
        }
        return rotation;

    }

    private enum RotationDirection
    {
        LeftOutward,
        LeftInward
    }

    private enum RotationAxis
    {
        x,
        y,
        z
    }
}
