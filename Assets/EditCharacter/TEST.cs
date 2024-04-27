using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public bool callStart = false;

    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        CharacterData data = CharacterLoader.LoadFromFile("Testing1");

        AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;

        overrideController["EmptyClip"] = data.idleAnim;
        //Debug.Log(data.idleAnim.);
    }

    // Update is called once per frame
    void Update()
    {
        if (callStart)
        {
            callStart = false;
            Start();
        }
    }
}
