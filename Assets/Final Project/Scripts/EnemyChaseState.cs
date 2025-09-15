using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : BaseState
{
    
    public void EnterState(Enemy enemy)
    {
        enemy.Animator.SetTrigger("ChaseState");
        Debug.Log("Start Chasing");
    }

    public void UpdateState(Enemy enemy)
    {
        if (enemy.PlayerTransform != null)
        {
            enemy.NavMeshAgent.destination = enemy.PlayerTransform.transform.position;
            if (Vector3.Distance(enemy.transform.position, enemy.PlayerTransform.transform.position) > enemy.ChaseDistance)
            {
                enemy.SwitchState(enemy.PatrolState);
            }
        }
    }
    public void ExitState(Enemy enemy)
    {
        Debug.Log("Stop Chasing");
    }
}
