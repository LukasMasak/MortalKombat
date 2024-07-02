using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _characterLabel;
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Slider _spdSlider;
    [SerializeField] private Slider _dmglider;

    [SerializeField] private Slider _atkSizeSlider;
    [SerializeField] private Slider _atkXSlider;
    [SerializeField] private Slider _atkYSlider;
    [SerializeField] private Slider _atkFrameSlider;


    private List<CharacterData> _allCharacters = new List<CharacterData>();

    // Start is called before the first frame update
    void Start()
    {
        CharacterLoader.LoadAllCharacters(_allCharacters);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
