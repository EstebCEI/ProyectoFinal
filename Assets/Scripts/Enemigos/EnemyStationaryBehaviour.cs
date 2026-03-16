using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    public Transform player;

    public float viewDistance = 10f;
    public float viewAngle = 90f;

    public float alertTime = 3f;
    public float alertRadius = 10f;

    private float alertTimer;

    private Animator animator;

    enum State
    {
        Normal,
        Alert,
        Danger
    }

    State currentState = State.Normal;

    bool seesPlayer = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckVision();
        UpdateState();
    }

    void CheckVision()
    {
        if (player == null) return;

        Vector3 dir = player.position - transform.position;

        float distance = dir.magnitude;

        bool playerVisible = false;

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
                        playerVisible = true;
                    }
                }
            }
        }

        if (playerVisible)
        {
            seesPlayer = true;
        }
        else
        {
            seesPlayer = false;
        }
    }

    void UpdateState()
    {
        switch (currentState)
        {
            case State.Normal:

                if (seesPlayer)
                {
                    EnterDanger();
                }

                break;

            case State.Alert:

                alertTimer -= Time.deltaTime;

                if (seesPlayer)
                {
                    EnterDanger();
                }

                if (alertTimer <= 0)
                {
                    EnterNormal();
                }

                break;

            case State.Danger:

                if (!seesPlayer)
                {
                    EnterAlert();
                }

                break;
        }
    }

    void EnterNormal()
    {
        currentState = State.Normal;

        Debug.Log(name + " estado NORMAL");

        animator.SetInteger("EnemyState", 0);
    }

    void EnterAlert()
    {
        currentState = State.Alert;

        alertTimer = alertTime;

        Debug.Log(name + " estado ALERTA");

        animator.SetInteger("EnemyState", 1);
    }

    void EnterDanger()
    {
        currentState = State.Danger;

        Debug.Log(name + " estado PELIGRO");

        animator.SetInteger("EnemyState", 2);

        AlertNearbyEnemies();
    }

    void AlertNearbyEnemies()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, alertRadius);

        foreach (Collider col in enemies)
        {
            EnemyAI other = col.GetComponent<EnemyAI>();

            if (other != null && other != this)
            {
                other.ReceiveAlert();
            }
        }
    }

    public void ReceiveAlert()
    {
        if (currentState == State.Normal)
        {
            EnterAlert();
        }
    }

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