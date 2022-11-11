using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_IdleState : IdleState
{
  private Enemy1 enemy;

  public E1_IdleState(Entity entity, FinalStateMachine stateMachine, string animBoolName, D_IdleState stateData, Enemy1 enemy) : base(entity, stateMachine, animBoolName, stateData)
  {
    this.enemy = enemy;
  }

  public override void Enter()
  {
    base.Enter();
  }

  public override void Exit()
  {
    base.Exit();
    enemy.idleState.SetFlipAfterIdle(true);
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();


    if (isAdleTimeOver)
    {
      stateMachine.ChangeState(enemy.moveState);
    }
  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();
  }
}
