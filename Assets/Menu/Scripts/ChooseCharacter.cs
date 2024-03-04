using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



[RequireComponent(typeof(GridLayoutGroup))]
public class ChooseCharacter : MonoBehaviour
{
    [SerializeField] private Animator _fadeInOutAnimator;
    [SerializeField] private int _player1Choice = 0;
    [SerializeField] private int _player2Choice = 1;
    
    [SerializeField] private Image _playerPreviewLeft;
    [SerializeField] private Image _playerPreviewRight;

    // TODO Use loader from GlobalState
    [SerializeField] private Sprite[] _characterPreviews;

    private int _columCount;
    private int _characterCount;

    // TODO Dynamically create bubbles based on character data
    private List<ChooseBubble> characterBubbles = new();
    private bool _hasChosenPlayer1, _hasChosenPlayer2;


    // Get all character bubbles and params
    private void Start()
    {
        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
        _columCount = grid.constraintCount;
        _characterCount = transform.childCount;
        //int rowCount = (characterCount + columCount - 1) / columCount;

        // Get all character bubbles
        for(int i = 0; i < _characterCount; i++)
        {
            characterBubbles.Add(transform.GetChild(i).GetComponent<ChooseBubble>());
        }

        // Select default choices
        characterBubbles[_player1Choice].SelectByPlayer(GlobalState.Player.one);
        characterBubbles[_player2Choice].SelectByPlayer(GlobalState.Player.two);
        _playerPreviewLeft.sprite = _characterPreviews[_player1Choice];
        _playerPreviewRight.sprite = _characterPreviews[_player2Choice];
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
            characterBubbles[_player1Choice].GetChosen();
            GlobalState.Player1Character = (GlobalState.Characters)_player1Choice;
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
            characterBubbles[_player2Choice].GetChosen();
            GlobalState.Player2Character = (GlobalState.Characters)_player2Choice;
            _hasChosenPlayer2 = true;
        }

        // Continue to next scene
        if (_hasChosenPlayer1 && _hasChosenPlayer2)
        {
            _fadeInOutAnimator.SetTrigger("StartEndMenu");
        }
    }

    // Moves the player selection of a given player in a given direction 
    private void MovePlayer(GlobalState.Player player, GlobalState.Direction direction)
    {
        // Player one selection
        if (player == GlobalState.Player.one)
        {
            characterBubbles[_player1Choice].Deselect(GlobalState.Player.one);
            _hasChosenPlayer1 = false;

            if (direction == GlobalState.Direction.up)
            {
                _player1Choice = Mathf.Clamp(_player1Choice - _columCount, 0, _characterCount - 1);
            }
            else if(direction == GlobalState.Direction.down)
            {
                _player1Choice = Mathf.Clamp(_player1Choice + _columCount, 0, _characterCount - 1);
            }
            else if(direction == GlobalState.Direction.left)
            {
                _player1Choice = Mathf.Clamp(_player1Choice - 1, 0, _characterCount -1);
            }
            else if (direction == GlobalState.Direction.right)
            {
                _player1Choice = Mathf.Clamp(_player1Choice + 1, 0, _characterCount -1);
            }
            _playerPreviewLeft.sprite = _characterPreviews[_player1Choice];
            characterBubbles[_player1Choice].SelectByPlayer(GlobalState.Player.one);
        }
        
        // Player two selection
        if (player == GlobalState.Player.two)
        {
            characterBubbles[_player2Choice].Deselect(GlobalState.Player.two);
            _hasChosenPlayer2 = false;

            if (direction == GlobalState.Direction.up)
            {
                _player2Choice = Mathf.Clamp(_player2Choice - _columCount, 0, _characterCount - 1);
            }
            else if (direction == GlobalState.Direction.down)
            {
                _player2Choice = Mathf.Clamp(_player2Choice + _columCount, 0, _characterCount - 1);
            }
            else if (direction == GlobalState.Direction.left)
            {
                _player2Choice = Mathf.Clamp(_player2Choice - 1, 0, _characterCount - 1);
            }
            else if (direction == GlobalState.Direction.right)
            {
                _player2Choice = Mathf.Clamp(_player2Choice + 1, 0, _characterCount - 1);
            }
            _playerPreviewRight.sprite = _characterPreviews[_player2Choice];
            characterBubbles[_player2Choice].SelectByPlayer(GlobalState.Player.two);
        }
    }


}
