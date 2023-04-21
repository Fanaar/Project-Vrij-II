using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{
    public GameObject firePoint;
    public List<GameObject> vfx = new List<GameObject> ();

    private GameObject effectToSpawn;
    void Start()
    {
        effectToSpawn = vfx[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
