using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;



[RequireComponent(typeof(GridLayoutGroup))]
public class ChooseChracter : MonoBehaviour
{
    private GridLayoutGroup grid;
    private int rowCount;
    private int columCount;

    public bool playerRight;

    [SerializeField]
    private int PlayerOne = 0;
    [SerializeField]
    private int PlayerTwo = 0;

    private int charecterCount;

    private List<Character> characters = new();

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
            if(d == direction.up)
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
            characters[PlayerOne].Selected(1);
        }
        else
        {
            characters[PlayerTwo].Deselect(2);

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
            characters[PlayerTwo].Selected(2);
        }
    }

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
            var nevim = characters[PlayerOne].GetSelected();
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
            var nevim = characters[PlayerOne].GetSelected();
        }
    }



}