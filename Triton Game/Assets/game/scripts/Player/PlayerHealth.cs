using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de vida")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI")]
    public TextMeshProUGUI healthText;

    [Header("Efectos de estado")]
    public bool isShocked = false;
    public bool isCorrupted = false;

    [Header("Partículas eléctricas")]
    public GameObject leftHandSparkFX;
    public GameObject rightHandSparkFX;

    [Header("Overlay de tinta (proyectil corrupto)")]
    public GameObject inkOverlay;



    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHUD();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHUD();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void UpdateHUD()
    {
        if (healthText != null)
            healthText.text = "Vida: " + Mathf.RoundToInt(currentHealth).ToString();
    }

    void Die()
    {
        isDead = true;
        Debug.Log(" El jugador ha muerto. Cargando escena de Game Over...");

        StartCoroutine(LoadGameOverScene());
    }

    IEnumerator LoadGameOverScene()
    {
        yield return new WaitForSeconds(1.5f); // breve delay para dramatismo o efectos
        SceneManager.LoadScene(1); // Reemplaza con el nombre exacto de tu escena
    }

    // ⚡ Paralizar al jugador con un proyectil eléctrico
    public void ApplyElectricShock(float duration)
    {
        Debug.Log("⚡ Electric shock aplicado"); // <-- Este log

        if (!isShocked)
            StartCoroutine(ElectricShockRoutine(duration));
    }


    IEnumerator ElectricShockRoutine(float duration)
    {
        isShocked = true;

        if (leftHandSparkFX != null)
        {
            leftHandSparkFX.SetActive(true);
            var ps = leftHandSparkFX.GetComponentInChildren<ParticleSystem>();
            if (ps != null) ps.Play(true);
        }

        if (rightHandSparkFX != null)
        {
            rightHandSparkFX.SetActive(true);
            var ps = rightHandSparkFX.GetComponentInChildren<ParticleSystem>();
            if (ps != null) ps.Play(true);
        }

        yield return new WaitForSeconds(duration);

        isShocked = false;

        if (leftHandSparkFX != null) leftHandSparkFX.SetActive(false);
        if (rightHandSparkFX != null) rightHandSparkFX.SetActive(false);
    }



    public void ApplyCorruptionEffect(float duration)
    {
        if (!isCorrupted)
            StartCoroutine(CorruptionRoutine(duration));
    }

    IEnumerator CorruptionRoutine(float duration)
    {
        isCorrupted = true;
        Debug.Log("Visión distorsionada");

        if (inkOverlay != null) inkOverlay.SetActive(true);

        yield return new WaitForSeconds(duration);

        if (inkOverlay != null) inkOverlay.SetActive(false);

        isCorrupted = false;
        Debug.Log("Visión restaurada");
    }



}
