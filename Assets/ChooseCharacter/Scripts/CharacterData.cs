using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterData
{
    public bool isValid;

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
    public AnimationClip idleAnim;
    public AnimationClip attackAnim;
    public AnimationClip moveAnim;
    public AnimationClip blockAnim;
    public AnimationClip jumpAnim;
    public AnimationClip deathAnim;
    public AnimationClip hurtAnim;
}
