using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventController : MonoBehaviour
{
    [SerializeField] private float _operationDelay = 0;
    

    // Used in map and character choosing
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Used for DEBUG back button
    public void PrevScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    // Load a scene with given index
    public void LoadGivenScene(int sceneBuildIdxToLoad = -1)
    {
        if (sceneBuildIdxToLoad == -1) return;
        
        SceneManager.LoadScene(sceneBuildIdxToLoad);
    }

    // Quits the game
    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    // DEBUG print to test functionality of event
    public void DebugPrint()
    {
        Debug.Log("Pressed button " + name);

        CharacterLoader.CreateFreshCharacter("Testing1");

    }

    /*private IEnumerator DelayCoroutine(UnityEvent eventToCall)
    {
        yield return new WaitForSeconds(_operationDelay);
        eventToCall.Invoke();
    }*/
}
