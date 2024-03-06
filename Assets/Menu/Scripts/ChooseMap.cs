using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(GridLayoutGroup))]
public class ChooseMap : MonoBehaviour
{
    [SerializeField] private Animator _fadeInOutAnimator;
    [SerializeField] private int _player1Choice = 0;
    [SerializeField] private int _player2Choice = 0;
    
    [SerializeField] private Image _mapPreview;

    // Map previews, last one is the random image
    [SerializeField] private Sprite[] _mapPreviews;
    [SerializeField] private string[] _mapDescriptions;

    private int _columCount;
    private int _mapCount;

    private List<ChooseBubble> _mapBubbles = new();
    private bool _hasChosenPlayer1, _hasChosenPlayer2;


    // Get all map bubbles and params
    private void Start()
    {
        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
        _columCount = grid.constraintCount;
        _mapCount = transform.childCount;

        // Get all map bubbles
        for(int i = 0; i < _mapCount; i++)
        {
            _mapBubbles.Add(transform.GetChild(i).GetComponent<ChooseBubble>());
        }

        // Select default choices
        _mapBubbles[_player1Choice].SelectByPlayer(GlobalState.Player.one);
        _mapBubbles[_player2Choice].SelectByPlayer(GlobalState.Player.two);
        _mapPreview.sprite = _mapPreviews[_player1Choice];
    }

    private void Update()
    {
        // Player 1 input
        if (Input.GetButtonDown("Player1Up"))
        {
            MovePlayer(GlobalState.Player.one, GlobalState.Direction.up);
        }
        if (Input.GetButtonDown("Player1Down"))
        {
            MovePlayer(GlobalState.Player.one, GlobalState.Direction.down);
        }
        if (Input.GetButtonDown("Player1Right"))
        {
            MovePlayer(GlobalState.Player.one, GlobalState.Direction.right);
        }
        if (Input.GetButtonDown("Player1Left"))
        {
            MovePlayer(GlobalState.Player.one, GlobalState.Direction.left);
        }
        if (Input.GetButtonDown("Player1Accept"))
        {
            _mapBubbles[_player1Choice].GetChosen();
            _hasChosenPlayer1 = true;
        }

        // Player 2 input
        if (Input.GetButtonDown("Player2Up"))
        {
            MovePlayer(GlobalState.Player.two, GlobalState.Direction.up);
        }
        if (Input.GetButtonDown("Player2Down"))
        {
            MovePlayer(GlobalState.Player.two, GlobalState.Direction.down);
        }
        if (Input.GetButtonDown("Player2Right"))
        {
            MovePlayer(GlobalState.Player.two, GlobalState.Direction.right);
        }
        if (Input.GetButtonDown("Player2Left"))
        {
            MovePlayer(GlobalState.Player.two, GlobalState.Direction.left);
        }
        if (Input.GetButtonDown("Player2Accept"))
        {
            _mapBubbles[_player2Choice].GetChosen();
            _hasChosenPlayer2 = true;
        }

        // Continue to next scene
        if (_hasChosenPlayer1 && _hasChosenPlayer2)
        {
            // Set the map to be loaded based on player choices
            if (_player1Choice != _player2Choice)
            {
                GlobalState.Map = (Random.value > 0.5) ? (GlobalState.Maps)_player1Choice : (GlobalState.Maps)_player2Choice;
            }
            else
            {
                GlobalState.Map = (GlobalState.Maps)_player1Choice;
            }
            _fadeInOutAnimator.SetTrigger("StartEndMenu");
        }
    }

    // Moves the player selection of a given player in a given direction 
    private void MovePlayer(GlobalState.Player player, GlobalState.Direction direction)
    {
        // Player one selection
        if (player == GlobalState.Player.one)
        {
            _mapBubbles[_player1Choice].Deselect(GlobalState.Player.one);
            _hasChosenPlayer1 = false;

            if (direction == GlobalState.Direction.up)
            {
                _player1Choice = Mathf.Clamp(_player1Choice - _columCount, 0, _mapCount - 1);
            }
            else if(direction == GlobalState.Direction.down)
            {
                _player1Choice = Mathf.Clamp(_player1Choice + _columCount, 0, _mapCount - 1);
            }
            else if(direction == GlobalState.Direction.left)
            {
                _player1Choice = Mathf.Clamp(_player1Choice - 1, 0, _mapCount -1);
            }
            else if (direction == GlobalState.Direction.right)
            {
                _player1Choice = Mathf.Clamp(_player1Choice + 1, 0, _mapCount -1);
            }
            _mapBubbles[_player1Choice].SelectByPlayer(GlobalState.Player.one);
        }
        
        // Player two selection
        if (player == GlobalState.Player.two)
        {
            _mapBubbles[_player2Choice].Deselect(GlobalState.Player.two);
            _hasChosenPlayer2 = false;

            if (direction == GlobalState.Direction.up)
            {
                _player2Choice = Mathf.Clamp(_player2Choice - _columCount, 0, _mapCount - 1);
            }
            else if (direction == GlobalState.Direction.down)
            {
                _player2Choice = Mathf.Clamp(_player2Choice + _columCount, 0, _mapCount - 1);
            }
            else if (direction == GlobalState.Direction.left)
            {
                _player2Choice = Mathf.Clamp(_player2Choice - 1, 0, _mapCount - 1);
            }
            else if (direction == GlobalState.Direction.right)
            {
                _player2Choice = Mathf.Clamp(_player2Choice + 1, 0, _mapCount - 1);
            }
            _mapBubbles[_player2Choice].SelectByPlayer(GlobalState.Player.two);
        }

        // Change the map preview
        if (_player1Choice != _player2Choice)
        {
            _mapPreview.sprite = _mapPreviews[_mapCount];
        }
        else
        {
            _mapPreview.sprite = _mapPreviews[_player1Choice];
        }
    }
}
