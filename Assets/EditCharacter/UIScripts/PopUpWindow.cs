using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PopUpWindow : MonoBehaviour
{
    [SerializeField] private float timeToAppear = 0.5f;
    [SerializeField] private float timeToStay = 1.0f;
    [SerializeField] private float timeToDissapear = 1.0f;

    private CanvasGroup _canvasGroup;
    private TMP_Text _popUpText;
    private string _popUpMessage;
    private float _t = 0;

    private enum WindowStates {
        Appearing,
        Staying,
        Dissapearing
    }
    private WindowStates state = WindowStates.Appearing;

    public void Initialize(string message)
    {
        _popUpMessage = message;
    }

    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _popUpText = GetComponentInChildren<TMP_Text>();
        _canvasGroup.alpha = 0;
        _popUpText.text = _popUpMessage;

    }

    void Update()
    {
        if (state == WindowStates.Appearing)
        {
            _t += Time.deltaTime * (1f/timeToAppear);
            _canvasGroup.alpha = _t;

            if (_t >= 1)
            {
                _t = 0;
                state = WindowStates.Staying;
            }
        }
        else if (state == WindowStates.Staying)
        {
            _t += Time.deltaTime * (1f/timeToStay);

            if (_t >= 1)
            {
                _t = 0;
                state = WindowStates.Dissapearing;
            }
        }
        else if (state == WindowStates.Dissapearing)
        {
            _t += Time.deltaTime * (1f/timeToDissapear);
            _canvasGroup.alpha = 1 - _t;

            if (_t >= 1)
            {
                Destroy(gameObject);
            }
        }
    }
}
