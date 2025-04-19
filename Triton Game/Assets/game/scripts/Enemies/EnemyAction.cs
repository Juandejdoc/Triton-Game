using UnityEngine;

public class EnemyAction : MonoBehaviour
{
    [Header("Salud del enemigo")]
    public float maxHealth = 50f;
    private float currentHealth;

    [Header("Reacción al impacto")]
    public float knockbackForce = 2f;       // Para Rigidbody
    public float fallbackDistance = 0.3f;   // Si no hay Rigidbody

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount, Vector3 attackerPosition)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            Die();
        }
        else
        {
            ApplyKnockback(attackerPosition);
        }
    }

    void ApplyKnockback(Vector3 attackerPosition)
    {
        Vector3 direction = (transform.position - attackerPosition).normalized;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direction * knockbackForce, ForceMode.Impulse);
        }
        else
        {
            transform.position += direction * fallbackDistance;
        }
    }

    void Die()
    {
        Destroy(gameObject); // eliminará también todos los tridentes hijos
    }

}
