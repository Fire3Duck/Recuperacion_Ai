using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    private NavMeshAgent _enemyAgent;
    public enum EnemyState
    {
        Patrolling,
        Chasing,
        Attacking
    }

    public EnemyState currentState;
    Transform _player;
    [SerializeField] private Transform[] _patrolPoints;
    public int indexPatrolling;
    [SerializeField] private float _detectionRange = 4;
    [SerializeField] private float _attackRange = 2;
    void Awake()
    {
        _enemyAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").transform;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = EnemyState.Patrolling;
        SetRandomPatrolPoint();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
            Patrol();
            break;

            case EnemyState.Chasing:
            Chase();
            break;

            case EnemyState.Attacking:
            Attack();
            break;
        }
    }

    void Patrol()
    {
        if(OnRange(_detectionRange))
        {
            currentState = EnemyState.Chasing;
        }

        if(_enemyAgent.remainingDistance < 0.5f)
        {
            SetRandomPatrolPoint();
        }

    }

    void Chase()
    {
        if(!OnRange(_detectionRange))
        {
            currentState = EnemyState.Patrolling;
        }

        if(OnRange(_attackRange))
        {
            currentState = EnemyState.Attacking;
            attackTimer = attackDelay;
        }
        _enemyAgent.SetDestination(_player.position);
    }

    float attackDelay;
    float attackTimer;


    void Attack()
    {
        if(OnRange(_attackRange))
        {
            currentState = EnemyState.Chasing;
        }

        if(attackTimer < attackDelay)
        {
            attackTimer += Time.deltaTime;

            return;
        }

        Debug.Log("Attack");

        attackTimer = 0;
    }

    void SetRandomPatrolPoint()
    {
        _enemyAgent.SetDestination(_patrolPoints[Random.Range(0, _patrolPoints.Length)].position);

    }

    bool OnRange(float distance)
    {
        if(Vector3.Distance(transform.position, _player.position) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
