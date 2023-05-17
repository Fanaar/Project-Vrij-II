using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;   // The player's starting health
    public int currentHealth;          // The player's current health

    public Slider healthSlider;        // Reference to the UI slider that displays the player's health
    public Image damageImage;          // Reference to the UI image that displays the damage effect

    public float flashSpeed = 5f;      // The speed at which the damage image fades out
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);   // The colour of the damage image

    bool isDead;                       // Whether the player is dead or not
    bool damaged;                      // Whether the player has taken damage or not

    

    void Start()
    {
        currentHealth = startingHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        if (damaged)
        {
            // Fade out the damage image
            damageImage.color = flashColour;
        }
        else
        {
            // Fade in the damage image
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        damaged = false;
    }

    // This function is called whenever the player takes damage
    public void TakeDamage(int amount)
    {
        if (isDead)
            return;

        currentHealth -= amount;
        UpdateHealthUI();

        damaged = true;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        healthSlider.value = currentHealth;
    }

    void Die()
    {
        isDead = true;

        // Disable any components or behaviours that should stop when the player dies
        // (e.g. movement, shooting, etc.)

        // Show game over screen or trigger end of game logic
    }
}
