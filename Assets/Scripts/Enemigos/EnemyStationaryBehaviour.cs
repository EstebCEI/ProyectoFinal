using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyGuard : MonoBehaviour, IEnemySaveable
{
    public string enemyID;

    public Transform player;
    public PlayerHealth playerHealth;
    public Transform guardCenter;

    private NavMeshAgent agent;
    private Animator animator;

    public float maxHealth = 100f;
    private float health;

    public float guardRadius = 10f;

    public float viewDistance = 12f;
    public float viewAngle = 90f;

    public float shootDistance = 8f;

    public float damage = 10f;
    public float fireRate = 1.2f;
    private float nextFireTime;

    private Vector3 lastKnownPosition;
    private bool seesPlayer;

    [SerializeField] AudioClip shootSound;

    public bool isDead = false;

    enum State { Idle, Aggro, Search }
    State state = State.Idle;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        health = maxHealth;
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
        isDead = data.isDead;

        if (isDead)
            Die();
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

        agent.ResetPath();
        agent.enabled = false;

        animator.CrossFade("Death", 0.1f);

        GetComponent<Collider>().enabled = false;
    }

    bool IsAgentValid()
    {
        return agent != null &&
               agent.isActiveAndEnabled &&
               agent.isOnNavMesh;
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
        if (state == State.Idle && seesPlayer) state = State.Aggro;
        if (state == State.Aggro && !seesPlayer) state = State.Search;
        if (state == State.Search && seesPlayer) state = State.Aggro;
    }

    void HandleMovement()
    {
        switch (state)
        {
            case State.Idle:
                StayInZone();
                break;

            case State.Aggro:
                Combat();
                break;

            case State.Search:
                SafeSetDestination(lastKnownPosition);
                break;
        }
    }

    void StayInZone()
    {
        if (Vector3.Distance(transform.position, guardCenter.position) > guardRadius)
            SafeSetDestination(guardCenter.position);
        else
            agent.ResetPath();
    }

    void Combat()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= shootDistance)
        {
            agent.ResetPath();
            TryShoot();
            if (shootSound != null)
                AudioSource.PlayClipAtPoint(shootSound, transform.position, 1f);
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
        playerHealth.TakeDamage(damage);

        Invoke(nameof(StopShoot), 0.2f);
    }

    void StopShoot()
    {
        animator.SetBool("isShooting", false);
    }

    void SafeSetDestination(Vector3 pos)
    {
        if (IsAgentValid())
            agent.SetDestination(pos);
    }

    void HandleAnimations()
    {
        animator.SetInteger("Speed", agent.hasPath ? 1 : 0);
        animator.SetBool("IsAggro", state == State.Aggro);
    }
}