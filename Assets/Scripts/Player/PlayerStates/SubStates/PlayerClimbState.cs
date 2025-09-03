using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbState : PlayerState
{
    private int yInput;
    private int xInput;

    public PlayerClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
        : base(player, stateMachine, playerData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        // 进入爬梯子：关闭重力，停止速度
        //player.RB.gravityScale = 0f;
        player.RB.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
        // 恢复默认重力
        //player.RB.gravityScale = player.gravityScale;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        yInput = player.InputHandler.NormInputY;
        xInput = player.InputHandler.NormInputX;

        if (player.InputHandler.JumpInput)
        {
            player.InputHandler.UseJumpInput();
            stateMachine.ChangeState(player.JumpState);
            return;
        }

        // 在梯子上，纵向移动
        player.SetVelocityY(yInput * playerData.climbVelocity);

         player.CheckIfShouldFlip(-xInput);
         player.SetVelocityX(xInput * 0.5f * playerData.movementVelocity); 

        if (!player.isTouchingLadder)
        {
            if (player.CheckIfTouchingGround())
                stateMachine.ChangeState(player.IdleState);
            else
                stateMachine.ChangeState(player.InAirState);
        }
    }
}

