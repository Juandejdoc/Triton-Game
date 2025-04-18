using UnityEngine;

public class VRRigFollower : MonoBehaviour
{
    [Header("VR References")]
    public Transform cameraRig;
    public Transform vrHead;

    [Header("Character Rig Bones")]
    public Transform headBone;

    [Header("Camera Offset (relative to headBone)")]
    public Vector3 cameraOffset = new Vector3(0f, 0.1f, 0.1f);

    void LateUpdate()
    {
        // Alinear CameraRig con la cabeza del personaje
        if (cameraRig != null && headBone != null && vrHead != null)
        {
            Vector3 targetHeadPosition = headBone.position + headBone.TransformDirection(cameraOffset);
            Vector3 headOffset = vrHead.position - cameraRig.position;
            cameraRig.position = targetHeadPosition - headOffset;
        }

        // Rotar cabeza
        if (headBone != null && vrHead != null && vrHead != headBone && !vrHead.IsChildOf(headBone))
        {
            headBone.rotation = vrHead.rotation;
        }

        // 🔇 YA NO movemos leftHandBone ni rightHandBone aquí.
        // Eso lo hace el sistema de IK automáticamente desde VRIKController.cs
    }
}
