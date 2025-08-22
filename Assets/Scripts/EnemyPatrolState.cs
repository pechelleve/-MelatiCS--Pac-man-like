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
        Debug.Log("Start Patrol");
    }

    public void UpdateState(Enemy enemy)
    {
        if (!_isMoving)
        {
            _isMoving = true;
            int index = UnityEngine.Random.Range(0, enemy._waypoints.Count);
            _destination = enemy._waypoints[index].transform.position;
            enemy.NavMeshAgent.destination = _destination;
            Debug.Log("Patrolling");
        }
        else
        { 
        if (Vector3.Distance(_destination, enemy.transform.position) <= 0.1)
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
