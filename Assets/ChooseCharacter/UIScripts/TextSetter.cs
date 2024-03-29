using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI; 

    private Slider _slider;

    // Check all possible setter configs to set initial value
    private void Start()
    {
        _slider = GetComponent<Slider>();
        if (_slider != null) SetSliderValue();
    }

    // Sets the value of the text based on slider
    public void SetSliderValue()
    {
        _textMeshProUGUI.text = _slider?.value.ToString();
    }

}
