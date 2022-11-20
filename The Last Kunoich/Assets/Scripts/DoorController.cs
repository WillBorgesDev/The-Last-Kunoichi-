using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float timer = 0.8f;
    public bool timeOn;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play("Stop");
        gameObject.tag = "Door";
        timeOn = false;
    }

    void Update()
    {
        if(timeOn == true)
        {
            if(timer >0 )
            {
                timer -= Time.deltaTime;
            } else if(timer <= 0)
            {
                timeOn = false;
                Destroy(gameObject);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "BasicAtack")
        {
            anim.Play("BrokenDoor");
            timeOn = true;
        }
    }
}
