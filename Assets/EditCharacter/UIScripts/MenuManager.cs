using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _characterLabel;
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Slider _spdSlider;
    [SerializeField] private Slider _dmgSlider;
    [SerializeField] private Slider _jmpSlider;
    [SerializeField] private Slider _atkSizeSlider;
    [SerializeField] private Slider _atkXSlider;
    [SerializeField] private Slider _atkYSlider;
    [SerializeField] private Slider _atkFrameSlider;

    [SerializeField] private FajtovPlayerAnimator _previewAnimator;

    [SerializeField] private TMP_Dropdown _animationSelectDropdown;
    [SerializeField] private TMP_InputField _characterNameInput;

    public enum DropdownAnimationIdxs {
        Idle = 0,
        Attack,
        Move,
        Block,
        Jump,
        Hurt,
        Death,
        Icon,
        Preview
    }

    private CharacterData _selectedCharacter;
    

    // Start is called before the first frame update
    void Start()
    {
        ReloadAllCharacters();
    }


    // Updates the UI with the loaded stats
    private void UpdateStatsWithSelectedCharacter()
    {
        _characterLabel.text = _selectedCharacter.name;
        _hpSlider.value = _selectedCharacter.health;
        _spdSlider.value = _selectedCharacter.speed;
        _dmgSlider.value = _selectedCharacter.damage;
        _jmpSlider.value = _selectedCharacter.jump;
        _atkSizeSlider.value = _selectedCharacter.attackSize;
        _atkXSlider.value = _selectedCharacter.attackPointOffset.x * 10;
        _atkYSlider.value = _selectedCharacter.attackPointOffset.y * 10;
        _atkFrameSlider.value = _selectedCharacter.attackFrameIdx;
        _atkFrameSlider.maxValue = (int)(_selectedCharacter.attackAnim.frames.Length * CharacterLoader.FRAMERATE);
    }


    // Reloads all characters
    public void ReloadAllCharacters()
    {
        CharacterLoader.LoadAllCharacters(GlobalState.AllCharacters);
        _selectedCharacter = GlobalState.AllCharacters[0];
        _previewAnimator.Initialize(ref _selectedCharacter);
        UpdateStatsWithSelectedCharacter();
        SwitchPreviewAnimation(0);
    }


    // Saves all changes made in the UI elements
    public void SaveCharacterChanges()
    {
        int charIdx = GlobalState.AllCharacters.IndexOf(_selectedCharacter);

        _selectedCharacter.health = (int)_hpSlider.value;
        _selectedCharacter.speed = _spdSlider.value;
        _selectedCharacter.damage = (int)_dmgSlider.value;
        _selectedCharacter.jump = _jmpSlider.value;
        _selectedCharacter.attackSize = _atkSizeSlider.value;
        _selectedCharacter.attackPointOffset.x = _atkXSlider.value / 10f;
        _selectedCharacter.attackPointOffset.y = _atkYSlider.value / 10f;
        _selectedCharacter.attackFrameIdx = (uint)_atkFrameSlider.value;

        GlobalState.AllCharacters[charIdx] = _selectedCharacter;
        CharacterLoader.SaveConfigOfCharacter(_selectedCharacter);
    }


    // Callback for selecting characters in CharacterSelectMenu
    public void SwitchCharacterToIdx(int idx)
    {
        int characterIdx;

        // Create a new character
        if (idx == 0) 
        {
            if (_characterNameInput.text.Length < 1) return;

            characterIdx = CharacterLoader.CreateFreshCharacter(_characterNameInput.text);
            _characterNameInput.text = "";
        }
        // Switch to a new character
        else 
        {
            characterIdx = idx - 1; // first option is Add character
        }

        _selectedCharacter = GlobalState.AllCharacters[characterIdx];
        UpdateStatsWithSelectedCharacter();
        SwitchPreviewAnimation(0);
    }


    // Callback for selecting animation in dropdown
    public void SwitchAnimationDropdown()
    {
        SwitchPreviewAnimation(_animationSelectDropdown.value);
    }


    // Switch the preview animation based on given animation index
    private void SwitchPreviewAnimation(int idx)
    {
        if (idx == (int)DropdownAnimationIdxs.Idle)
        {
            _previewAnimator.ChangeState(FajtovPlayerAnimator.FajtovAnimationStates.Idle, true);
        }
        else if (idx == (int)DropdownAnimationIdxs.Attack)
        {
            _previewAnimator.ChangeState(FajtovPlayerAnimator.FajtovAnimationStates.Attack, true);
        }
        else if (idx == (int)DropdownAnimationIdxs.Move)
        {
            _previewAnimator.ChangeState(FajtovPlayerAnimator.FajtovAnimationStates.Move, true);
        }
        else if (idx == (int)DropdownAnimationIdxs.Block)
        {
            _previewAnimator.ChangeState(FajtovPlayerAnimator.FajtovAnimationStates.Block, true);
        }
        else if (idx == (int)DropdownAnimationIdxs.Jump)
        {
            _previewAnimator.ChangeState(FajtovPlayerAnimator.FajtovAnimationStates.Jump, true);
        }
        else if (idx == (int)DropdownAnimationIdxs.Hurt)
        {
            _previewAnimator.ChangeState(FajtovPlayerAnimator.FajtovAnimationStates.Hurt, true);
        }
        else if (idx == (int)DropdownAnimationIdxs.Death)
        {
            _previewAnimator.ChangeState(FajtovPlayerAnimator.FajtovAnimationStates.Death, true);
        }
        else if (idx == (int)DropdownAnimationIdxs.Icon)
        {
            _previewAnimator.ShowBubbleIcon();
        }
        else if (idx == (int)DropdownAnimationIdxs.Preview)
        {
            _previewAnimator.ShowPreviewIcon();
        }

        _animationSelectDropdown.value = idx;
    }
}
