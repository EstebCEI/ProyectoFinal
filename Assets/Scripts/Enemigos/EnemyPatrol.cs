using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyPatrolAdvanced : MonoBehaviour
{
    public Transform player;
    public PlayerHealth playerHealth;
    public Transform[] patrolPoints;

    private NavMeshAgent agent;
    private Animator animator;

    private int currentPoint;

    [Header("Vida")]
    public float maxHealth = 100f;
    private float health;

    [Header("Visión")]
    public float viewDistance = 12f;
    public float viewAngle = 90f;

    [Header("Disparo")]
    public float damage = 10f;
    public float shootDistance = 10f;
    public float fireRate = 1.5f;
    private float nextFireTime;

    private Vector3 lastKnownPosition;
    private bool seesPlayer;

    public bool isDead = false;

    enum State { Patrol, Alert, Aggro, Search }
    State state = State.Patrol;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        health = maxHealth;

        GoToNextPoint();
    }

    void Update()
    {
        if (isDead) return;
        if (!IsAgentValid()) return;

        CheckVision();
        UpdateState();
        HandleMovement();
        HandleAnimations();
    }

    bool IsAgentValid()
    {
        return agent != null &&
               agent.isActiveAndEnabled &&
               agent.isOnNavMesh;
    }

    // ---------------- VIDA ----------------

    public void TakeDamage(float dmg)
    {
        if (isDead) return;

        health -= dmg;

        if (health <= 0f)
        {
            health = 0f;
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        if (agent != null && agent.isOnNavMesh)
        {
            agent.ResetPath();
            agent.enabled = false;
        }

        if (animator != null)
        {
            animator.SetBool("IsAggro", false);
            animator.SetBool("IsShooting", false);
            animator.CrossFade("Death", 0.1f);
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        RagdollController ragdoll = GetComponent<RagdollController>();
        if (ragdoll != null)
            ragdoll.EnableRagdoll();
    }

    // ---------------- VISIÓN ----------------

    void CheckVision()
    {
        seesPlayer = false;
        if (player == null) return;

        Vector3 dir = player.position - transform.position;
        float dist = dir.magnitude;

        if (dist <= viewDistance)
        {
            float angle = Vector3.Angle(transform.forward, dir);

            if (angle <= viewAngle / 2f)
            {
                if (Physics.Raycast(transform.position + Vector3.up, dir.normalized, out RaycastHit hit, viewDistance))
                {
                    if (hit.transform == player)
                    {
                        seesPlayer = true;
                        lastKnownPosition = player.position;
                    }
                }
            }
        }
    }

    void UpdateState()
    {
        if (state == State.Patrol && seesPlayer) state = State.Aggro;
        if (state == State.Alert && seesPlayer) state = State.Aggro;

        if (state == State.Aggro && !seesPlayer)
            state = State.Search;

        if (state == State.Search && seesPlayer)
            state = State.Aggro;
    }

    // ---------------- MOVIMIENTO ----------------

    void HandleMovement()
    {
        switch (state)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Aggro:
                Combat();
                break;

            case State.Search:
                SafeSetDestination(lastKnownPosition);
                break;
        }
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;
        if (!IsAgentValid()) return;

        if (agent.pathPending) return;
        if (!agent.hasPath) return;

        float dist = agent.remainingDistance;

        if (!float.IsInfinity(dist) && dist < 0.5f)
        {
            GoToNextPoint();
        }
    }

    void GoToNextPoint()
    {
        if (!IsAgentValid()) return;
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[currentPoint].position);
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }

    void Combat()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        RotateTowards(player.position);

        if (dist <= shootDistance)
        {
            SafeResetPath();
            TryShoot();
        }
        else
        {
            SafeSetDestination(player.position);
        }
    }

    void TryShoot()
    {
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireRate;

        if (playerHealth == null) return;

        animator.SetBool("IsShooting", true);

        playerHealth.TakeDamage(damage);

        Invoke(nameof(StopShoot), 0.2f);
    }

    void StopShoot()
    {
        animator.SetBool("IsShooting", false);
    }

    // ---------------- SAFE NAVMESH ----------------

    void SafeSetDestination(Vector3 pos)
    {
        if (!IsAgentValid()) return;
        agent.SetDestination(pos);
    }

    void SafeResetPath()
    {
        if (!IsAgentValid()) return;
        agent.ResetPath();
    }

    // ---------------- ANIMACIONES ----------------

    void HandleAnimations()
    {
        int speed = agent.hasPath ? 1 : 0;
        animator.SetInteger("Speed", speed);
        animator.SetBool("IsAggro", state == State.Aggro);
    }

    void RotateTowards(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        dir.y = 0;

        if (dir == Vector3.zero) return;

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
    }
}