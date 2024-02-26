using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMap : MonoBehaviour
{
    public GameObject[] _mapPrefabs;

    // Spawn the right map based on the GlobalState of the game
    private void Awake()
    {
        var Map = Instantiate(_mapPrefabs[(int)GlobalState.Map]);
        Map.transform.position = Vector3.zero;
    }
}
