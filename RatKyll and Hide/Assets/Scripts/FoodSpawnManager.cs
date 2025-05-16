using UnityEngine;
using System.Collections.Generic;

public class FoodSpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<Transform> spawnPoints;        // Invisible GameObjects defining positions
    public List<GameObject> foodPrefabs;       // Food prefabs to randomly choose from

    private Dictionary<Transform, GameObject> activeFood = new Dictionary<Transform, GameObject>();

    void Start()
    {
        int spawnCount = spawnPoints.Count / 2;
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnFoodAtRandomFreePoint();
        }
    }

    public void SpawnFoodAtRandomFreePoint()
    {
        // Gets list of all avaliable points
        List<Transform> availablePoints = spawnPoints.FindAll(p => !activeFood.ContainsKey(p));
        if (availablePoints.Count == 0) return;

        // Chooses a random point and a random prefab
        Transform chosenPoint = availablePoints[Random.Range(0, availablePoints.Count)];
        GameObject prefab = foodPrefabs[Random.Range(0, foodPrefabs.Count)];


        GameObject spawned = Instantiate(prefab, chosenPoint.position, Quaternion.identity);
        spawned.transform.localScale *= 2f;
        activeFood[chosenPoint] = spawned;


        if (spawned.TryGetComponent<IPickUpItem>(out var foodItem))
        {
            foodItem.SpawnPoint = chosenPoint;
        }


        // Let the food know who its manager is
        // if (spawned.TryGetComponent<FoodItem>(out var foodItem))
        // {
        //     foodItem.SetManager(this, chosenPoint);
        // }

    }

    public void NotifyFoodDestroyed(Transform spawnPoint)
    {
        if (activeFood.ContainsKey(spawnPoint))
        {
            activeFood.Remove(spawnPoint);
            SpawnFoodAtRandomFreePoint();
        }
    }
}
