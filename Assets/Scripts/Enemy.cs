using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private IdleBehaviorType _idleBehavior;
    private AggroBehaviorType _aggroBehavior;

    private Transform _heroTarget;
    private List<Transform> _patrulPoints;
    [SerializeField] private ParticleSystem _deathEffectPrefab;

    private Queue<Vector3> _targetPositions;
    private Vector3 _currentTarget;

    private float _speed = 3f;
    private float _agroDistance = 3f;
    private float _minDistanceToTarget = 0.1f;

    private Vector3 _wanderDirection;

    private float _wanderChangeTime = 1f;
    private float _wanderTimer;

    private void Update()
    {
        Vector3 direction = GetDirectionToHero();

        if (direction.magnitude < _agroDistance)
            HandleAggroBehavior();
        else
            HandleIdleBehavior();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, _agroDistance);
    }

    public void Initialize(IdleBehaviorType idleBehavior, AggroBehaviorType aggroBehavior, List<Transform> patrulPoints, Transform heroTarget)
    {
        _idleBehavior = idleBehavior;
        _aggroBehavior = aggroBehavior;
        _patrulPoints = patrulPoints;
        _heroTarget = heroTarget;
    }

    public void InitializePatrulPoints()
    {
        _targetPositions = new Queue<Vector3>();

        foreach (Transform target in _patrulPoints)
            _targetPositions.Enqueue(target.position);

        _currentTarget = _targetPositions.Dequeue();

        _wanderTimer = _wanderChangeTime;
    }

    private void HandleIdleBehavior()
    {
        switch (_idleBehavior)
        {
            case IdleBehaviorType.StayStill:
                StayStill();
                break;
            case IdleBehaviorType.Patrol:
                Patrol();
                break;
            case IdleBehaviorType.Wander:
                Wander();
                break;
        }
    }

    private void HandleAggroBehavior()
    {
        switch (_aggroBehavior)
        {
            case AggroBehaviorType.RunAway:
                RunAway();
                break;
            case AggroBehaviorType.ChasePlayer:
                ChasePlayer();
                break;
            case AggroBehaviorType.DieFromFear:
                DieFromFear();
                break;
        }
    }

    private void StayStill()
    {

    }

    private void Patrol()
    {
        Vector3 direction = GetDirectionToCurrentTarget();

        float distanceToTarget = direction.magnitude;

        if (distanceToTarget < _minDistanceToTarget)
            SwitchTarget();

        Vector3 normalizeDirection = direction.normalized;

        ProcessMoveTo(normalizeDirection);
    }

    private void Wander()
    {
        _wanderTimer -= Time.deltaTime;

        if (_wanderTimer <= 0f)
        {
            Vector3 randomPoint = GetRandomPatrulPoint();

            _wanderDirection = (randomPoint - transform.position).normalized;

            _wanderTimer = _wanderChangeTime;
        }

        ProcessMoveTo(_wanderDirection);
    }

    private void ChasePlayer()
    {
        Vector3 directionToHero = GetDirectionToHero();

        Vector3 normalizeDirection = directionToHero.normalized;

        ProcessMoveTo(normalizeDirection);
    }

    private void RunAway()
    {
        Vector3 direction = transform.position - _heroTarget.position;
        Vector3 desiredDirection = new Vector3(direction.x, 0, direction.z);

        ProcessMoveTo(desiredDirection.normalized);
    }

    private void DieFromFear()
    {
        Instantiate(_deathEffectPrefab, transform.position, Quaternion.identity, null);
        Destroy(gameObject);
    }

    private void ProcessMoveTo(Vector3 direction)
    {
        transform.Translate(direction * _speed * Time.deltaTime);
    }

    private void SwitchTarget()
    {
        _targetPositions.Enqueue(_currentTarget);

        _currentTarget = _targetPositions.Dequeue();
    }

    private Vector3 GetDirectionToCurrentTarget() => _currentTarget - transform.position;

    private Vector3 GetDirectionToHero() => _heroTarget.position - transform.position;

    private Vector3 GetRandomPatrulPoint() => _patrulPoints[UnityEngine.Random.Range(0, _patrulPoints.Count)].transform.position;
}
