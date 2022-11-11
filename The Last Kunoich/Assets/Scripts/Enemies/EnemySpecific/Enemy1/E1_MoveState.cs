using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_MoveState : MoveState
{
  private Enemy1 enemy;
  public E1_MoveState(Entity entity, FinalStateMachine stateMachine, string animBoolName, D_MoveState stateData, Enemy1 enemy) : base(entity, stateMachine, animBoolName, stateData)
  {
    this.enemy = enemy;
  }

  public override void Enter()
  {
    base.Enter();
    entity.SetVelocity(3f);
    // Debug.Log("Entrei Move State");
  }

  public override void Exit()
  {
    base.Exit();
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();

    Debug.Log("maxMoving");
    Debug.Log(maxMoving);

    if (isDetectingWall || !isDetectingLedge || maxMoving)
    {
      // enemy.idleState.SetFlipAfterIdle(true);
      stateMachine.ChangeState(enemy.idleState);
    }
  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();
  }
}
