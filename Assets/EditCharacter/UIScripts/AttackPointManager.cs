using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AttackPointManager : MonoBehaviour
{
    [SerializeField] private Slider _attackSizeSlider;
    [SerializeField] private Slider _attackXSlider;
    [SerializeField] private Slider _attackYSlider;
    [SerializeField] private Slider _attackFrameSlider;
    [SerializeField] private Animator _previewAnimator;
    [SerializeField]private Color _imageOnColor;
    [SerializeField]private Color _imageOffColor;

    private SpriteRenderer _attackPointSpriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        _attackPointSpriteRenderer = GetComponent<SpriteRenderer>();
        OnCoordSliderChange();
        OnSizeSliderChange();
    }


    // Changes the color of the attack point preview
    void Update()
    {
        float normalizedTime = _previewAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float length = _previewAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        int frameNum = (int)((normalizedTime * length) * CharacterLoader.FRAMERATE) + 1;
        if (frameNum == _attackFrameSlider.value)
        {
            _attackPointSpriteRenderer.color = _imageOnColor;
        }
        else 
        {
            _attackPointSpriteRenderer.color = _imageOffColor;
        }
    }


    private void OnEnable()
    {
        OnCoordSliderChange();
        OnSizeSliderChange();
    }


    public void OnCoordSliderChange()
    {
        transform.position = new Vector3(_attackXSlider.value, _attackYSlider.value, 0);
    }


    public void OnSizeSliderChange()
    {
        transform.localScale = new Vector3(_attackSizeSlider.value, _attackSizeSlider.value, 1);
    }
}
