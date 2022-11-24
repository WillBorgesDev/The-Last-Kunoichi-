using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackAction : MonoBehaviour
{
    public float _damage = 10;
    public int flip;
    private BoxCollider2D colliderATK;
    // Start is called before the first frame update
    void Start()
    {
        colliderATK = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        flip = EnemyController.flipState;
        if (EnemyController.flipState == -1)
        {
            colliderATK.offset = new Vector2(-0.20f, 0.02f);
        } else if (EnemyController.flipState == 1)
        {
            colliderATK.offset = new Vector2(0.20f, 0.02f);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
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
