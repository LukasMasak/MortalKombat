using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    int currentHealth;
    private bool OffSetTakeDamage = false;
    public Slider slider;
    public Gradient gradient;
    public Image fill;


    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        slider.value = currentHealth;
        fill.color = gradient.Evaluate(1f);
    }
    public void TakeDamage(int damage)
    {
        if (!OffSetTakeDamage && PlayerMovement.isBlocking)
        {
            Debug.Log(PlayerMovement.isBlocking);
            
            OffSetTakeDamage = true;
            currentHealth -= damage;
            slider.value = currentHealth;
            fill.color = gradient.Evaluate(slider.normalizedValue);

            animator.SetTrigger("Hurt");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        else if (!OffSetTakeDamage && !PlayerMovement.isBlocking)
        {
            animator.SetTrigger("Hurt");

            Debug.Log("SSSSSSSSSSS");
            Debug.Log(PlayerMovement.isBlocking);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        StartCoroutine(OffSetTakeDamageTime());
    }

    public void Die()
    {
        animator.SetTrigger("Death");
        this.enabled = false;
        this.GetComponent<PlayerMovement>().enabled = false;
    }
    private IEnumerator OffSetTakeDamageTime()
    {
        yield return new WaitForSeconds(0.50f);
        OffSetTakeDamage = false;
    }
}
