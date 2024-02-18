using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;



[RequireComponent(typeof(GridLayoutGroup))]
public class ChooseMap : MonoBehaviour
{
    private GridLayoutGroup grid;
    private int rowCount;
    private int columCount;

    public TextMeshProUGUI descriction;

    public string[] descriptions;

    [SerializeField]
    private int PlayerOne = 0;
    [SerializeField]
    private int PlayerTwo = 0;

    private int MapCount;

    private List<Maps> s_mapa = new();
    public Sprite[] bg;

    [Space]
    public Image ShowImageMap;
    public Sprite BGRandom;

    private bool selectedOne, selectedTwo;


    private void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
        columCount = grid.constraintCount;
        MapCount = transform.childCount;
        rowCount = (MapCount + columCount - 1) / columCount;

        for(int i = 0; i < MapCount; i++)
        {
            s_mapa.Add(transform.GetChild(i).GetComponent<Maps>());
        }

        s_mapa[PlayerOne].Selected(1);
        s_mapa[PlayerTwo].Selected(2);

        ShowImageMap.sprite = bg[0];
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
            s_mapa[PlayerOne].Deselect(1);
            selectedOne = false;

            if (d == direction.up)
            {
                PlayerOne = (int)Mathf.Clamp(PlayerOne - columCount, 0, MapCount - 1);
            }
            else if(d == direction.down)
            {
                PlayerOne = (int)Mathf.Clamp(PlayerOne + columCount, 0, MapCount - 1);
            }
            else if(d == direction.left)
            {
                PlayerOne = (int)Mathf.Clamp(PlayerOne - 1, 0, MapCount -1);
            }
            else if (d == direction.right)
            {
                PlayerOne = (int)Mathf.Clamp(PlayerOne + 1, 0, MapCount -1);
            }
            s_mapa[PlayerOne].Selected(1);
        }
        else
        {
            s_mapa[PlayerTwo].Deselect(2);
            selectedTwo = false;

            if (d == direction.up)
            {
                PlayerTwo = (int)Mathf.Clamp(PlayerTwo - columCount, 0, MapCount - 1);
            }
            else if (d == direction.down)
            {
                PlayerTwo = (int)Mathf.Clamp(PlayerTwo + columCount, 0, MapCount - 1);
            }
            else if (d == direction.left)
            {
                PlayerTwo = (int)Mathf.Clamp(PlayerTwo - 1, 0, MapCount - 1);
            }
            else if (d == direction.right)
            {
                PlayerTwo = (int)Mathf.Clamp(PlayerTwo + 1, 0, MapCount - 1);
            }
            s_mapa[PlayerTwo].Selected(2);
        }
        if (PlayerOne == PlayerTwo)
        {
            ShowImageMap.sprite = bg[PlayerTwo];
            GlobalState.Map = (Maps._Maps)PlayerTwo;
            descriction.text = descriptions[PlayerTwo];
        }
        else
        {
            ShowImageMap.sprite = BGRandom;
            float randomNumber = Random.value;
            descriction.text = "Random";

            if(randomNumber >= 0.5f)
            {
                GlobalState.Map = (Maps._Maps)PlayerOne;
            }
            else
            {
                GlobalState.Map = (Maps._Maps)PlayerTwo;
            }
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
            var nevim = s_mapa[PlayerOne].GetChoosen();
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
            var nevim = s_mapa[PlayerTwo].GetChoosen();
            selectedTwo = true;
        }
        if (selectedOne && selectedTwo)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }



}
