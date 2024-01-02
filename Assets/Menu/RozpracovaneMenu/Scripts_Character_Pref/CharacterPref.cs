using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Default,
    Character
}
public abstract class CharacterPref : ScriptableObject
{
    public GameObject prefab;
    public ItemType type;
    [TextArea(15,20)]
    public string description;
}
