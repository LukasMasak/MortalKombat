using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char : MonoBehaviour
{
    public CharacterMenu menu;
    public CharacterPref _char;
    public CharacterPref _char2;

    public void AddCharacter()
    {
        menu.AddCharacter(_char,1);
    }
    public void AddCharacter2()
    {
        menu.AddCharacter(_char2, 1);
    }
}
