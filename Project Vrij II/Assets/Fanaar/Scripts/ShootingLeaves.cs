using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingLeaves : MonoBehaviour
{
    public GameObject objectPrefab;  // Prefab of the object to be instantiated
    public int numberOfObjects = 5;  // Number of objects in the train
    public float objectSpacing = 1f; // Spacing between each object in the train
    public float shootSpeed = 10f;   // Speed at which the train moves towards the player
    public float idleTimeThreshold = 5f;  // Time threshold for triggering boss fight
    public Transform player;         // Reference to the player's transform

    private List<GameObject> objectPool;  // Object pool to store instantiated objects
    private bool isWaitingForBossFight = false;  // Flag to track if waiting for boss fight
    private Coroutine bossFightCoroutine;  // Coroutine for boss fight timer

    private void Start()
    {
        objectPool = new List<GameObject>(); // Initialize the object pool

        StartIdleTimer();
    }

    private void Update()
    {
        // Check if the player is moving
        if (player.position != transform.position)
        {
            ResetIdleTimer();
        }
    }

    private void StartIdleTimer()
    {
        // Start the idle timer coroutine
        bossFightCoroutine = StartCoroutine(BossFightTimer());
    }

    private void ResetIdleTimer()
    {
        // Reset the idle timer
        if (isWaitingForBossFight)
        {
            StopCoroutine(bossFightCoroutine);
            isWaitingForBossFight = false;
        }

        StartIdleTimer();
    }

    private IEnumerator BossFightTimer()
    {
        // Wait for the idle time threshold
        yield return new WaitForSeconds(idleTimeThreshold);

        // Trigger the boss fight
        StartBossFight();
    }

    public void StartBossFight()
    {
        // Calculate the direction from the boss to the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Calculate the initial spawn position
        Vector3 spawnPosition = transform.position + direction * (objectSpacing * (numberOfObjects - 1) / 2f);

        // Shoot the train of objects from the object pool
        for (int i = 0; i < numberOfObjects; i++)
        {
            GameObject obj = GetPooledObject();

            if (obj != null)
            {
                // Activate the object and set its position
                obj.SetActive(true);
                obj.transform.position = spawnPosition;

                Rigidbody rb = obj.GetComponent<Rigidbody>();

                // Set the velocity of the object to move towards the player
                rb.velocity = direction * shootSpeed;
            }

            // Update the spawn position for the next object
            spawnPosition -= direction * objectSpacing;
        }
    }

    private GameObject GetPooledObject()
    {
        // Find a deactivated object in the object pool and return it
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                return objectPool[i];
            }
        }

        // If no deactivated object is found, instantiate a new one and add it to the object pool
        GameObject obj = Instantiate(objectPrefab);
        obj.SetActive(false);
        objectPool.Add(obj);

        return obj;
    }
}

