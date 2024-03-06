using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hronák : MonoBehaviour
{
    private Animator animator;
    public GameObject attackPoint;
    public float attackPointSize;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private bool once= false;

    public void StartRandom()
    {
        if (!once)
        {
            StartCoroutine(RandomlyTimedFunction());
        }
        once = true;
        Debug.Log("onec");
    }

    IEnumerator RandomlyTimedFunction()
    {
        while (true)
        {
            yield return new WaitForSeconds(45f);
            AttactStart();
        }
    }
    public void AttactStart()
    {
        animator.SetTrigger("Attack");
        Debug.Log("attack");

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
            if (enemy.tag == "Player")
            {
                enemy.GetComponent<Health>().TakeDamage(300);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackPointSize);
    }
}
