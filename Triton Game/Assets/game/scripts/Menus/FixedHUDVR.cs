using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class FixedHUDVR : MonoBehaviour
{
    public float distanceFromCamera = 1.5f;
    public int sortingOrder = 9999;

    private void Start()
    {
        // Coloca el canvas como hijo de la cámara VR
        Transform cameraTransform = Camera.main.transform;
        transform.SetParent(cameraTransform);
        transform.localPosition = new Vector3(0f, 0f, distanceFromCamera);
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one * 0.0025f;

        // Forzar overrideSorting
        var canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.overrideSorting = true;
        canvas.sortingOrder = sortingOrder;

        // Opcional: asegurarte que tiene el layer "UI"
        gameObject.layer = LayerMask.NameToLayer("UI");
    }
}
