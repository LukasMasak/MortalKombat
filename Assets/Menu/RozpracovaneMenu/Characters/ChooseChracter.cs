using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;



[RequireComponent(typeof(GridLayoutGroup))]
public class ChooseChracter : MonoBehaviour
{
    private GridLayoutGroup grid;
    private int rowCount;
    private int columCount;

    [SerializeField]
    private int PlayerOne = 0;
    [SerializeField]
    private int PlayerTwo = 0;

    private int charecterCount;

    private List<Character> characters = new();
    public Sprite[] bg;

    [Space]
    public Image showPlayerLeft;
    public Image showPlayerRight;

    private bool selectedOne, selectedTwo;

    private void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
        columCount = grid.constraintCount;
        charecterCount = transform.childCount;
        rowCount = (charecterCount + columCount - 1) / columCount;

        for(int i = 0; i < charecterCount; i++)
        {
            characters.Add(transform.GetChild(i).GetComponent<Character>());
        }

        characters[PlayerOne].Selected(1);
        characters[PlayerTwo].Selected(2);
    }

    enum direction
    {
        up,
        left,
        right,
        down
    }

    private void movePlayer(int i, direction d)
    {
        if (i == 1)
        {
            characters[PlayerOne].Deselect(1);
            selectedOne = false;

            if (d == direction.up)
            {
                PlayerOne = (int)Mathf.Clamp(PlayerOne - columCount, 0, charecterCount - 1);
            }
            else if(d == direction.down)
            {
                PlayerOne = (int)Mathf.Clamp(PlayerOne + columCount, 0, charecterCount - 1);
            }
            else if(d == direction.left)
            {
                PlayerOne = (int)Mathf.Clamp(PlayerOne - 1, 0, charecterCount -1);
            }
            else if (d == direction.right)
            {
                PlayerOne = (int)Mathf.Clamp(PlayerOne + 1, 0, charecterCount -1);
            }
            showPlayerLeft.sprite = bg[PlayerOne];
            GlobalState.PlayerOne = (Character._Character)PlayerOne;
            characters[PlayerOne].Selected(1);
        }
        else
        {
            characters[PlayerTwo].Deselect(2);
            selectedTwo = false;

            if (d == direction.up)
            {
                PlayerTwo = (int)Mathf.Clamp(PlayerTwo - columCount, 0, charecterCount - 1);
            }
            else if (d == direction.down)
            {
                PlayerTwo = (int)Mathf.Clamp(PlayerTwo + columCount, 0, charecterCount - 1);
            }
            else if (d == direction.left)
            {
                PlayerTwo = (int)Mathf.Clamp(PlayerTwo - 1, 0, charecterCount - 1);
            }
            else if (d == direction.right)
            {
                PlayerTwo = (int)Mathf.Clamp(PlayerTwo + 1, 0, charecterCount - 1);
            }
            showPlayerRight.sprite = bg[PlayerTwo];
            GlobalState.PlayerTwo = (Character._Character)PlayerTwo;
            characters[PlayerTwo].Selected(2);
        }
    }
    ////////////////////////////////////////////////////
    // Tady jsem to oficiálně ztratil, omlouvám se...

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            movePlayer(1, direction.up);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            movePlayer(1, direction.down);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            movePlayer(1, direction.left);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            movePlayer(1, direction.right);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            var nevim = characters[PlayerOne].GetChoosen();
            selectedOne = true;
        }

        ////////////////////////////////////////////////////


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            movePlayer(2, direction.up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            movePlayer(2, direction.down);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movePlayer(2, direction.left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            movePlayer(2, direction.right);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            var nevim = characters[PlayerTwo].GetChoosen();
            selectedTwo = true;
        }
        if (selectedOne && selectedTwo)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }



}
