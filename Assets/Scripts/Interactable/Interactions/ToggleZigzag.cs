using UnityEngine;

public class ToggleZigzag : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    
    public void Toggle()
    {
        _animator.SetBool("Activate", !_animator.GetBool("Activate"));
    }

    
}
