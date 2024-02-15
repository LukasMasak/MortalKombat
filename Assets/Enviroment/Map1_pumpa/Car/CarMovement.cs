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

    void Update()
    {
        if (movesRight) transform.position += transform.right * speed * Time.deltaTime;
        else transform.position += -transform.right * speed * Time.deltaTime;
    }

    public void SetGoesRight(bool doesMoveRight)
    {
        movesRight = doesMoveRight;
        if (movesRight)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with " + other.name + " tag " + other.tag);
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            Debug.Log("Collided with player " + other.name + " tag " + other.tag);
            other.GetComponent<Health>().TakeDamage(damageDone);
            if (movesRight)
            {
                other.GetComponent<Rigidbody>().velocity = bumpVector;
            }
        }
    }

}