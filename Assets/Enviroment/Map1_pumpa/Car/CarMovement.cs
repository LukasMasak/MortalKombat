using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private Vector3 bumpVector = new Vector3(5,10,0);
    [SerializeField] private bool movesRight = false;
    [SerializeField] private int damageDone = 10;


    void Start()
    {
        // Reset the rotation
        transform.localRotation = Quaternion.AngleAxis(0, Vector3.up);
    }

    // Moves left or right based on the direction
    void Update()
    {
        if (movesRight) transform.position += transform.right * speed * Time.deltaTime;
        else transform.position += -transform.right * speed * Time.deltaTime;
    }

    // Utility method for the spawner to set the right way the car should be moving
    public void SetGoesRight(bool doesMoveRight)
    {
        movesRight = doesMoveRight;
        if (movesRight)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // Deals damage to playes and bumps them
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            other.GetComponent<Health>().TakeDamage(damageDone);
            if (movesRight)
            {
                other.GetComponent<Rigidbody>().velocity = bumpVector;
            }
        }
    }
}