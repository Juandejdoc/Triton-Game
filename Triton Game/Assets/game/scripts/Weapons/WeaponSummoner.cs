using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using TMPro;

public class WeaponSummoner : MonoBehaviour
{
    [Header("ANCHORS")]
    public Transform shieldAnchor;
    public Transform tridentAnchor;

    [Header("PREFABS")]
    public GameObject shieldPrefab;
    public GameObject tridentPrefab;
    public GameObject thrownTridentPrefab;

    [Header("Character Reference")]
    public Transform headBone;

    [Header("Tridente")]
    public float throwForceMultiplier = 1.5f;

    [Header("Munición")]
    public int maxAmmo = 5;
    private int currentAmmo;
    private bool isReloading = false;
    public TextMeshProUGUI ammoText;

    private GameObject currentShield;
    private GameObject currentTrident;
    private GameObject lastThrownTrident;

    private Vector3 lastTridentPosition;
    private Vector3 currentTridentVelocity;

    private PlayerHealth playerHealth;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();

        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    void Update()
    {
        HandleShield();
        HandleTrident();
        HandleReload();
    }

    void FixedUpdate()
    {
        if (lastThrownTrident != null)
        {
            Rigidbody rb = lastThrownTrident.GetComponent<Rigidbody>();
            if (rb != null && rb.linearVelocity.sqrMagnitude > 0.1f)
            {
                lastThrownTrident.transform.rotation = Quaternion.LookRotation(rb.linearVelocity.normalized);
            }
        }
    }

    void HandleShield()
    {
        if (playerHealth != null && playerHealth.isShocked)
            return;

        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        if (leftDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerPressed))
        {
            if (isTriggerPressed)
            {
                if (currentShield == null && shieldPrefab != null && shieldAnchor != null)
                {
                    currentShield = Instantiate(shieldPrefab, shieldAnchor);
                    currentShield.transform.localPosition = Vector3.zero;
                    currentShield.transform.localRotation = Quaternion.identity;
                }
            }
            else
            {
                if (currentShield != null)
                {
                    Destroy(currentShield);
                    currentShield = null;
                }
            }
        }
    }

    void HandleTrident()
    {
        if (playerHealth != null && playerHealth.isShocked)
            return;

        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (rightDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerPressed))
        {
            if (isTriggerPressed)
            {
                if (currentAmmo <= 0 || isReloading)
                    return;

                if (currentTrident == null && tridentPrefab != null && tridentAnchor != null)
                {
                    currentTrident = Instantiate(tridentPrefab, tridentAnchor);
                    currentTrident.transform.localPosition = Vector3.zero;
                    currentTrident.transform.localRotation = Quaternion.identity;
                    lastTridentPosition = tridentAnchor.position;
                }

                currentTridentVelocity = (tridentAnchor.position - lastTridentPosition) / Time.deltaTime;
                lastTridentPosition = tridentAnchor.position;
            }
            else
            {
                if (currentTrident != null && !isReloading)
                {
                    if (currentAmmo <= 0) return;

                    Vector3 launchPosition = tridentAnchor.position;
                    Quaternion launchRotation = tridentAnchor.rotation;
                    Vector3 launchVelocity = currentTridentVelocity;

                    Destroy(currentTrident);
                    currentTrident = null;

                    currentAmmo--;
                    UpdateAmmoUI();

                    if (thrownTridentPrefab != null)
                    {
                        GameObject thrown = Instantiate(thrownTridentPrefab, launchPosition, launchRotation);
                        Rigidbody rb = thrown.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            if (headBone != null)
                            {
                                rb.linearVelocity = headBone.forward * launchVelocity.magnitude * throwForceMultiplier;
                            }
                            else
                            {
                                rb.linearVelocity = launchVelocity * throwForceMultiplier;
                            }

                            lastThrownTrident = thrown;
                        }

                        thrown.AddComponent<TridentDamageHandler>();
                    }
                }
            }
        }
    }

    void HandleReload()
    {
        if (playerHealth != null && playerHealth.isShocked)
            return;

        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (!isReloading && rightDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool isGripPressed) && isGripPressed)
        {
            StartCoroutine(ReloadTridents());
        }
    }

    IEnumerator ReloadTridents()
    {
        isReloading = true;
        if (ammoText != null)
            ammoText.text = "Recargando...";

        yield return new WaitForSeconds(2f);

        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = $"Tridentes: {currentAmmo} / {maxAmmo}";
        }
    }

    public class TridentDamageHandler : MonoBehaviour
    {
        public float damage = 10f;
        public float destroyAfterTime = 10f;
        private bool hasHit = false;

        void Start()
        {
            Destroy(gameObject, destroyAfterTime);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (hasHit) return;

            if (collision.gameObject.CompareTag("Shield"))
            {
                Destroy(gameObject);
            }
            else if (collision.gameObject.CompareTag("Enemy"))
            {
                EnemyAction enemy = collision.gameObject.GetComponent<EnemyAction>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage, transform.position);
                }

                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                transform.SetParent(collision.transform);
                hasHit = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
