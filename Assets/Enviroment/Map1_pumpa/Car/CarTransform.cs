using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTransform : MonoBehaviour
{
    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.AngleAxis(0, Vector3.up);
        transform.position += -transform.right * speed * Time.deltaTime;

    }

}