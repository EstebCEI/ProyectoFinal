// NO USADO

using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    private Rigidbody[] rigidbodies;
    private Collider[] colliders;

    private Animator animator;
    private CharacterController characterController;

    void Awake()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        DisableRagdoll();
    }

    public void EnableRagdoll()
    {
        Debug.Log("Activando ragdoll");

        // Animator OFF
        if (animator != null)
            animator.enabled = false;

        // CharacterController OFF
        if (characterController != null)
            characterController.enabled = false;

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }
    }


    public void DisableRagdoll()
    {
        // Activar Animator
        if (animator != null)
            animator.enabled = true;

        // Activar CharacterController
        if (characterController != null)
            characterController.enabled = true;

        // Desactivar físicas en huesos
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
        }

        foreach (Collider col in colliders)
        {
            // IMPORTANTE: desactiva colliders de huesos, pero no el CharacterController
            if (!(col is CharacterController))
                col.enabled = false;
        }
    }
}