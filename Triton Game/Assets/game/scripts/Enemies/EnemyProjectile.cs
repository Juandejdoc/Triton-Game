using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 10f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Shield"))
        {
            // Bloqueado por escudo
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        else
        {
            // Impacto genérico
            Destroy(gameObject);
        }
    }
}
