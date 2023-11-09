using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Adjustable variables")]
    [Tooltip("Interactable is reusable")]
    [SerializeField] private bool singleUse = false;
    private bool used = false;

    public UnityEvent InteractEvent;

    public void Use()
    {
        if (singleUse && used)
        {
            return;
        }
        InteractEvent?.Invoke();
        used = true;
        if (singleUse)
        {
            if (gameObject.TryGetComponent(out Highlight highlight)){
                highlight.EnableHighlight(false);
            }
        }
    }
}
