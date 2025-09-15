using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRetreatState : BaseState
{
    public void EnterState(Enemy enemy)
    {
        enemy.Animator.SetTrigger("RetreatState");
        Debug.Log("Start Retreating");
    }

    public void UpdateState(Enemy enemy)
    {
        if (enemy.PlayerTransform != null)
        {
            enemy.NavMeshAgent.destination = enemy.transform.position - enemy.PlayerTransform.position;
        }
    }
    public void ExitState(Enemy enemy)
    {
        Debug.Log("Stop Retreating");
    }
}
