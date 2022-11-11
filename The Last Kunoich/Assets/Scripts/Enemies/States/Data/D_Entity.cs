using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
  public float wallCheckDistance = 1.2f;
  public float ledgeCheckDistance = 1.4f;
  public float maxXGo = 1f;
  public float minXGo = -1f;
  public LayerMask whatIsGround;
}
