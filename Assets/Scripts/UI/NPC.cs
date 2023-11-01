using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<float> anxietyUpdated;

    [SerializeField]
    private float anxietySpeed;

    private float _anxiety;

    public float Anxiety
    {
        get
        {
            return _anxiety;
        }
        private set
        {
            _anxiety = value;
            anxietyUpdated.Invoke(value);
        }
    }

    private void Start()
    {
        Anxiety = 50f;
    }

    private void Update()
    {
        Anxiety -= anxietySpeed * Time.deltaTime;
    }
}
