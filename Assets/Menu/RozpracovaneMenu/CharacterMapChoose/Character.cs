using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Character : MonoBehaviour
{
    [SerializeField]
    public Image BorderPlayerOne;
    public Image BorderPlayerTwo;
    public Image BorderBoth;
    public Image BorderDone;

    [Space]

    private bool PlayerOneON;
    private bool PlayerTwoON;

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

        //Both
        if (PlayerOneON && PlayerTwoON)
        {
            BorderPlayerTwo.gameObject.SetActive(false);
            BorderPlayerOne.gameObject.SetActive(false);
            BorderBoth.gameObject.SetActive(true);

        }
        //Left
        else if (PlayerOneON)
        {
            BorderBoth.gameObject.SetActive(false);
            BorderPlayerTwo.gameObject.SetActive(false);
            BorderPlayerOne.gameObject.SetActive(true);
        }
        //Right
        else if (PlayerTwoON)
        {
            BorderBoth.gameObject.SetActive(false);
            BorderPlayerOne.gameObject.SetActive(false);
            BorderPlayerTwo.gameObject.SetActive(true);
        }
        //none
        else
        {
            BorderBoth.gameObject.SetActive(false);
            BorderPlayerOne.gameObject.SetActive(false);
            BorderPlayerTwo.gameObject.SetActive(false);
        }
    }
    public void Deselect(int player)
    {
        GetUnChosen();

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
            BorderPlayerOne.gameObject.SetActive(false);
            BorderBoth.gameObject.SetActive(true);
        }
        else if (PlayerOneON)
        {
            BorderBoth.gameObject.SetActive(false);
            BorderPlayerTwo.gameObject.SetActive(false);
            BorderPlayerOne.gameObject.SetActive(true);
        }
        else if (PlayerTwoON)
        {
            BorderBoth.gameObject.SetActive(false);
            BorderPlayerOne.gameObject.SetActive(false);
            BorderPlayerTwo.gameObject.SetActive(true);
        }
        else
        {
            BorderBoth.gameObject.SetActive(false);
            BorderPlayerOne.gameObject.SetActive(false);
            BorderPlayerTwo.gameObject.SetActive(false);
        }
    }

    public void GetChosen()
    {
        BorderDone.gameObject.SetActive(true);
        BorderBoth.gameObject.SetActive(false);
        BorderPlayerOne.gameObject.SetActive(false);
        BorderPlayerTwo.gameObject.SetActive(false);
    }

    public void GetUnChosen()
    {
        BorderDone.gameObject.SetActive(false);
        BorderBoth.gameObject.SetActive(false);
        BorderPlayerOne.gameObject.SetActive(false);
        BorderPlayerTwo.gameObject.SetActive(false);
    }
}
