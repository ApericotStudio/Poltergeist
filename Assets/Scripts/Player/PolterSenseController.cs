using System.Collections.Generic;
using UnityEngine;

public class PolterSenseController : MonoBehaviour
{
    [SerializeField] private List<Outline> outlinesInRange = new List<Outline>();
    private bool isOn = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        if (!isOn)
        {
            foreach (Outline outline in outlinesInRange)
            {
                outline.enabled = true;
            }
            isOn = !isOn;
            return;
        }
        foreach (Outline outline in outlinesInRange)
        {
            outline.enabled = false;
        }
        isOn = !isOn;
    }

    public void AddOutline(Outline outline)
    {
        print("added");
        outlinesInRange.Add(outline);
        if (isOn)
        {
            outline.enabled = true;
        }
    }

    public void RemoveOutline(Outline outline)
    {
        print("removed");
        outlinesInRange.Remove(outline);
        outline.enabled = false;
    }
}
