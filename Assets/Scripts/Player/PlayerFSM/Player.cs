using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    public PlayerJumpState JumpState { get; private set; }

    public PlayerClimbState ClimbState { get; private set; }

    public PlayerInAirState InAirState { get; private set; }

    [SerializeField]
    private PlayerData playerData;

    #endregion

    #region Components
    public Animator Anim { get; private set; }
    public PlayerInputHander InputHandler { get; private set; }

    public Rigidbody2D RB { get; private set; }

    public BoxCollider2D Collider2D { get; private set; }

    public Collider2D otherCollider;

    public Vector2 CurrentVelocity { get; private set; }
    #endregion

    #region check Transforms
    [SerializeField]
    private Transform groundcheck;
    #endregion

    #region other variables
    public int FacingDirection { get; private set; }

    private Vector2 workspace;

    public bool isTouchingLadder { get; set; }

    public float gravityScale { get; private set; }
    #endregion

    #region Callback Functions
    public void Awake()
    {
        stateMachine = new PlayerStateMachine();

        InputHandler = GetComponent<PlayerInputHander>();

        IdleState = new PlayerIdleState(this, stateMachine,playerData, "idle");

        MoveState = new PlayerMoveState(this, stateMachine, playerData, "move");

        JumpState = new PlayerJumpState(this, stateMachine, playerData, "inair");

        InAirState = new PlayerInAirState(this, stateMachine, playerData, "inair");

        ClimbState = new PlayerClimbState(this, stateMachine, playerData, "climb");
    }
    
    private void Start()
    {
        Anim = GetComponent<Animator>();

        stateMachine.Initialize(IdleState);

        RB = GetComponent<Rigidbody2D>();

        Collider2D = GetComponent<BoxCollider2D>();

        gravityScale = RB.gravityScale;

        FacingDirection = 1;
    }

    private void Update()   
    {
        CurrentVelocity = RB.velocity;
        stateMachine.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }
    #endregion

    #region Set Functions
    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    #endregion

    #region Check Functions
    public bool CheckIfTouchingGround()
    {
        return Physics2D.OverlapCircle(groundcheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isTouchingLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isTouchingLadder = false;
        }
    }

    #endregion

    #region other Functions
    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public void SetControlled(bool controlled)
    {
        if (controlled)
        {
            InputHandler.PlayerInput.enabled = true;   
        }
        else
        {
            InputHandler.PlayerInput.enabled = false;
        }
    }

    #endregion
}
