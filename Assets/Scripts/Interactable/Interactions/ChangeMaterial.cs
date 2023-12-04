using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    [SerializeField]
    private Material _offMaterial;
    [SerializeField]
    private Material _onMaterial;

    private MeshRenderer _renderer;
    private bool _turnedOn = false;
    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    public void Change()
    {
        if (!_turnedOn)
        {
            _renderer.material = _onMaterial;
            _turnedOn = true;
        }

        else
        {
            _renderer.material = _offMaterial;
            _turnedOn = false;
        }
    }
}
