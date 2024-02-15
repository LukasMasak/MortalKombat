using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDestroyWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("name: " + col.name + ", tag: " + col.tag); 
        if (col.CompareTag("Car"))
        {
            Destroy(col.gameObject);
        }
    }

}
