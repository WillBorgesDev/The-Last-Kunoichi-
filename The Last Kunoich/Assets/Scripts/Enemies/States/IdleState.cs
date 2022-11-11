using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
  protected D_IdleState stateData;
  protected bool flipAfterIdle;
  protected bool isAdleTimeOver;
  protected float idleTime;
  protected bool maxMoving;
  public IdleState(Entity entity, FinalStateMachine stateMachine, string animBoolName, D_IdleState stateData) : base(entity, stateMachine, animBoolName)
  {
    this.stateData = stateData;
  }

  public override void Enter()
  {
    base.Enter();

    maxMoving = entity.MaxMoving();
    entity.SetVelocity(0f);
    isAdleTimeOver = false;
    SetRandomIdleTime();
  }

  public override void Exit()
  {
    base.Exit();

    if (flipAfterIdle)
    {
      entity.Flip();
    }
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();

    if (Time.time >= startTime + idleTime)
    {
      isAdleTimeOver = true;
    }
  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();
  }

  public void SetFlipAfterIdle(bool flip)
  {
    flipAfterIdle = flip;
  }

  private void SetRandomIdleTime()
  {
    idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
  }
}
