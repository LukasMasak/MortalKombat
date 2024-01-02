using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimator : MonoBehaviour
{
    private InputAction move;
    private InputAction Attack;
    private InputAction Jump;


    private Animator animator;
    private Rigidbody rb;
    private float maxSpeed = 5f;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();

    }
    private void Update()
    {
        float _move = rb.velocity.magnitude / maxSpeed;

        if (_move > 0.1f)
        {
            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
        }
    }
}
