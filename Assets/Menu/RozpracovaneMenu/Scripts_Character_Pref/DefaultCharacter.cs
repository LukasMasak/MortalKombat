using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Character", menuName = "Choose Character/Characters/DefaultCharacter")]

public class DefaultCharacter : CharacterPref
{
    [Space]
    public int Health = 100;
    public float movementForce = 1f;
    public float jumpForce = 5f;

    private void Awake()
    {
        type = ItemType.Character;
    }
}
