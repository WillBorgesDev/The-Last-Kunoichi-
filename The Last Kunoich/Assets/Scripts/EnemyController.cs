using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  public float _damage = 10;
  // Start is called before the first frame update

  void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.tag == "Player")
    {
      var player = other.transform.GetComponent<PlayerController>();

      if (player != null)
      {
        player.TakeDamage(_damage);
      }
    }

  }
}
