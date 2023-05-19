using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHealth : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null && playerHealth.currentHealth < playerHealth.startingHealth)
            {
                int healthToAdd = 5;
                int healthDifference = playerHealth.startingHealth - playerHealth.currentHealth;
                if (healthToAdd > healthDifference)
                {
                    healthToAdd = healthDifference;
                }
                playerHealth.currentHealth += healthToAdd;
                if (playerHealth.currentHealth > playerHealth.startingHealth)
                {
                    playerHealth.currentHealth = playerHealth.startingHealth;
                }
                playerHealth.UpdateHealthUI();
            }
        }
    }
}

