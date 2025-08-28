using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerAbilityState
{
    private bool isGrounded;
    private int xInput;
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.CheckIfTouchingGround();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;

        xInput = player.InputHandler.NormInputX;

        if (player.isTouchingLadder && player.InputHandler.NormInputY != 0)
        {
            stateMachine.ChangeState(player.ClimbState);
            return;
        }

        if (isGrounded && player.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else
        {
            player.CheckIfShouldFlip(xInput);
            player.SetVelocityX(xInput * playerData.movementVelocity);
        }

        if (isGrounded && player.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else
        {
            player.CheckIfShouldFlip(xInput);
            player.SetVelocityX(xInput * playerData.movementVelocity);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
