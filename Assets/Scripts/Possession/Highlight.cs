using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Renderers that will be highlighted")]
    [SerializeField] private List<Renderer> renderers;

    [Header("Adjustable variables")]
    [SerializeField] private Color highlightColor = Color.white;

    private List<Material> materials;

    private void Awake()
    {
        SetupMaterials();
    }

    /// <summary>
    /// Gets materials from renderers and stores them in local variable 'materials'
    /// </summary>
    private void SetupMaterials()
    {
        materials = new List<Material>();
        foreach (Renderer renderer in renderers)
        {
            materials.AddRange(new List<Material>(renderer.materials));
        }
    }

    /// <summary>
    /// Turn highlight on or off
    /// </summary>
    public void ToggleHighlight(bool turnOn)
    {
        if (turnOn)
        {
            foreach (Material material in materials)
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", highlightColor);
            }
        }
        else
        {
            foreach (Material material in materials)
            {
                material.DisableKeyword("_EMISSION");
            }
        }
    }
}
