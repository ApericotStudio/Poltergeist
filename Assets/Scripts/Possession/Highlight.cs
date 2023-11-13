using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Renderers that will be highlighted")]
    [SerializeField] private List<Renderer> _renderers;

    [Header("Adjustable variables")]
    [SerializeField] private Color _highlightColor = Color.white;

    private bool _highlightable;
    private List<Material> _materials;

    private void Awake()
    {
        SetupMaterials();
    }

    /// <summary>
    /// Gets materials from renderers and stores them in local variable 'materials'
    /// </summary>
    private void SetupMaterials()
    {
        _materials = new List<Material>();
        foreach (Renderer renderer in _renderers)
        {
            _materials.AddRange(new List<Material>(renderer.materials));
        }
    }

    /// <summary>
    /// Turn highlight on or off
    /// </summary>
    public void Highlighted(bool turnOn)
    {
        if (turnOn)
        {
            if (_highlightable)
            {
                return;
            }
            foreach (Material material in _materials)
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", _highlightColor);
            }
        }
        else
        {
            foreach (Material material in _materials)
            {
                material.DisableKeyword("_EMISSION");
            }
        }
    }

    /// <summary>
    /// Enable or disable the ability to highlight.
    /// </summary>
    public void Highlightable(bool value)
    {
        _highlightable = value;
        if (!_highlightable)
        {
            Highlighted(false);
        }
    }
}
