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


    private CharacterData _selectedCharacter;

    // Start is called before the first frame update
    void Start()
    {
        ReloadAllCharacters();
        
        _selectedCharacter = GlobalState.AllCharacters[0];
        UpdateUIWithSelectedCharacter();
    }

    private void UpdateUIWithSelectedCharacter()
    {
        _characterLabel.text = _selectedCharacter.name;
        _hpSlider.value = _selectedCharacter.health;
        _spdSlider.value = _selectedCharacter.speed;
        _dmglider.value = _selectedCharacter.damage;
        _atkSizeSlider.value = _selectedCharacter.attackSize;
        _atkXSlider.value = _selectedCharacter.attackPointOffset.x;
        _atkYSlider.value = _selectedCharacter.attackPointOffset.y;
        _atkFrameSlider.value = _selectedCharacter.attackFrameIdx;
    }

    public void ReloadAllCharacters()
    {
        CharacterLoader.LoadAllCharacters(GlobalState.AllCharacters);
    }
}
