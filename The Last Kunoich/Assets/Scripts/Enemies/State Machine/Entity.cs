using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
  public FinalStateMachine stateMachine;
  public D_Entity entityData;

  public int facingDirection { get; private set; }
  public Rigidbody2D rb { get; private set; }
  public Animator anim { get; private set; }
  public GameObject aliveGO { get; private set; }

  [SerializeField]
  private LayerMask ledgeCheck;
  BoxCollider2D bc2d;

  private Vector2 velocityWorkspace;
  private Vector2 initialPosition;

  public virtual void Start()
  {
    facingDirection = 1;

    aliveGO = transform.Find("Alive").gameObject;
    rb = aliveGO.GetComponent<Rigidbody2D>();
    anim = aliveGO.GetComponent<Animator>();
    bc2d = aliveGO.GetComponent<BoxCollider2D>();
    initialPosition = aliveGO.transform.position;

    stateMachine = new FinalStateMachine();
  }

  public virtual void Update()
  {
    stateMachine.currentState.LogicUpdate();
  }
  public virtual void FixedUpdate()
  {
    stateMachine.currentState.PhysicsUpdate();
  }

  public virtual void SetVelocity(float velocity)
  {
    // Debug.Log("Seteu");
    // Debug.Log(stateMachine.currentState);
    // Debug.Log(velocity);

    velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
    rb.velocity = velocityWorkspace;
    Debug.Log("rb.velocity");
    Debug.Log(rb.velocity);
  }

  public virtual bool MaxMoving()
  {
    Debug.Log("------------------------------------");
    Debug.Log(rb.position.x);
    Debug.Log(initialPosition.x);
    Debug.Log(initialPosition.x + entityData.maxXGo);
    Debug.Log(initialPosition.x - entityData.minXGo);

    // if (rb.position.x > initialPosition.x + entityData.maxXGo)
    // {
    //   rb.position = initialPosition;
    // }


    // if (rb.position.x < initialPosition.x - entityData.minXGo)
    // {
    //   return true;

    // }

    return false;
  }


  public virtual bool CheckWall()
  {
    return Physics2D.Raycast(bc2d.bounds.center, aliveGO.transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
  }

  public virtual bool CheckLedge()
  {
    return Physics2D.Raycast(bc2d.bounds.center, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround) || Physics2D.Raycast(bc2d.bounds.center, new Vector2(1, -1), entityData.ledgeCheckDistance, entityData.whatIsGround);
  }


  public virtual void Flip()
  {
    facingDirection *= -1;
    velocityWorkspace *= -1;
    aliveGO.transform.Rotate(0f, 180f, 0f);
    // aliveGO.transform.Find("HUD").gameObject.transform.Rotate(0f, 0f, 0f);
  }
}
