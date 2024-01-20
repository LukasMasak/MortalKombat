using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingJoysticks : MonoBehaviour
{
    public KeyCode JoystickOne;
    public KeyCode JoystickTwo;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(JoystickOne))
        {
            Debug.Log("hrac jedna ");
        }
        if (Input.GetKeyDown(JoystickTwo))
        {
            Debug.Log("hrac jedna ");
        }
    }
}
