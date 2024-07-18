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
    [SerializeField] private Slider _dmglider;

    [SerializeField] private Slider _atkSizeSlider;
    [SerializeField] private Slider _atkXSlider;
    [SerializeField] private Slider _atkYSlider;
    [SerializeField] private Slider _atkFrameSlider;

    [SerializeField] private Animator _previewAnimator;

    //[SerializeField] private TMP_Dropdown _characterSelectDropdown;
    [SerializeField] private TMP_Dropdown _animationSelectDropdown;
    [SerializeField] private TMP_InputField _characterNameInput;

    public enum AnimationIndexes {
        Idle = 0,
        Attack,
        Move,
        Block,
        Jump,
        Hit,
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

    private void UpdateStatsWithSelectedCharacter()
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
        _selectedCharacter = GlobalState.AllCharacters[0];
        UpdateStatsWithSelectedCharacter();
        SwitchPreviewAnimation(0);
    }

    public void SaveCharacterChanges()
    {
        // TODO
        AssetDatabase.Refresh();
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
        AnimatorOverrideController overrideController = new AnimatorOverrideController(_previewAnimator.runtimeAnimatorController);
        _previewAnimator.runtimeAnimatorController = overrideController;

        if (idx == (int)AnimationIndexes.Idle)
        {
            _previewAnimator.enabled = true;
            overrideController["EmptyClip"] = _selectedCharacter.idleAnim;
        }
        else if (idx == (int)AnimationIndexes.Attack)
        {
            _previewAnimator.enabled = true;
            overrideController["EmptyClip"] = _selectedCharacter.attackAnim;
        }
        else if (idx == (int)AnimationIndexes.Move)
        {
            _previewAnimator.enabled = true;
            overrideController["EmptyClip"] = _selectedCharacter.walkAnim;
        }
        else if (idx == (int)AnimationIndexes.Block)
        {
            _previewAnimator.enabled = true;
            overrideController["EmptyClip"] = _selectedCharacter.blockAnim;
        }
        else if (idx == (int)AnimationIndexes.Jump)
        {
            _previewAnimator.enabled = true;
            overrideController["EmptyClip"] = _selectedCharacter.jumpAnim;
        }
        else if (idx == (int)AnimationIndexes.Hit)
        {
            _previewAnimator.enabled = true;
            overrideController["EmptyClip"] = _selectedCharacter.hurtAnim;
        }
        else if (idx == (int)AnimationIndexes.Death)
        {
            _previewAnimator.enabled = true;
            overrideController["EmptyClip"] = _selectedCharacter.deathAnim;
        }
        else if (idx == (int)AnimationIndexes.Icon)
        {
            _previewAnimator.enabled = false;
            _previewAnimator.GetComponent<SpriteRenderer>().sprite = _selectedCharacter.bubbleIcon;
        }
        else if (idx == (int)AnimationIndexes.Preview)
        {
            _previewAnimator.enabled = false;
            _previewAnimator.GetComponent<SpriteRenderer>().sprite = _selectedCharacter.preview;
        }

        _animationSelectDropdown.value = idx;
    }
}
