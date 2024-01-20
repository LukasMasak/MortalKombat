using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMap : MonoBehaviour
{
    public GameObject[] _MapPrefs;

    private void Awake()
    {
        var Map = Instantiate(_MapPrefs[(int)GlobalState.Map]);
        Map.transform.position = Vector3.zero;
    }
}
