using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ColliderManager : MonoBehaviour
{
    [SerializeField] private RectTransform _topSemiCircle;
    [SerializeField] private RectTransform _bottomSemiCircle;
    [SerializeField] private RectTransform _middleSquare;

    private float _width = 1;
    private float _height = 4;
    private float _offsetX = 0;
    private float _offsetY = 0;

    const float SCALE_MULT = 0.285f;


    public void SetWidth(Slider widthSlider)
    {
        _width = widthSlider.value;
        UpdateGraphic();
    }

    public void SetHeight(Slider heightSlider)
    {
        _height = heightSlider.value;
        UpdateGraphic();
    }

    public void SetOffsetX(Slider offsetXSlider)
    {
        _offsetX = offsetXSlider.value;
        UpdateGraphic();
    }

    public void SetOffsetY(Slider offsetYSlider)
    {
        _offsetY = offsetYSlider.value;
        UpdateGraphic();
    }

    private void UpdateGraphic()
    {
        if (_height < _width * 2) _height = _width * 2;

        float pixelWidth = _width * SCALE_MULT;
        float pixelHeight = _height * SCALE_MULT;

        float _squareHeight = (pixelHeight - (pixelWidth * 2))/2.0f;

        _topSemiCircle.localScale = new Vector2(pixelWidth, pixelWidth);
        Vector3 topSemiCirclePos = _topSemiCircle.anchoredPosition;
        topSemiCirclePos.y = 600 * _squareHeight/2.0f;
        _topSemiCircle.anchoredPosition = topSemiCirclePos;

        _bottomSemiCircle.localScale = new Vector2(pixelWidth, pixelWidth);
        Vector3 bottomSemiCirclePos = _bottomSemiCircle.anchoredPosition;
        bottomSemiCirclePos.y = 600 * -_squareHeight/2.0f;
        _bottomSemiCircle.anchoredPosition = bottomSemiCirclePos;

        _middleSquare.localScale = new Vector2(pixelWidth, _squareHeight);

        transform.position = new Vector3(_offsetX, _offsetY, 0) * 10;
    }
}
