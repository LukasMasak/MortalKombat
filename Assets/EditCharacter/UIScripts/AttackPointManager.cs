using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AttackPointManager : MonoBehaviour
{
    [SerializeField] private Slider _attackSizeSlider;
    [SerializeField] private Slider _attackXSlider;
    [SerializeField] private Slider _attackYSlider;
    [SerializeField] private Toggle _attackFrameToggle;
    [SerializeField] private FajtovPlayerAnimator _previewAnimator;
    [SerializeField]private Color _imageOnColor;
    [SerializeField]private Color _imageOffColor;

    private SpriteRenderer _attackPointSpriteRenderer;
    private int _attackFrameNum = 0;


    // Start is called before the first frame update
    void Start()
    {
        _attackPointSpriteRenderer = GetComponent<SpriteRenderer>();
        OnEnable();
    }


    public void SetAttackFrame(int value)
    {
        _attackFrameNum = value;
    }


    // Updates the indicator when turned on
    private void OnEnable()
    {
        OnCoordSliderChange();
        OnSizeSliderChange();
    }


    // Callback for changing the position of the attack point
    public void OnCoordSliderChange()
    {
        transform.position = new Vector3(_attackXSlider.value, _attackYSlider.value, 0) * 10;
    }


    // Callback for changing the size of the attack point
    public void OnSizeSliderChange()
    {
        transform.localScale = new Vector3(_attackSizeSlider.value, _attackSizeSlider.value, 1);
    }

    public void OnAttackFrameToggleChange()
    {
        if (!_attackFrameToggle.isOn) return;

        _attackFrameNum = _previewAnimator.GetCurrentFrameNum();
        OnFrameChange();
    }

    public void OnFrameChange()
    {
        if (_attackPointSpriteRenderer == null) Start();

        int frameNum = _previewAnimator.GetCurrentFrameNum();
        if (frameNum == _attackFrameNum)
        {
            _attackPointSpriteRenderer.color = _imageOnColor;
            _attackFrameToggle.isOn = true;
        }
        else 
        {
            _attackPointSpriteRenderer.color = _imageOffColor;
            _attackFrameToggle.isOn = false;
        }
    }
}
