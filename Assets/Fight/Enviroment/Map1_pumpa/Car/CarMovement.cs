using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    [SerializeField] private Vector3 _bumpVector = new Vector3(5,10,0);
    [SerializeField] private bool _movesRight = false;
    [SerializeField] private int _damageDone = 10;


    void Start()
    {
        // Reset the rotation
        transform.localRotation = Quaternion.AngleAxis(0, Vector3.up);
    }

    // Moves left or right based on the direction
    void Update()
    {
        if (_movesRight) transform.position += transform.right * _speed * Time.deltaTime;
        else transform.position += -transform.right * _speed * Time.deltaTime;
    }

    // Utility method for the spawner to set the right way the car should be moving
    public void SetGoesRight(bool doesMoveRight)
    {
        _movesRight = doesMoveRight;
        if (_movesRight)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // Deals damage to playes and bumps them
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            other.GetComponent<Health>().TakeDamage(_damageDone);
            if (_movesRight)
            {
                other.GetComponent<Rigidbody>().velocity = _bumpVector;
            }
            else
            {
                other.GetComponent<Rigidbody>().velocity = _bumpVector * new Vector2(-1, 1);
            }
        }
    }
}