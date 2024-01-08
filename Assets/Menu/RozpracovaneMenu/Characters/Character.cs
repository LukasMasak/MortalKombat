using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Character : MonoBehaviour
{
    [SerializeField]
    public Image BorderOne;
    public Image BorderPlayerTwo;
    public Image BorderBoth;

    private bool PlayerOneON;
    private bool PlayerTwoON;

    public enum _Character
    {
        jedna,
        dva,
        tri,
        ctyry
    }

    [SerializeField]
    private _Character charac;

    public _Character GetSelected()
    {
        Debug.Log("13");
        return charac;
    }

    private void Awake()
    {
        Selected(0);
    }

    public void Selected(int player)
    {
        switch (player)
        {
            case 1 :
                PlayerOneON = true;
                break;
            case 2:
                PlayerTwoON = true;
                break;

            default:
                PlayerOneON = false;
                PlayerTwoON = false;
                break;
        }

        if (PlayerOneON && PlayerTwoON)
        {
            BorderPlayerTwo.gameObject.SetActive(false);
            BorderOne.gameObject.SetActive(false);
            BorderBoth.gameObject.SetActive(true);
        }
        else if (PlayerOneON)
        {
            BorderBoth.gameObject.SetActive(false);
            BorderPlayerTwo.gameObject.SetActive(false);
            BorderOne.gameObject.SetActive(true);
        }
        else if (PlayerTwoON)
        {
            BorderBoth.gameObject.SetActive(false);
            BorderOne.gameObject.SetActive(false);
            BorderPlayerTwo.gameObject.SetActive(true);
        }
        else
        {
            BorderBoth.gameObject.SetActive(false);
            BorderOne.gameObject.SetActive(false);
            BorderPlayerTwo.gameObject.SetActive(false);
        }
    }
    public void Deselect(int player)
    {
        if(player == 1)
        {
            PlayerOneON = false;
        }
        else
        {
            PlayerTwoON = false;
        }

        if (PlayerOneON && PlayerTwoON)
        {
            BorderPlayerTwo.gameObject.SetActive(false);
            BorderOne.gameObject.SetActive(false);
            BorderBoth.gameObject.SetActive(true);
        }
        else if (PlayerOneON)
        {
            BorderBoth.gameObject.SetActive(false);
            BorderPlayerTwo.gameObject.SetActive(false);
            BorderOne.gameObject.SetActive(true);
        }
        else if (PlayerTwoON)
        {
            BorderBoth.gameObject.SetActive(false);
            BorderOne.gameObject.SetActive(false);
            BorderPlayerTwo.gameObject.SetActive(true);
        }
        else
        {
            BorderBoth.gameObject.SetActive(false);
            BorderOne.gameObject.SetActive(false);
            BorderPlayerTwo.gameObject.SetActive(false);
        }
    }

}
