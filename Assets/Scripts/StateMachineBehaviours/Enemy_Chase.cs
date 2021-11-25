using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//State when chasing the target
public class Enemy_Chase : StateMachineBehaviour
{
    //Attached AI
    EnemyAI aI;

    Rigidbody2D rb;

    bool isClimbing;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        aI = animator.GetComponent<EnemyAI>();
        rb = animator.GetComponent<Rigidbody2D>();
        isClimbing = false;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        aI.LookAtTarget();
        if(isClimbing)
        {
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 8;
        }
        
        //distance between gameobject and target
        float distance = Vector2.Distance(aI.GetTarget().position, aI.transform.position);
        if(distance < aI.GetEnemy().AttackRange)
        {
            aI.Attack();
        }
        else if(distance < aI.GetEnemy().SightRange)
        {
            if(aI.path.Length > 0)
            {
               
                Navlink currentNavlink = aI.path[0];

                if(currentNavlink.IsJumpLink && (aI.IsOnGround()||isClimbing))
                {
                    //if we're on a jump navlink, jump
                    isClimbing = false;
                    aI.Jump(CustomUtilities.CalculateJumpSpeed(currentNavlink.Start.Transform.position, currentNavlink.End.Transform.position, aI.GetEnemy().JumpForce));
                }
                else
                {
                    //If we're on a ladder, climb
                    if(currentNavlink.Start.Type == NavpointType.Ladder)
                    {
                        isClimbing = true;
                        aI.ClimbTo(currentNavlink.End.Transform.position,aI.GetEnemy().WalkSpeed);
                    }
                    else
                    {
                        //move towards the next pathpoint
                        if(aI.IsOnGround()||isClimbing)
                        {
                            isClimbing = false;
                            
                            aI.MoveTowards(currentNavlink.End.Transform.position, aI.GetEnemy().RunSpeed);
                        }
                        
                    }     
                }
            }
            
        
        }
        else
        {
            isClimbing = false;
            rb.gravityScale = 8;
            animator.SetBool("isPatrolling",true);
            animator.SetBool("isChasing", false);
            
        }
        
    }
    
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    

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
