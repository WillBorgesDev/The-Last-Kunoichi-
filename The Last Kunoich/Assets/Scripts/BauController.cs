using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BauController : MonoBehaviour
{

    SpriteRenderer sprite;
    Animator anim;

    public GameObject player;
    private bool isPlayer = false;
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

        if(Input.GetKeyDown(KeyCode.W) && isPlayer == true)
        {
            anim.Play("Shuriken Open");
            isPlayer = false;
            stillOpen = true;       
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(!stillOpen)
            {
                isPlayer = true;
            }
            
        }
    }
}
