using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    private Animator _animator;
    private bool _isOn = false;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleAnimation()
    {
        _animator.SetBool("isOn", !_animator.GetBool("isOn"));
    }
}
