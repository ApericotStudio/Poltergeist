using System.Collections.Generic;
using UnityEngine;

public class PolterSenseController : MonoBehaviour
{
    [SerializeField] private List<Outline> outlines = new List<Outline>();
    private bool isOn = false;
    private float range = 5;

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
            foreach (Outline outline in outlines)
            {
                outline.enabled = false;
            }
            isOn = !isOn;
            return;
        }
        foreach (Outline outline in outlines)
        {
            if (InRange(outline.gameObject))
            {
                outline.enabled = true;
            }
        }
        isOn = !isOn;
    }

    private bool InRange(GameObject other)
    {
        if (Vector3.Distance(transform.position, other.transform.position) < range)
        {
            return true;
        }
        return false;
    }
}
