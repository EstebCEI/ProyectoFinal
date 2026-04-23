// EnemyPatrolAdvanced.cs

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyPatrolAdvanced : MonoBehaviour, IEnemySaveable
{
    public string enemyID;

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
    [SerializeField]private AudioClip shootSound;

    private Vector3 lastKnownPosition;
    private bool seesPlayer;

    public bool isDead = false;

    enum State { Patrol, Alert, Aggro, Search }
    State state = State.Patrol;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        health = maxHealth;
        GoToNextPoint();
    }

    public EnemyData GetData()
    {
        return new EnemyData(
            enemyID,
            transform.position,
            health,
            isDead
        );
    }

    public void LoadData(EnemyData data)
    {
        transform.position = data.position;
        health = data.health;

        if (data.isDead || health <= 0f)
        {
            isDead = false;
            Die();
        }
        else
        {
            isDead = false;
        }
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

        if (agent != null && agent.enabled)
        {
            agent.ResetPath();
            agent.enabled = false;
        }

        if (animator != null)
        {
            animator.SetBool("IsAggro", false);
            animator.SetBool("isShooting", false);
            animator.CrossFade("Death", 0.05f);
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        RagdollController rag = GetComponent<RagdollController>();
        if (rag != null)
            rag.EnableRagdoll();
    }

    bool IsAgentValid()
    {
        return agent != null &&
               agent.isActiveAndEnabled &&
               agent.isOnNavMesh;
    }

    void GoToNextPoint()
    {
        if (!IsAgentValid()) return;
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[currentPoint].position);
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }

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
        if (state == State.Aggro && !seesPlayer) state = State.Search;
        if (state == State.Search && seesPlayer) state = State.Aggro;
    }

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
        if (agent.hasPath && agent.remainingDistance < 0.5f)
            GoToNextPoint();
    }

    void Combat()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= shootDistance)
        {
            agent.ResetPath();
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

        animator.SetBool("isShooting", true);

        if (playerHealth != null)
            playerHealth.TakeDamage(damage);

        if (shootSound != null)
            AudioSource.PlayClipAtPoint(shootSound, transform.position);
        Invoke(nameof(StopShoot), 0.2f);

    }

    void StopShoot()
    {
        if (animator != null)
            animator.SetBool("isShooting", false);
    }

    void SafeSetDestination(Vector3 pos)
    {
        if (IsAgentValid())
            agent.SetDestination(pos);
    }

    void HandleAnimations()
    {
        if (animator == null || agent == null) return;

        animator.SetInteger("Speed", agent.hasPath ? 1 : 0);
        animator.SetBool("IsAggro", state == State.Aggro);
    }
}