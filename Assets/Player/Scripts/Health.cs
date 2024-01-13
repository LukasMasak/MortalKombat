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


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        slider.value = currentHealth;
        fill.color = gradient.Evaluate(1f);
    }

    public void IsBlocking()
    {
        BlockingBool = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
    }
    public void DeactivateBlocking()
    {
        BlockingBool = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
    }
    private void Update()
    {
       // Debug.Log(this.gameObject.name + " is blocking " + BlockingBool);
    }

    public void TakeDamage(int damage)
    {
        if (!BlockingBool)
        {
            //Debug.Log(PlayerMovement.isBlocking);
            
            //OffSetTakeDamage = true;
            currentHealth -= damage;
            slider.value = currentHealth;
            fill.color = gradient.Evaluate(slider.normalizedValue);

            animator.SetTrigger("Hurt");

            if (currentHealth <= 0)
            {
                Die();
            }
        }
        
        else if (BlockingBool)
        {
            animator.SetTrigger("Hurt");

            //Debug.Log(PlayerMovement.isBlocking);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        animator.SetTrigger("Death");
        this.enabled = false;
        this.GetComponent<PlayerMovement>().enabled = false;
    }
    
}
