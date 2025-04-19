using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public Transform playerHead; // Asigna el headBone del jugador desde Inspector

    public float fireInterval = 3f;
    public float projectileSpeed = 10f;
    public float projectileDamage = 10f;

    private float timer = 0f;

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

        // Pasar el da�o (pensado para proyectiles personalizables)
        EnemyProjectile ep = projectile.GetComponent<EnemyProjectile>();
        if (ep != null)
        {
            ep.damage = projectileDamage;
        }
    }
}
