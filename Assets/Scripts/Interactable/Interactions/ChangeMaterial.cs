using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    [SerializeField]
    private Material _offMaterial;
    [SerializeField]
    private Material _onMaterial;

    private Material _currentMaterial;
    private bool _turnedOn = false;
    private void Awake()
    {
        _currentMaterial = GetComponent<Material>();
    }

    public void Change()
    {
        if (!_turnedOn)
        {
            _currentMaterial = _onMaterial;
            _turnedOn = true;
        }

        else
        {
            _currentMaterial = _offMaterial;
            _turnedOn = false;
        }
    }
}
