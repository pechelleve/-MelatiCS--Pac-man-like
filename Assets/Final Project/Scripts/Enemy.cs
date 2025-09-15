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

    [SerializeField] public List<Transform> _waypoints = new List<Transform>();
    [SerializeField] public float ChaseDistance;
    [SerializeField] private PlayerPowerUpHandler _playerPowerUpHandler;
    public Transform PlayerTransform { get; private set; }

    [HideInInspector] public NavMeshAgent NavMeshAgent;
    public Animator Animator;

    private void Awake()
    {
        _currentState = PatrolState;
        _currentState.EnterState(this);
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();

        if (_playerPowerUpHandler != null)
        {
            PlayerTransform = _playerPowerUpHandler.transform;
        }
    }

    private void Start()
    {
        if (_playerPowerUpHandler != null)
        {
            _playerPowerUpHandler.OnPowerUpStart += StartRetreating;
            _playerPowerUpHandler.OnPowerUpStop += StopRetreating;
        }
        else
        {
            Debug.LogError("PlayerPowerUpHandler not assigned on " + gameObject.name + "!", this);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks and errors
        if (_playerPowerUpHandler != null)
        {
            _playerPowerUpHandler.OnPowerUpStart -= StartRetreating;
            _playerPowerUpHandler.OnPowerUpStop -= StopRetreating;
        }
    }

    private void Update()
    {
        if(_currentState != null)
        {
            _currentState.UpdateState(this);
        }
    }

    public void Dead()
    {
        Destroy(gameObject);
    }

    public void SwitchState(BaseState state)
    { 
        _currentState.ExitState(this);
        _currentState = state;
        _currentState.EnterState(this);
    }

    private void StartRetreating()
    {
        SwitchState(RetreatState);
    }

    private void StopRetreating()
    { 
        SwitchState(PatrolState);
    }

}
