using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    Rigidbody2D body;
    public float speed = 10; 
    public float forceY = 15;
    public float timer;
    public bool timeOn;
    // public float forceX = 2;
    public int side;

    Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        timeOn = true;
        body = GetComponent<Rigidbody2D>();
        
        if(side == 1)
        {
            speed = speed * side;
        } else
        {
            speed = speed * side;
        }

        body.AddForce(new UnityEngine.Vector2(0, forceY), ForceMode2D.Impulse);
        body.velocity = new UnityEngine.Vector2(speed, body.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(timeOn == true)
        {
            if(timer >0 )
            {
                timer -= Time.deltaTime;
            } else if(timer <= 0)
            {
                timer -= Time.deltaTime;
                anim.Play("Explosion");
            } 

            if(timer <= -0.3)
            {
                timeOn = false;
                Destroy(gameObject);
            }
        }
        
    }
}
