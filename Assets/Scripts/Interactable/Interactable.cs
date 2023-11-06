using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Tooltip("Interactable is reusable")]
    [SerializeField] private bool singleUse = false;
    private bool isUsed = false;

    public UnityEvent InteractEvent;

    public void Use()
    {
        if (singleUse && isUsed)
        {
            return;
        }
        InteractEvent?.Invoke();
        isUsed = true;
    }
}
