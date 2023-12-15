using UnityEngine;

public class OpenFridge : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    public void Activate()
    {
        _animator.SetTrigger("Open");
    }
}