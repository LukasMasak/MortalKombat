using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventController : MonoBehaviour
{
    [SerializeField] private float _operationDelay = 0;
    
    // Used in map and character choosing
    public void NextScene()
    {
        // Scene over the max valid index
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            Debug.LogWarning("Tried to load scene of index" + SceneManager.sceneCount + ", will not do anything!");
            return;
        }

        StartCoroutine(DelayFunction(() => 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }));
    }

    // Used for DEBUG back button
    public void PrevScene()
    {
        // Scene under the valid indicies
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Debug.LogWarning("Tried to load scene of index -1, will not do anything!");
            return;
        }

        StartCoroutine(DelayFunction(() => 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }));
    }

    // Load a scene with given index
    public void LoadGivenScene(int sceneBuildIdxToLoad = -1)
    {
        // Scene under the valid indicies
        if (sceneBuildIdxToLoad < 0)
        {
            Debug.LogWarning("Tried to load scene of index -1, will not do anything!");
            return;
        }

        // Scene over the max valid index
        if (sceneBuildIdxToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("Tried to load scene of index" + SceneManager.sceneCount + ", will not do anything!");
            return;
        }

        StartCoroutine(DelayFunction(() => 
        {
            SceneManager.LoadScene(sceneBuildIdxToLoad);
        }));
    }

    // Quits the game
    public void QuitGame()
    {
        Debug.Log("Quitting game");

        StartCoroutine(DelayFunction(() => 
        {
            Application.Quit();
        }));
    }

    // DEBUG print to test functionality of event
    public void DebugPrint()
    {
        Debug.Log("Pressed button " + name + " before delay");

        StartCoroutine(DelayFunction(() => 
        {
            Debug.Log("Pressed button " + name + " after delay");
        }));
        
    }

    private IEnumerator DelayFunction(Action function)
    {
        yield return new WaitForSeconds(_operationDelay);
        function();
    }
}
