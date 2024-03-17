using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationEventController : MonoBehaviour
{
    [Tooltip("Used only if LoadGivenScene() is called.")]
    [SerilizableField] private int sceneBuildIdxToLoad = -1;

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
    public void LoadGivenScene()
    {
        if (sceneBuildIdxToLoad == -1) return;
        SceneManager.LoadScene(sceneBuildIdxToLoad);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        //Application.Quit();
    }

    public void DebugPrint()
    {
        Debug.Log("Pressed button " + name);
    }
}
