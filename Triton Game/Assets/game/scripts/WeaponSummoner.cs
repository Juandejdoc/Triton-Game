using UnityEngine;
using UnityEngine.XR;

public class WeaponSummoner : MonoBehaviour
{
    [Header("ANCHORS")]
    public Transform shieldAnchor;
    public Transform tridentAnchor;

    [Header("PREFABS")]
    public GameObject shieldPrefab;
    public GameObject tridentPrefab;         // sostenido
    public GameObject thrownTridentPrefab;   // lanzado

    private GameObject currentShield;
    private GameObject currentTrident;

    private Vector3 lastTridentPosition;
    private Vector3 currentTridentVelocity;

    void Update()
    {
        HandleShield();
        HandleTrident();
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
                // Presionando el gatillo: mostrar el tridente
                if (currentTrident == null && tridentPrefab != null && tridentAnchor != null)
                {
                    currentTrident = Instantiate(tridentPrefab, tridentAnchor);
                    currentTrident.transform.localPosition = Vector3.zero;
                    currentTrident.transform.localRotation = Quaternion.identity;
                    lastTridentPosition = tridentAnchor.position;
                }

                // Guardar velocidad (gesto de lanzamiento)
                currentTridentVelocity = (tridentAnchor.position - lastTridentPosition) / Time.deltaTime;
                lastTridentPosition = tridentAnchor.position;
            }
            else
            {
                // Al soltar el gatillo: lanzar el tridente
                if (currentTrident != null)
                {
                    // Guardar posición y dirección antes de destruir
                    Vector3 launchPosition = currentTrident.transform.position;
                    Quaternion launchRotation = currentTrident.transform.rotation;
                    Vector3 launchVelocity = currentTridentVelocity;

                    Destroy(currentTrident);
                    currentTrident = null;

                    if (thrownTridentPrefab != null)
                    {
                        GameObject thrown = Instantiate(thrownTridentPrefab, launchPosition, launchRotation);
                        Rigidbody rb = thrown.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.linearVelocity = launchVelocity;
                        }
                    }
                }
            }
        }
    }
}
