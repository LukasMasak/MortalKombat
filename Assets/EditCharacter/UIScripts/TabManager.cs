using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    [SerializeField] private Color pressedTabColor;
    [SerializeField] private Color unpressedTabColor;
    [SerializeField] private Button[] tabButtons;
    [SerializeField] private GameObject[] tabs;
    [SerializeField] private int defaultSelection;

    private int _currentTabIdx = 0;


    // Setup listeners for the buttons and select a default
    void Start()
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            Button btn = tabButtons[i];
            var tempI = i;
            btn.onClick.AddListener(delegate{ButtonPressed(tempI);});
        }

        // Disable all tabs
        foreach (GameObject tab in tabs)
        {
            tab.SetActive(false);
        }

        // Select a default option
        tabButtons[defaultSelection].Select();
        ButtonPressed(defaultSelection);
    }


    // Unselect the last choice a select the idx choice
    public void ButtonPressed(int idx)
    {
        if (idx >= tabButtons.Length)
        {
            Debug.LogWarning("Invalid index of button " + idx + "!");
            return;
        }

        if (idx >= tabs.Length)
        {
            Debug.LogWarning("Invalid index of tab " + idx + "!");
            return;
        }

        // Turn off last tab
        tabs[_currentTabIdx].SetActive(false);
        tabButtons[_currentTabIdx].transition = Selectable.Transition.ColorTint;

        // Turn on new tab
        _currentTabIdx = idx;
        tabs[_currentTabIdx].SetActive(true);
        tabButtons[_currentTabIdx].transition = Selectable.Transition.None;
    }
}
