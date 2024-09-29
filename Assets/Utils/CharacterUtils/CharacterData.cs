using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData
{
    public bool isValid;

    // --------------Character-Stats---------------------------
    public string name;
    public float speed;
    public float jump;
    public int health;
    public int damage;
    public Vector2 attackPointOffset;
    public uint attackFrameIdx;
    public float attackSize;
    public float colliderWidth;
    public float colliderHeight;

    public Vector2 colliderOffset;


    // --------------Character-Sprites/Animations--------------
    public Sprite preview;
    public Texture2D previewNormalMap;
    public Sprite bubbleIcon;
    public FajtovAnimationClip idleAnim;
    public FajtovAnimationClip attackAnim;
    public FajtovAnimationClip walkAnim;
    public FajtovAnimationClip blockAnim;
    public FajtovAnimationClip jumpAnim;
    public FajtovAnimationClip deathAnim;
    public FajtovAnimationClip hurtAnim;
}
