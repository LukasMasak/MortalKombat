using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAnimatorPreview : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Toggle _playToggle;
    [SerializeField] private GameObject _playOnGraphic;
    [SerializeField] private GameObject _playOffGraphic;


    // Toggles the animations on/off and changes the button graphic
    public void TogglePlay()
    {
        _playOnGraphic.SetActive(_playToggle.isOn);
        _playOffGraphic.SetActive(!_playToggle.isOn);

        if (_playToggle.isOn)
        {
            _animator.speed = 1;
        }
        else
        {
            _animator.speed = 0;
        }
    }


    // Moves the animation one frame forward
    public void NextFrame()
    {
        float normalizedTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float oneFrameLength = CharacterLoader.FRAME_DELAY / _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        float newTime =  Mathf.Min(normalizedTime + oneFrameLength, 0.99f);
        _animator.Play("Preview", 0, newTime);
    }


    // Moves the animation one frame backwards
    public void PrevFrame()
    {
        float normalizedTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float oneFrameLength = CharacterLoader.FRAME_DELAY / _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        float newTime =  Mathf.Max(0, normalizedTime - oneFrameLength);
        _animator.Play("Preview", 0, newTime);
    }
}
