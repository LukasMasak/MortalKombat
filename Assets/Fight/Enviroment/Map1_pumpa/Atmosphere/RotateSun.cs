using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSun : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 2.0f; 

    void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }
}
