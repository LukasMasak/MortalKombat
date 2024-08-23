using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplyActiveState : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToCopyActiveStateTo;

    private void OnEnable()
    {
        foreach (GameObject obj in objectsToCopyActiveStateTo)
        {
            obj.SetActive(true);
        }
    }

    private void OnDisable()
    {
        foreach (GameObject obj in objectsToCopyActiveStateTo)
        {
            obj.SetActive(false);
        }
    }

    public void Override(Toggle toggle)
    {
        if (toggle.isOn) OnEnable();
        else OnDisable();
    }
}
