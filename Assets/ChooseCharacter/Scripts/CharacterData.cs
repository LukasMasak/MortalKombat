using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterData
{
    // --------------Character-Stats---------------------------
    public string name;
    public float speed;
    public float jump;
    public uint health;
    public uint damage;
    public Vector2 attackPointOffset;
    public uint attackFrameIdx;
    public float attackSize;

    // --------------Character-Sprites/Animations--------------
    public Sprite preview;
    public Sprite bubbleIcon;
    public Animation attackAnim;
    public Animation moveAnim;
    public Animation blockAnim;
    public Animation jumpAnim;
    public Animation idleAnim;
    public Animation deathAnim;
    public Animation hurtAnim;
}
