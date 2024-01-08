using System.Collections.Generic;
using UnityEngine;

public class PolterSenseController : MonoBehaviour
{
    [SerializeField] private List<Outline> _outlinesInRange = new List<Outline>();
    [SerializeField] private bool _isOn = false;
    private PossessionController _possessionController;

    private void Start()
    {
        _possessionController = GetComponent<PossessionController>();
    }
    public void SetState(bool enable)
    {
        CheckCancellationTokens();
        _isOn = enable;
        if (enable)
        {
            foreach (Outline outline in _outlinesInRange)
            {
                if (_possessionController.CurrentPossession != null && outline.gameObject.GetComponent<Interactable>())
                {
                    outline.enabled = false;
                }
                else
                {
                    outline.enabled = true;
                }
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

    private void CheckCancellationTokens()
    {
        for (int i = 0; i < _outlinesInRange.Count; i++)
        {

            if (_outlinesInRange[i] == null || _outlinesInRange[i].destroyCancellationToken.IsCancellationRequested)
            {
                _outlinesInRange.RemoveAt(i);
            }

        }
    }
}
