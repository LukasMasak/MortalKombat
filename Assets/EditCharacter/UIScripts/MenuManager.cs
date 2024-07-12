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

    [SerializeField] private Animator _previewAnimator;


    public enum AnimationIndexes {
        Idle = 0,
        Attack,
        Move,
        Block,
        Jump,
        Hit,
        Death
    }


    private CharacterData _selectedCharacter;


    // Start is called before the first frame update
    void Start()
    {
        ReloadAllCharacters();
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
        _selectedCharacter = GlobalState.AllCharacters[0];
        UpdateUIWithSelectedCharacter();
        SwitchPreviewAnimation(0);
    }

    public void SaveCharacterChanges()
    {
        // TODO
    }

    public void SwitchAnimationDropdown(TMP_Dropdown dropdown)
    {
        SwitchPreviewAnimation(dropdown.value);
    }

    private void SwitchPreviewAnimation(int idx)
    {
        AnimatorOverrideController overrideController = new AnimatorOverrideController(_previewAnimator.runtimeAnimatorController);
        _previewAnimator.runtimeAnimatorController = overrideController;

        if (idx == (int)AnimationIndexes.Idle)
        {
            overrideController["EmptyClip"] = _selectedCharacter.idleAnim;
        }
        else if (idx == (int)AnimationIndexes.Attack)
        {
            overrideController["EmptyClip"] = _selectedCharacter.attackAnim;
        }
        else if (idx == (int)AnimationIndexes.Move)
        {
            overrideController["EmptyClip"] = _selectedCharacter.walkAnim;
        }
        else if (idx == (int)AnimationIndexes.Block)
        {
            overrideController["EmptyClip"] = _selectedCharacter.blockAnim;
        }
        else if (idx == (int)AnimationIndexes.Jump)
        {
            overrideController["EmptyClip"] = _selectedCharacter.jumpAnim;
        }
        else if (idx == (int)AnimationIndexes.Hit)
        {
            overrideController["EmptyClip"] = _selectedCharacter.hurtAnim;
        }
        else if (idx == (int)AnimationIndexes.Death)
        {
            overrideController["EmptyClip"] = _selectedCharacter.deathAnim;
        }
    }
}
