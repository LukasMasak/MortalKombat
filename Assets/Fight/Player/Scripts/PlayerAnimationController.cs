using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    public void InitializeAnimations(ref CharacterData characterData)
    {
        if (_animator == null) Start();
        if (_spriteRenderer == null) Start();

        AnimatorOverrideController overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = overrideController;

        overrideController["Idle"] = characterData.idleAnim;
        overrideController["Walk"] = characterData.walkAnim;
        overrideController["Attack"] = characterData.attackAnim;
        overrideController["Block"] = characterData.blockAnim;
        overrideController["Jump"] = characterData.jumpAnim;
        overrideController["Hurt"] = characterData.hurtAnim;
        overrideController["Death"] = characterData.deathAnim;

    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
    }
}
