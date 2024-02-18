using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCar : MonoBehaviour
{
    [SerializeField] private GameObject[] CarsToSpawn;
    [SerializeField] private float speed;
    [SerializeField] private float respawnTime;
    [SerializeField] private Transform spawnRightTransform;
    [SerializeField] private Transform spawnLeftTransform;


    void Start()
    {
        StartCoroutine(SpawnCars());
    }

    // Spawns a random car either left or right and waits respawnTime seconds
    private IEnumerator SpawnCars()
    {
        bool isGoingRight;
        Vector3 spawnPosition;

        // Start going left or right
        if (Random.value < 0.5f)
        {
            spawnPosition = spawnRightTransform.position;
            isGoingRight = false;
        }
        else
        {
            spawnPosition = spawnLeftTransform.position;
            isGoingRight = true;
        }

        // Get random car prefab and instantiate it
        int randomIndex = Random.Range(0, CarsToSpawn.Length);
        GameObject carPrefab = CarsToSpawn[randomIndex];
        GameObject spawnedCar = Instantiate(carPrefab, spawnPosition, carPrefab.transform.rotation);

        // set direction and parent
        spawnedCar.GetComponent<CarMovement>().SetGoesRight(isGoingRight);
        spawnedCar.transform.parent = transform;

        // Wait for next spawn
        yield return new WaitForSeconds(respawnTime);
        StartCoroutine(SpawnCars());
    }
}
