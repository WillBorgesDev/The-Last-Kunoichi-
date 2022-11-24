using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BauController : MonoBehaviour
{

    SpriteRenderer sprite;
    Animator anim;
    public int _whatUpgrade;

    public GameObject player;
    private bool stillOpen = false;
    public float fadeDuration;
    
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        anim.Play("Shuriken idle");
    }

    void Update() 
    {
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            var player = other.transform.GetComponent<PlayerController>();
            
            if(!stillOpen)
            {
                anim.Play("Shuriken Open");
                stillOpen = true;
                player.getUpgrade(_whatUpgrade); 
            } 
            
        }
    }
}
