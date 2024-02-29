using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDestroyWall : MonoBehaviour
{
    // A wall that destroys car upon trigger
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Car"))
        {
            Destroy(col.gameObject);
        }
    }

}
