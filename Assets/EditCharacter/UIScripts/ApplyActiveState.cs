using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
