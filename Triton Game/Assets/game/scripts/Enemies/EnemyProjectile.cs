using UnityEngine;

public enum ProjectileType { Normal, Electric, Corrupt }

public class EnemyProjectile : MonoBehaviour
{
    public ProjectileType projectileType = ProjectileType.Normal;
    public float damage = 10f;
    public float effectDuration = 3f;
    public float speed = 10f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * speed;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 🛡️ Si impacta con un escudo, se destruye
        if (other.CompareTag("Shield"))
        {
            Debug.Log("🛡️ Proyectil bloqueado por escudo");
            Destroy(gameObject);
            return;
        }

        // ☠️ Si impacta con el jugador
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log($"☠️ Proyectil {projectileType} impactó al jugador");

                switch (projectileType)
                {
                    case ProjectileType.Electric:
                        Debug.Log("⚡ Aplicando efecto eléctrico");
                        player.ApplyElectricShock(effectDuration);
                        break;

                    case ProjectileType.Corrupt:
                        Debug.Log("🖤 Aplicando efecto corrupto (tinta)");
                        player.ApplyCorruptionEffect(effectDuration);
                        break;

                    case ProjectileType.Normal:
                        // Solo daño
                        break;
                }
            }

            Destroy(gameObject);
        }
        else if (!other.CompareTag("Enemy"))
        {
            // Impacto con cualquier otro objeto
            Destroy(gameObject);
        }
    }
}
