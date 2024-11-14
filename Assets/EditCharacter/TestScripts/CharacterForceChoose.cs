using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CharacterForceChoose : MonoBehaviour
{
    [SerializeField] private string _chosenCharacter1;
    [SerializeField] private string _chosenCharacter2;

    private void Start()
    {
        GlobalState.Player1Character = GlobalState.GetCharacterDataFromName(_chosenCharacter1);
        GlobalState.Player2Character = GlobalState.GetCharacterDataFromName(_chosenCharacter2);

        GetComponent<SpawnPlayers>().Spawn();
    }
}
