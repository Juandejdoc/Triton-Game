using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public Transform playerHead; // Se asigna automáticamente si está vacío

    public float fireInterval = 3f;
    public float projectileSpeed = 10f;
    public float projectileDamage = 10f;

    private float timer = 0f;

    void Start()
    {
        // Buscar el objeto con tag "Player", que en tu caso es el hueso "Head"
        if (playerHead == null)
        {
            GameObject headObject = GameObject.FindWithTag("Player");
            if (headObject != null)
            {
                playerHead = headObject.transform;
            }
        }
    }

    void Update()
    {
        if (playerHead == null || projectilePrefab == null || shootPoint == null) return;

        timer += Time.deltaTime;
        if (timer >= fireInterval)
        {
            timer = 0f;
            ShootAtPlayer();
        }
    }

    void ShootAtPlayer()
    {
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Vector3 direction = (playerHead.position - shootPoint.position).normalized;

        // Orientar el proyectil hacia el jugador
        projectile.transform.forward = direction;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }

        // Pasar el daño si es un proyectil con lógica
        EnemyProjectile ep = projectile.GetComponent<EnemyProjectile>();
        if (ep != null)
        {
            ep.damage = projectileDamage;
        }
    }
}
