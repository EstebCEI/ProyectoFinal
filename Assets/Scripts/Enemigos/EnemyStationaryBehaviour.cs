using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;

    private Animator animator;
    private NavMeshAgent agent;

    [Header("Visión")]
    public float viewDistance = 10f;
    public float viewAngle = 90f;

    [Header("Estados")]
    public float alertTime = 3f;
    public float searchTime = 5f;
    public float alertRadius = 10f;

    private float alertTimer;
    private float searchTimer;

    [Header("Movimiento")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3.5f;
    public float rotationSpeed = 5f;

    private Vector3 lastKnownPosition;

    enum State
    {
        Normal,
        Alert,
        Danger,
        Search
    }

    State currentState = State.Normal;

    bool seesPlayer = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        agent.speed = patrolSpeed;
    }

    void Update()
    {
        CheckVision();
        UpdateState();
        HandleMovement();
    }

    // ------------------ VISIÓN ------------------

    void CheckVision()
    {
        if (player == null) return;

        Vector3 dir = player.position - transform.position;
        float distance = dir.magnitude;

        seesPlayer = false;

        if (distance <= viewDistance)
        {
            float angle = Vector3.Angle(transform.forward, dir);

            if (angle <= viewAngle / 2f)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position + Vector3.up, dir.normalized, out hit, viewDistance))
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

    // ------------------ ESTADOS ------------------

    void UpdateState()
    {
        switch (currentState)
        {
            case State.Normal:

                if (seesPlayer)
                    EnterDanger();

                break;

            case State.Alert:

                alertTimer -= Time.deltaTime;

                if (seesPlayer)
                    EnterDanger();

                if (alertTimer <= 0)
                    EnterSearch();

                break;

            case State.Danger:

                if (!seesPlayer)
                    EnterSearch();

                break;

            case State.Search:

                searchTimer -= Time.deltaTime;

                if (seesPlayer)
                    EnterDanger();

                if (searchTimer <= 0)
                    EnterNormal();

                break;
        }
    }

    // ------------------ MOVIMIENTO ------------------

    void HandleMovement()
    {
        switch (currentState)
        {
            case State.Normal:
                agent.speed = patrolSpeed;
                agent.ResetPath();
                break;

            case State.Alert:
                agent.speed = patrolSpeed;
                agent.SetDestination(lastKnownPosition);
                break;

            case State.Danger:
                agent.speed = chaseSpeed;

                // Persecución suave (no perfecta)
                Vector3 predictedPosition = player.position + player.forward * 1.5f;
                agent.SetDestination(predictedPosition);

                RotateTowards(player.position);
                break;

            case State.Search:
                agent.speed = patrolSpeed;
                agent.SetDestination(lastKnownPosition);
                break;
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        direction.y = 0;

        if (direction == Vector3.zero) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    // ------------------ TRANSICIONES ------------------

    void EnterNormal()
    {
        currentState = State.Normal;

        Debug.Log(name + " NORMAL");

        animator.SetInteger("EnemyState", 0);
    }

    void EnterAlert()
    {
        currentState = State.Alert;

        alertTimer = alertTime;

        Debug.Log(name + " ALERTA");

        animator.SetInteger("EnemyState", 1);
    }

    void EnterDanger()
    {
        currentState = State.Danger;

        Debug.Log(name + " PELIGRO");

        animator.SetInteger("EnemyState", 2);

        AlertNearbyEnemies();
    }

    void EnterSearch()
    {
        currentState = State.Search;

        searchTimer = searchTime;

        Debug.Log(name + " BUSCANDO");

        animator.SetInteger("EnemyState", 3);
    }

    // ------------------ ALERTA EN GRUPO ------------------

    void AlertNearbyEnemies()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, alertRadius);

        foreach (Collider col in enemies)
        {
            EnemyAI other = col.GetComponent<EnemyAI>();

            if (other != null && other != this)
            {
                other.ReceiveAlert(lastKnownPosition);
            }
        }
    }

    public void ReceiveAlert(Vector3 position)
    {
        if (currentState == State.Normal)
        {
            lastKnownPosition = position;
            EnterAlert();
        }
    }

    // ------------------ DEBUG ------------------

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alertRadius);

        Vector3 left = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;

        Gizmos.DrawLine(transform.position, transform.position + left * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + right * viewDistance);
    }
}