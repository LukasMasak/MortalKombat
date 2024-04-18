using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Character
{
    private const string CHARACTER_FOLDER = "";
    private string _name;
    // Preview              Sprite
    // Icon                 Sprite
    // Attack               Animation
    // Move                 Animation
    // Block                Animation
    // Jump                 Animation
    // Idle                 Animation
    // Dead                 Animation
    // Hurt                 Animation
    
    // Speed                float
    // HP                   float
    // Attack point offset  Vector2
    // Attack frame idx     uint
    // Attack size          float

    private Character(string name)
    {
        _name = name;
    }

    public static Character LoadFromFile(string path)
    {


        return null;
    }
}
