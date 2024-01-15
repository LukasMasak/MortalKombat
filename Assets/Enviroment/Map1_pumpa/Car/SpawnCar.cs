using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCar : MonoBehaviour
{
    [SerializeField] private GameObject[] SpawnAmounthCar;
    [SerializeField] private Transform EnviromentLayer;

    [SerializeField] private float speed;
    [SerializeField] private float respawnTime;
    [SerializeField] private Transform _spawn;


    void Start()
    {
        StartCoroutine(spawnrate());
    }

    private void SpawnObject()
    {
        Vector3 spawn = new Vector3(_spawn.position.x, _spawn.position.y, _spawn.position.z); 
        int randomIndex = Random.Range(0, SpawnAmounthCar.Length);
        GameObject car = SpawnAmounthCar[randomIndex];
        GameObject spawnedCar = Instantiate(car, spawn, car.transform.rotation);
        spawnedCar.transform.parent = EnviromentLayer;
    }

    IEnumerator spawnrate()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);
            SpawnObject();
        }
    }
}
