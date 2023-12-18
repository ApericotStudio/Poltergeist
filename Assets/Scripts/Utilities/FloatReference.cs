using UnityEngine;

[CreateAssetMenu(fileName = "New Float Reference", menuName = "ScriptableObjects/Float Reference", order = 1)]
public class FloatReference : ScriptableObject
{
    [SerializeField] private float _value = 0f;
    public float Value
    {
        get
        {
            return _value;
        }
    }
}
