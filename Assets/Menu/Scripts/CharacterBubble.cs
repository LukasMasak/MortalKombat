using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterBubble : MonoBehaviour
{
    [SerializeField] private Image _borderImage;
    [SerializeField] private Sprite _borderUnSelectedSprite;
    [SerializeField] private Sprite _borderPlayer1Sprite;
    [SerializeField] private Sprite _borderPlayer2Sprite;
    [SerializeField] private Sprite _borderBothSprite;
    [SerializeField] private Sprite _borderChosenSprite;

    private bool _playerOneSelected;
    private bool _playerTwoSelected;
    private bool _isChosen;

    // Set Default select
    private void Start()
    {
        SelectByPlayer(GlobalState.Player.none);
    }

    // Updates the state of the bubble and changes the border
    public void SelectByPlayer(GlobalState.Player player)
    {
        // Set State based on player info in parameter
        switch (player)
        {
            case GlobalState.Player.one:
                _playerOneSelected = true;
                break;
            case GlobalState.Player.two:
                _playerTwoSelected = true;
                break;

            default:
                _playerOneSelected = false;
                _playerTwoSelected = false;
                break;
        }

        UpdateBorderBasedOnState();
    }

    // Updates the state of the bubble and changes the border
    public void Deselect(GlobalState.Player player)
    {
        _isChosen = false;
        
        if(player == GlobalState.Player.one)
        {
            _playerOneSelected = false;
        }
        else if (player == GlobalState.Player.two)
        {
            _playerTwoSelected = false;
        }

        UpdateBorderBasedOnState();
    }

    // Set the bubble as chosen by a player
    public void GetChosen()
    {
        _isChosen = true;
        UpdateBorderBasedOnState();
    }

    // Stop the bubble from being chosen
    public void GetUnChosen()
    {
        _isChosen = false;
        UpdateBorderBasedOnState();
    }

    // Changes the border sprite based on state
    private void UpdateBorderBasedOnState()
    {
        // Bubble is chosen by a player
        if (_isChosen)
        {
            _borderImage.sprite = _borderChosenSprite;
        }
        // Both players on bubble
        else if (_playerOneSelected && _playerTwoSelected)
        {
            _borderImage.sprite = _borderBothSprite;
        }
        // Left player on bubble
        else if (_playerOneSelected)
        {
            _borderImage.sprite = _borderPlayer1Sprite;
        }
        // Right player on bubble
        else if (_playerTwoSelected)
        {
            _borderImage.sprite = _borderPlayer2Sprite;
        }
        // No player on bubble
        else
        {
            _borderImage.sprite = _borderUnSelectedSprite;
        }
    }
}
