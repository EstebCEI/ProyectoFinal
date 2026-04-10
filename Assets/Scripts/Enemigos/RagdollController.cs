using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class RagdollController : MonoBehaviour
{
    private Rigidbody[] rigidbodies;
    private Collider[] colliders;

    private Animator animator;
    private NavMeshAgent agent;
    private MonoBehaviour enemyAI; // tu script EnemyAI

    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyAI = GetComponent<MonoBehaviour>(); // o EnemyAI si quieres directo

        DisableRagdoll();
    }

    public void EnableRagdoll()
    {
        // DESACTIVAR IA
        if (enemyAI != null) enemyAI.enabled = false;

        // DESACTIVAR NAVMESH
        if (agent != null)
        {
            agent.enabled = false;
        }

        // DESACTIVAR ANIMATOR
        if (animator != null)
            animator.enabled = false;

        // ACTIVAR FÍSICA
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
        }

        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }
    }

    public void DisableRagdoll()
    {
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
        }

        foreach (Collider col in colliders)
        {
            if (col.gameObject != gameObject)
                col.enabled = false;
        }

        if (animator != null)
            animator.enabled = true;
    }
    void DebugDamage()
    {
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard.lKey.wasPressedThisFrame)
        {
            EnableRagdoll();
        }
    }
    private void Update()
    {
        DebugDamage();
    }
}