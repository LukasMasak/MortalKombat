using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState
{
    
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

    // TODO implement
    public static List<string> GetCharacterList()
    {
        return new List<string>();
    }
}
