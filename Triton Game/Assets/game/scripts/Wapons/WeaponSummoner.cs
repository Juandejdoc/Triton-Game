using UnityEngine;
using UnityEngine.XR;
using System.Collections;

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

    private GameObject currentShield;
    private GameObject currentTrident;
    private GameObject lastThrownTrident;

    private Vector3 lastTridentPosition;
    private Vector3 currentTridentVelocity;

    void Update()
    {
        HandleShield();
        HandleTrident();
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
        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (rightDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerPressed))
        {
            if (isTriggerPressed)
            {
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
                if (currentTrident != null)
                {
                    Vector3 launchPosition = tridentAnchor.position;
                    Quaternion launchRotation = tridentAnchor.rotation;
                    Vector3 launchVelocity = currentTridentVelocity;

                    Destroy(currentTrident);
                    currentTrident = null;

                    if (thrownTridentPrefab != null)
                    {
                        GameObject thrown = Instantiate(thrownTridentPrefab, launchPosition, launchRotation);
                        Rigidbody rb = thrown.GetComponent<Rigidbody>();
                        Transform forwardPoint = thrown.transform.Find("ForwardDirection");

                        if (rb != null)
                        {
                            if (headBone != null)
                            {
                                rb.linearVelocity = headBone.forward * launchVelocity.magnitude * throwForceMultiplier;
                            }
                            else if (forwardPoint != null)
                            {
                                rb.linearVelocity = forwardPoint.forward * launchVelocity.magnitude * throwForceMultiplier;
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

    // 👇 Script interno: daño + clavado con espera al knockback
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

            EnemyAction enemy = collision.gameObject.GetComponent<EnemyAction>();
            if (enemy != null)
            {
                hasHit = true;
                ContactPoint contact = collision.contacts[0]; // Primer punto de contacto
                StartCoroutine(StickAfterKnockback(enemy, contact));
            }
        }

        IEnumerator StickAfterKnockback(EnemyAction enemy, ContactPoint contact)
        {
            enemy.TakeDamage(damage, transform.position);

            yield return null; // esperar a que el enemigo se desplace por el knockback

            // Desactivar física
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.detectCollisions = false;
            }

            // 1. Colocar el tridente en el punto de impacto
            transform.position = contact.point;

            // 2. Orientarlo perpendicular a la superficie (como una lanza clavada)
            transform.rotation = Quaternion.LookRotation(-contact.normal);

            // 3. Hacerlo hijo del enemigo para que se destruya junto a él
            transform.SetParent(enemy.transform);

            // 4. Desactivar colisión
            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;

            // NO lo destruimos manualmente: ahora el enemigo lo arrastra consigo y se destruye junto con él
        }

    }
}
