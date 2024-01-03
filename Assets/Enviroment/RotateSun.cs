using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSun : MonoBehaviour
{
    public float rotationSpeed = 50.0f; // Rychlost rotace v stupních za sekundu

    void Update()
    {
        // Otáčí objekt kolem osy Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
