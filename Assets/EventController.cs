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

        //Get the path of the Game data folder
        string m_Path = Application.dataPath;

        //Output the Game data path to the console
        Debug.Log("dataPath : " + m_Path);
        m_Path += "/Characters";
        Debug.Log("dataPath : " + m_Path);
        Directory.CreateDirectory(m_Path);
        Debug.Log("exists : " + Directory.Exists(m_Path));

    }

    /*private IEnumerator DelayCoroutine(UnityEvent eventToCall)
    {
        yield return new WaitForSeconds(_operationDelay);
        eventToCall.Invoke();
    }*/
}
