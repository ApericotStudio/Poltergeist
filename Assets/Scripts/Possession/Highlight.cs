using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Highlight : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Renderers that will be highlighted")]
    [SerializeField] private List<Renderer> _renderers;

    [Header("Adjustable variables")]
    [SerializeField] private Color _highlightColor = Color.white;

    private bool _highlightable = true;
    private List<Material> _materials;
    private Dictionary<Material, LocalKeyword> _emmissions;

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
        _emmissions = new Dictionary<Material, LocalKeyword>();
        foreach (Renderer renderer in _renderers)
        {
            _materials.AddRange(new List<Material>(renderer.materials));
            foreach (Material mat in renderer.materials)
            {
                _emmissions.Add(mat, new LocalKeyword(mat.shader, "_EMISSION"));
            }
        }
    }

    /// <summary>
    /// Turn highlight on or off
    /// </summary>
    public void Highlighted(bool turnOn)
    {
        if (turnOn)
        {
            if (!_highlightable)
            {
                return;
            }
            foreach (Material material in _materials)
            {
                material.SetColor("_EmissionColor", _highlightColor);
            }
        }
        else
        {
            foreach (Material material in _materials)
            {
                material.SetColor("_EmissionColor", new Color(1f/255f, 1f / 255f, 1f / 255f, 1f));
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
