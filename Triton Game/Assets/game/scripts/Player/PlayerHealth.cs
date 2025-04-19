using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Parámetros de salud")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Texto de UI")]
    public TextMeshProUGUI healthText; // Asignar en el Inspector

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        UpdateHealthUI();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Vida: " + Mathf.Clamp(currentHealth, 0f, maxHealth).ToString("F0");
        }
    }

    void Die()
    {
        Debug.Log("Jugador ha muerto.");
        // Aquí puedes añadir animación, reinicio de escena, etc.
    }
}
