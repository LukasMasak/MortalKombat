using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hronák : MonoBehaviour
{
    private Animator animator;
    public GameObject attackPoint;
    public float attackPointSize;

    public void StartRandom()
    {
        StartCoroutine(RandomlyTimedFunction());
    }

    IEnumerator RandomlyTimedFunction()
    {
        while (true)
        {
            yield return new WaitForSeconds(GetRandomInterval());
            AttactStart();
        }
    }
    public void AttactStart()
    {
        animator.SetTrigger("Attack");

    }
    public void Attacked()
    {
        attackPoint.SetActive(true);
        Collider[] hitEnemy = Physics.OverlapSphere(attackPoint.transform.position, attackPointSize);
        foreach (Collider enemy in hitEnemy)
        {
            if (enemy.gameObject == this)
            {
                continue;
            }
            Debug.Log("hit " + enemy.name);

            enemy.GetComponent<Health>().TakeDamage(30);
        }
    }
    float GetRandomInterval()
    {
        return Random.Range(30f, 50f);
    }
}
