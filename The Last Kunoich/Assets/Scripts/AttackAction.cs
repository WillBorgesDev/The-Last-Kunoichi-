using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : MonoBehaviour
{
    private BoxCollider2D colliderATK;
    public float _damage = 10;
    void Start()
    {
        colliderATK = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.targetSide == 1)
        {
            colliderATK.offset = new Vector2(0.13f, 0.02f);
        } if (PlayerController.targetSide == -1)
        {
            colliderATK.offset = new Vector2(-0.13f, 0.02f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
      var enemy = other.transform.GetComponent<EnemyController>();
      var archer = other.transform.GetComponent<ArcherController>();
      switch (other.gameObject.tag)
      {
        case "Enemy":
          if (enemy != null)
          {
            enemy.TakeDamage(_damage);
          }
          break;
        case "RangedEnemy":
          if (archer != null)
          {
            archer.TakeDamage(_damage);
          }
          break;
          default:
          break;
      }

    }
}
