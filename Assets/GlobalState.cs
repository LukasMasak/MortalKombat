using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState
{
    static public List<CharacterData> AllCharacters = new List<CharacterData>();
    static public CharacterData Player1Character
    {
        get 
        { 
            if (_Player1Character == null || !_Player1Character.isValid) 
            {
                _Player1Character = GlobalState.GetCharacterForDebug();
                Debug.Log("Using P1 as Debug Character");
            }

            return _Player1Character;
        }
        set { _Player1Character = value; }
    }
    static private CharacterData _Player1Character;

    static public CharacterData Player2Character
    {
        get 
        { 
            if (_Player2Character == null ||!_Player2Character.isValid) 
            {
                _Player2Character = GlobalState.GetCharacterForDebug();
                Debug.Log("Using P1 as Debug Character");
            }

            return _Player2Character;
        }
        set { _Player2Character = value; }
    }
    static private CharacterData _Player2Character;


    static public Maps Map;

    // All the maps in game
    public enum Maps
    {
        Pumpa,
        Domek
    }

    // Direction enum for easier movement of player choosing
    public enum Direction
    {
        up,
        left,
        right,
        down
    }

    // Player enum for easier movement of player choosing
    public enum Player
    {
        one,
        two,
        none
    }

    // Returns the index of the character in the AllCharacters list or -1
    public static int GetCharacterIndexFromName(string name)
    {
        for (int i = 0; i < AllCharacters.Count; i++)
        {
            if (AllCharacters[i].name == name) return i;
        }
        return -1;
    }

    public static CharacterData GetCharacterForDebug()
    {
        if (AllCharacters.Count == 0) CharacterLoader.LoadAllCharacters(AllCharacters);
        return AllCharacters[0];
    }
}
