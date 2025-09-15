using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class EnemyPatrolState : BaseState
{
    private bool _isMoving;
    private Vector3 _destination;
    public void EnterState(Enemy enemy)
    {
        _isMoving = false;
        enemy.Animator.SetTrigger("PatrolState");
    }

    public void UpdateState(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, enemy.PlayerTransform.transform.position) < enemy.ChaseDistance)
        {
            enemy.SwitchState(enemy.ChaseState);
        }
        if (!_isMoving)
        {
            _isMoving = true;
            int index = UnityEngine.Random.Range(0, enemy._waypoints.Count);
            _destination = enemy._waypoints[index].transform.position;
            enemy.NavMeshAgent.destination = _destination;
        }
        else
        { 
        if (!enemy.NavMeshAgent.pathPending && enemy.NavMeshAgent.remainingDistance <= enemy.NavMeshAgent.stoppingDistance)
           { 
             _isMoving = false;
           }
        }
    }
    
    public void ExitState(Enemy enemy)
    {
        Debug.Log("Stop Patrol");
    }
}
