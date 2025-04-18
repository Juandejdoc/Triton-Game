using UnityEngine;
using UnityEngine.XR;

public class WeaponSummoner : MonoBehaviour
{
    [Header("ANCHORS")]
    public Transform shieldAnchor;   // Hijo del hueso de la muñeca izquierda
    public Transform tridentAnchor;  // Hijo del hueso de la muñeca derecha

    [Header("PREFABS")]
    public GameObject shieldPrefab;
    public GameObject tridentPrefab;

    private GameObject currentShield;
    private GameObject currentTrident;

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
                if (currentTrident == null && tridentPrefab != null && tridentAnchor != null)
                {
                    currentTrident = Instantiate(tridentPrefab, tridentAnchor);
                    currentTrident.transform.localPosition = Vector3.zero;
                    currentTrident.transform.localRotation = Quaternion.identity;
                }
            }
            else
            {
                if (currentTrident != null)
                {
                    Destroy(currentTrident);
                    currentTrident = null;
                }
            }
        }
    }
}
