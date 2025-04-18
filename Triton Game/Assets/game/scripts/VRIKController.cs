using UnityEngine;

[RequireComponent(typeof(Animator))]
public class VRIKController : MonoBehaviour
{
    [Header("Objetivos de las manos (hijos de los controles VR)")]
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    [Header("Rotación de corrección")]
    public Quaternion leftHandRotationOffset = Quaternion.Euler(0, 0, 90);   // ajusta si es necesario
    public Quaternion rightHandRotationOffset = Quaternion.Euler(0, 0, -90); // ajusta si es necesario

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;

        // Mano izquierda
        if (leftHandTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation * leftHandRotationOffset);
        }

        // Mano derecha
        if (rightHandTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation * rightHandRotationOffset);
        }
    }
}