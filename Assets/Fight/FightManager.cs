using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightManager : MonoBehaviour
{
    // Singleton for easy access
    public static FightManager Instance { get; private set; }

    // UI world objects used to show which player won
    [SerializeField] private GameObject _winPlayerOneUI;
    [SerializeField] private GameObject _winPlayerTwoUI;
    [SerializeField] private GameObject _diePlayerBothUI;

    // Reset button that shows up
    [SerializeField] private GameObject _reset;

    private bool _isPlayerOneDead = false;
    private bool _isPlayerTwoDead = false;


    // Singleton setup
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Changes the visibility of the win UI
    public void ShowWinUI(GlobalState.Player playerThatDied)
    {
        if (playerThatDied == GlobalState.Player.one) _isPlayerOneDead = true;
        if (playerThatDied == GlobalState.Player.two) _isPlayerTwoDead = true;

        // Both players died -> show lose UI
        if (_isPlayerOneDead && _isPlayerTwoDead)
        {
            _winPlayerOneUI.SetActive(false);
            _winPlayerTwoUI.SetActive(false);
            _diePlayerBothUI.SetActive(true);
            _reset.SetActive(true);
            return;
        }

        // Players two died -> show player one win UI
        if(_isPlayerTwoDead)
        {
            _winPlayerOneUI.SetActive(true);
            _reset.SetActive(true);
            return;
        }
        
        // Players one died -> show player two win UI
        if (_isPlayerOneDead)
        {
            _winPlayerTwoUI.SetActive(true);
            _reset.SetActive(true);
            return;
        }

    }

    // Used to for the Back button
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
