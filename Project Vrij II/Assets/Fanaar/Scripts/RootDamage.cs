using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootDamage : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Animator animator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerHealth.TakeDamage(5);
            animator.SetBool("RootHit", true);
        }
    }

    public void ResetRootHitBool()
    {
        animator.SetBool("RootHit", false);
    }
}
