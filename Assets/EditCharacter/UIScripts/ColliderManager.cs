using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColliderManager : MonoBehaviour
{
    [SerializeField] private bool update = false;
    [SerializeField] private RectTransform topSemiCircle;
    [SerializeField] private RectTransform bottomSemiCircle;
    [SerializeField] private RectTransform middleSquare;

    [SerializeField] private float width;
    [SerializeField] private float height;
    [SerializeField] private float mult;


    private SpriteRenderer _topSemiCircleRenderer;
    private SpriteRenderer _bottomSemiCircleRenderer;
    private SpriteRenderer _middleSquareRenderer;


    // Start is called before the first frame update
    void Start()
    {
        _topSemiCircleRenderer = topSemiCircle.GetComponent<SpriteRenderer>();
        _bottomSemiCircleRenderer = bottomSemiCircle.GetComponent<SpriteRenderer>();
        _middleSquareRenderer = middleSquare.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (update) UpdateGraphic();
    }

    private void UpdateGraphic()
    {
        if (_topSemiCircleRenderer == null) Start();

        if (height < width * 2) height = width * 2;

        float pixelWidth = width * mult;
        float pixelHeight = height * mult;

        float _squareHeight = pixelHeight - (pixelWidth * 2);

        _topSemiCircleRenderer.size = new Vector2(pixelWidth, pixelWidth);
        Vector3 topSemiCirclePos = topSemiCircle.anchoredPosition;
        topSemiCirclePos.y = _squareHeight/2;
        topSemiCircle.anchoredPosition = topSemiCirclePos;

        _bottomSemiCircleRenderer.size = new Vector2(pixelWidth, pixelWidth);
        Vector3 bottomSemiCirclePos = bottomSemiCircle.anchoredPosition;
        bottomSemiCirclePos.y = -_squareHeight/2;
        bottomSemiCircle.anchoredPosition = bottomSemiCirclePos;

        _middleSquareRenderer.size = new Vector2(pixelWidth, _squareHeight);
    }
}
