using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWall : MonoBehaviour
{
    private void Update()
    {
        
    }

    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("name: " + col.collider.name + ", tag: " + col.collider.tag); 
        if (col.collider.CompareTag("Car"))
        {
            Destroy(col.collider.gameObject);
        }
    }

}
