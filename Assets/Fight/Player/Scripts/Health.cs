using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    int currentHealth;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    private Rigidbody rb;
    private Vector3 velocity;

    private bool BlockingBool = false;

    public PlayerController playerMovement;

    
    private void Start()
    {
        playerMovement = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        slider.value = currentHealth;
        fill.color = gradient.Evaluate(1f);
    }

    public void IsBlocking()
    {
        CheckReferences();
        BlockingBool = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
    }
    public void DeactivateBlocking()
    {
        CheckReferences();
        BlockingBool = false;
        playerMovement.isAttacking = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
    }

    public void TakeDamage(int damage)
    {
        if (!BlockingBool && currentHealth > 0)
        {
            currentHealth -= damage;
            slider.value = currentHealth;
            fill.color = gradient.Evaluate(slider.normalizedValue);

            animator.SetTrigger("Hurt");
            playerMovement.isAttacking = false;
            playerMovement.isBlocking = false;

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void CheckReferences()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (playerMovement == null) playerMovement = GetComponent<PlayerController>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    public void Die()
    {
        animator.SetTrigger("Death");
        this.enabled = false;
        this.GetComponent<PlayerController>().enabled = false;
        // TODO FightManager.Instance.ShowWinUI(_whichPlayer);
    }
    
}
