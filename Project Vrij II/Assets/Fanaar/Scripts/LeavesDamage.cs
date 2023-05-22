using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavesDamage : MonoBehaviour
{
    public PlayerHealth playerHealth;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Leaf")
        {
            playerHealth.TakeDamage(2);

        }
    }
}
