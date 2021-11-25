using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Patrol : StateMachineBehaviour
{
    
    EnemyAI aI;
    
    Rigidbody2D rb;
    Vector2[] patrolPoints;
    int currentPatrolPoint;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        patrolPoints = new Vector2[0];
        currentPatrolPoint = 0;
        aI = animator.GetComponent<EnemyAI>();
        rb = animator.GetComponent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        aI.LookAtTarget();
        if (Vector2.Distance(aI.GetTarget().position, aI.transform.position) > aI.GetEnemy().SightRange)
        {
            //If we have patrol points, foollow them, else recalculate patrol points
            if (patrolPoints.Length > 0 && patrolPoints[0] != Vector2.zero && patrolPoints[1] != Vector2.zero)
            {
                if (Vector2.Distance(aI.transform.position, patrolPoints[currentPatrolPoint]) > 2f)
                {
                    aI.MoveTowards(patrolPoints[currentPatrolPoint], aI.GetEnemy().WalkSpeed);
                    
                }
                else
                {
                    currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
                }
            }
            else
            {
                if (aI.IsOnGround())
                {
                    patrolPoints = aI.CalculatePatrolPoints();
                }
            }
        }
        else
        {
            animator.SetBool("isPatrolling", false);
            animator.SetBool("isChasing", true);
        }
    }
    
    
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
