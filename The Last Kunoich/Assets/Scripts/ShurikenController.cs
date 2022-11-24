using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenController : MonoBehaviour
{
    Rigidbody2D body;
    public float speed = 20; 
    public float _damage = 15;
    public int side = 0;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
       
        if(side == 1)
        {
            speed = speed * side;
        } else
        {
            speed = speed * side;
        }
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = new UnityEngine.Vector2(speed, body.velocity.y);
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
            Destroy(this.gameObject);
          }
          break;
        case "RangedEnemy":
          if (archer != null)
          {
            archer.TakeDamage(_damage);
            Destroy(this.gameObject);
          }
          break;
          default:
          break;
      }

    }
}
