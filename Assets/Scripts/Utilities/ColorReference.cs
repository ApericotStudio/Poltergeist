using UnityEngine;

[CreateAssetMenu(fileName = "New Color Reference", menuName = "ScriptableObjects/Color Reference", order = 1)]
public class ColorReference : ScriptableObject
{
    [SerializeField] private Color _value = Color.white;
    public Color Value
    {
        get
        {
            return _value;
        }
        private set
        {
            _value = value;
        }
    }
}
