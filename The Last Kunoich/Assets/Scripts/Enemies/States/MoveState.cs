using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
  protected D_MoveState stateData;
  protected bool isDetectingWall;
  protected bool isDetectingLedge;
  protected bool maxMoving;

  public MoveState(Entity entity, FinalStateMachine stateMachine, string animBoolName, D_MoveState stateData) : base(entity, stateMachine, animBoolName)
  {
    this.stateData = stateData;
  }

  public override void Enter()
  {
    base.Enter();
    Debug.Log("stateData.movementSpeed");
    Debug.Log("stateData.movementSpeed");
    entity.SetVelocity(3f);

    isDetectingLedge = entity.CheckLedge();
    isDetectingWall = entity.CheckWall();
    maxMoving = entity.MaxMoving();

  }

  public override void Exit()
  {
    base.Exit();
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();
  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();

    isDetectingLedge = entity.CheckLedge();
    isDetectingWall = entity.CheckWall();
    maxMoving = entity.MaxMoving();
  }
}
