using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _SceneManager : MonoBehaviour
{
    public static _SceneManager instance { get; private set; }
    public  GameObject WinPlayerOneUI;
    public GameObject WinPlayerTwoUI;
    public GameObject reset;



    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    public void ShowWinUI(bool IsPlayerRight)
    {
        if(IsPlayerRight)
        {
            WinPlayerOneUI.SetActive(true);
            reset.SetActive(true);
        }
        else
        {
            WinPlayerTwoUI.SetActive(true);
            reset.SetActive(true);
        }
    }

}
