using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenController : MonoBehaviour
{
    Rigidbody2D body;
    public float speed = 20; 
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
}
