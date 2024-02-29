using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCar : MonoBehaviour
{
    [SerializeField] private GameObject[] _carsToSpawn;
    [SerializeField] private float _speed;
    [SerializeField] private float _respawnTime;
    [SerializeField] private Transform _spawnRightTransform;
    [SerializeField] private Transform _spawnLeftTransform;


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
            spawnPosition = _spawnRightTransform.position;
            isGoingRight = false;
        }
        else
        {
            spawnPosition = _spawnLeftTransform.position;
            isGoingRight = true;
        }

        // Get random car prefab and instantiate it
        int randomIndex = Random.Range(0, _carsToSpawn.Length);
        GameObject carPrefab = _carsToSpawn[randomIndex];
        GameObject spawnedCar = Instantiate(carPrefab, spawnPosition, carPrefab.transform.rotation);

        // set direction and parent
        spawnedCar.GetComponent<CarMovement>().SetGoesRight(isGoingRight);
        spawnedCar.transform.parent = transform;

        // Wait for next spawn
        yield return new WaitForSeconds(_respawnTime);
        StartCoroutine(SpawnCars());
    }
}
