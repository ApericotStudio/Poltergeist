using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Renderers that will be highlighted")]
    [SerializeField] private List<Renderer> _renderers;

    [Header("Adjustable variables")]
    [SerializeField] private Color _highlightColor = Color.white;

    private bool _disableHightlight;
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
    public void ToggleHighlight(bool turnOn)
    {
        if (turnOn)
        {
            if (_disableHightlight)
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
    /// <param name="enableHighlight"></param>
    public void EnableHighlight(bool enableHighlight)
    {
        _disableHightlight = !enableHighlight;
        if (_disableHightlight)
        {
            ToggleHighlight(false);
        }
    }
}
