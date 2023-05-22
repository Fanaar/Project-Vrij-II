using UnityEngine;
using System.Collections.Generic;

public class LeafShooter : MonoBehaviour
{
    public GameObject leafPrefab;
    public Transform playerTransform;
    public float idleTimeThreshold = 5f;
    public float shootForce = 10f;
    public int numberOfLeaves = 5;

    private float lastMovementTime;
    private bool isPlayerIdle;
    private List<GameObject> leafPool;

    private void Start()
    {
        lastMovementTime = Time.time;
        InitializeLeafPool();
    }

    private void Update()
    {
        if (playerTransform == null)
            return;

        // Check if player has moved
        if (playerTransform.position != transform.position)
        {
            lastMovementTime = Time.time;
            isPlayerIdle = false;
        }
        else
        {
            float timeSinceLastMovement = Time.time - lastMovementTime;
            if (timeSinceLastMovement >= idleTimeThreshold && !isPlayerIdle)
            {
                isPlayerIdle = true;
                ShootLeaves();
            }
        }
    }

    private void InitializeLeafPool()
    {
        leafPool = new List<GameObject>();
        for (int i = 0; i < numberOfLeaves; i++)
        {
            GameObject leaf = Instantiate(leafPrefab, transform.position, Quaternion.identity);
            leaf.SetActive(false);
            leafPool.Add(leaf);
        }
    }

    private GameObject GetPooledLeaf()
    {
        for (int i = 0; i < leafPool.Count; i++)
        {
            if (!leafPool[i].activeInHierarchy)
                return leafPool[i];
        }

        return null;
    }

    private void ShootLeaves()
    {
        for (int i = 0; i < numberOfLeaves; i++)
        {
            GameObject leaf = GetPooledLeaf();
            if (leaf == null)
                return;

            leaf.transform.position = transform.position;
            leaf.transform.rotation = Quaternion.identity;
            leaf.SetActive(true);

            Rigidbody rb = leaf.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.forward * shootForce, ForceMode.Impulse);
        }
    }
}
