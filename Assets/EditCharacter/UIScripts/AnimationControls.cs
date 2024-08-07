using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAnimatorPreview : MonoBehaviour
{
    [SerializeField] private FajtovPlayerAnimator _animator;
    [SerializeField] private Button _playButton;
    [SerializeField] private GameObject _playOnGraphic;
    [SerializeField] private GameObject _playOffGraphic;

    private bool _isPlayButtonOn = false;


    // Toggles the animations on/off and changes the button graphic
    public void TogglePlayButton()
    {
        if (!_isPlayButtonOn)
        {
            _animator.ContinueAnimation();
            UpdateToggleAnimStarted();
        }
        else
        {
            _animator.StopAnimation();
            UpdateToggleAnimEnded();
        }
    }

    public void UpdateToggleAnimStarted()
    {
        _isPlayButtonOn = true;  

        _playOnGraphic.SetActive(_isPlayButtonOn);
        _playOffGraphic.SetActive(!_isPlayButtonOn);
    }

    public void UpdateToggleAnimEnded()
    {
        _isPlayButtonOn = false;

        _playOnGraphic.SetActive(_isPlayButtonOn);
        _playOffGraphic.SetActive(!_isPlayButtonOn);
    }
}
