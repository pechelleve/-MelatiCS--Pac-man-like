using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private BaseState _currentState;
    public EnemyPatrolState PatrolState = new EnemyPatrolState();
    public EnemyChaseState ChaseState = new EnemyChaseState();
    public EnemyRetreatState RetreatState = new EnemyRetreatState();

    [SerializeField] public List<Waypoints> _waypoints = new List<Waypoints>();

    [HideInInspector] public NavMeshAgent NavMeshAgent;

    private void Awake()
    {
        _currentState = PatrolState;
        _currentState.EnterState(this);
        NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(_currentState != null)
        {
            _currentState.UpdateState(this);
        }
    }
}
