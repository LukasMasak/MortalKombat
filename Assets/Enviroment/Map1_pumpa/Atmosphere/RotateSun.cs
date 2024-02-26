using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSun : MonoBehaviour
{
    // Rychlost rotace v stupních za sekundu
    public float _rotationSpeed = 50.0f; 

    void Update()
    {
        // Otáčí objekt kolem osy Y
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }
}
