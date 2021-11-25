using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController2D controller;
    private Player player;
    private PlayerGraphics graphs;
    private float horizontalMove = 0f;
    private float verticalMove = 0f;
    private bool jump = false;
    private bool isClimbing = false;
    private bool canMove = true;
    void Start()
    {
        controller = GetComponent<CharacterController2D>();
        controller.OnLandEvent.AddListener(OnLanding);
        player = GetComponent<Player>();
        
        GetComponent<PlayerInteractor>().Interacted += OnInteraction;
        GetComponent<PlayerInteractor>().InteractorLeft += OnInteractorLeft;
        graphs = GetComponent<PlayerGraphics>();
        SetCanMove(true);
    }

    
    void Update()
    {

        if (canMove)
        {
            
            
            horizontalMove = Input.GetAxisRaw("Horizontal") * player.RunSpeed;
            graphs.SetSpeed(Mathf.Abs(horizontalMove));
            if (isClimbing)
            {
                verticalMove = Input.GetAxisRaw("Vertical") * player.ClimbSpeed;
                graphs.SetSpeed(new Vector2(horizontalMove, verticalMove).magnitude);
            }
            else
            {
                verticalMove = 0;
            }

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
                if (controller.IsGrounded())
                {
                    graphs.AnimateJump(true);
                    PlayerAudio.instance.PlayJump();
                }

            }
            if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > 5f && !isClimbing)
            {
                graphs.AnimateJump(true);
            }
        }
       
    }
    public void OnLanding()
    {

        graphs.AnimateJump(false);

    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime, false, jump, isClimbing);
        }
        
        jump = false;
    }
    void OnInteraction(InteractionEventArgs args)
    {
        if(args.InteractionType == InteractionType.ClimbLadder)
        {
            
            isClimbing = true;
            graphs.AnimateClimb(true);
        }
    }
    void OnInteractorLeft(InteractionEventArgs args)
    {
        if(args.InteractionType == InteractionType.ClimbLadder)
        {
            
            isClimbing = false;
            graphs.AnimateClimb(false);
        }
    }
    public void SetCanMove(bool b)
    {
        canMove = b;
        if(b == false)
        {
            controller.Move(0, 0, false, false, false);
            graphs.SetSpeed(0);
        }
        
    }
}
