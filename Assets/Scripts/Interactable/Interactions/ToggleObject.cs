using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    [SerializeField]
    private bool _enable = false;
    public void Excecute()
    {
        this.gameObject.SetActive(_enable);
    }
}
