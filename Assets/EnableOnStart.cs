using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EnableOnStart : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectsToEnable;


    // Start is called before the first frame update
    void Start()
    {
        GameObject blocker = GameObject.Find("Blocker");
        if (blocker != null) Destroy(blocker);

        foreach (GameObject obj in _objectsToEnable)
        {
            obj.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        foreach (GameObject obj in _objectsToEnable)
        {
            obj.SetActive(false);
        }

        FindFirstObjectByType<MenuManager>()?.SwitchCharacterDropdown();
    }

}
