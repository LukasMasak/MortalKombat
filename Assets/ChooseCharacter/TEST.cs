using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        CharacterData data = CharacterLoader.LoadFromFile("Testing1");
        //anim.
        //_testAnimatorOverride.animationClips[0] = data.idleAnim;

        Debug.Log(data.idleAnim);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
