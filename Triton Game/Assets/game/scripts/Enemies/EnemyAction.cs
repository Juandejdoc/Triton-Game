using TMPro;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public float knockbackForce = 5f;
    private Rigidbody rb;

    [Header("HUD de Vida")]
    public TextMeshProUGUI healthText;  // <-- Asignar desde el inspector

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        UpdateHealthText();
    }

    public void TakeDamage(float amount, Vector3 attackerPosition)
    {
        currentHealth -= amount;

        if (rb != null)
        {
            Vector3 pushDirection = (transform.position - attackerPosition).normalized;
            rb.AddForce(pushDirection * knockbackForce, ForceMode.Impulse);
        }

        UpdateHealthText();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = Mathf.RoundToInt(currentHealth).ToString();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
