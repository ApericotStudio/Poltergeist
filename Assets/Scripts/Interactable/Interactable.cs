using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Adjustable variables")]
    [Tooltip("Interactable is reusable")]
    [SerializeField] private bool _singleUse = false;
    private bool _used = false;

    public UnityEvent InteractEvent;

    public void Use()
    {
        if (_singleUse && _used)
        {
            return;
        }
        InteractEvent?.Invoke();
        _used = true;
        if (_singleUse)
        {
            if (gameObject.TryGetComponent(out Highlight highlight)){
                highlight.Highlightable(false);
            }
        }
    }
}
