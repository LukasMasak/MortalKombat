using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState
{
    static public List<CharacterData> AllCharacters = new List<CharacterData>();
    static public Characters Player1Character;
    static public Characters Player2Character;

    static public Maps Map;

    // TODO to be obsolete
    public enum Characters
    {
        Ruďoch,
        Tuan,
        Žaba,
        Týpek
    }

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
}
