using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    private Button _button;
    private Slider _slider;
    private ColorBlock color;
    [SerializeField] private Color _selectedColor;
    // Start is called before the first frame update
    void Start()
    {
        if(TryGetComponent<Slider>(out Slider slider))
        {
            _slider = slider;
            color = _slider.colors;
            color.selectedColor = _selectedColor;
            _slider.colors = color;
        }

        if(TryGetComponent<Button>(out Button button))
        {
            _button = button;
            color = _button.colors;
            color.selectedColor = _selectedColor;
            _button.colors = color;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
