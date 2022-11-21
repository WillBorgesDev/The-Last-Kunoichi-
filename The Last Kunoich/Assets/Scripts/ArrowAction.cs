using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAction : MonoBehaviour
{
    public float _damage = 10;
    Rigidbody2D body;
    public float speed = 20; 
    public int side;

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
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            var player = other.transform.GetComponent<PlayerController>();  
            if (player != null)
            {
                player.TakeDamage(_damage);
            }
        } else if (other.gameObject.tag == "BasicAtack")
        {
            Destroy(this.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        body.velocity = new UnityEngine.Vector2(speed, body.velocity.y);
    }
}
