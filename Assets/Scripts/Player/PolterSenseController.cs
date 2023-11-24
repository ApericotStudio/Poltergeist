using System.Collections.Generic;
using UnityEngine;

public class PolterSenseController : MonoBehaviour
{
    [SerializeField] private List<Outline> _outlinesInRange = new List<Outline>();
    [SerializeField] private bool _isOn = false;

    public void SetState(bool enable)
    {
        _isOn = enable;
        if (enable)
        {
            foreach (Outline outline in _outlinesInRange)
            {
                outline.enabled = true;
            }
            return;
        }
        foreach (Outline outline in _outlinesInRange)
        {
            outline.enabled = false;
        }
    }

    public void AddOutline(Outline outline)
    {
        _outlinesInRange.Add(outline);
        if (_isOn)
        {
            outline.enabled = true;
        }
    }

    public void RemoveOutline(Outline outline)
    {
        _outlinesInRange.Remove(outline);
        outline.enabled = false;
    }
}