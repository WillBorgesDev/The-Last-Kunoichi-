using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : MonoBehaviour
{
    public GameObject arrow;
    public Transform target;
    Animator anim; 
    public static bool isDetect;

    void Start()
    {
        anim = GetComponent<Animator>();
        isDetect = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDetect == true)
        {
            attack();
        }
    }

    public static void detected()
    {
        isDetect = true;
    }
    
    public void attack()
    {
        anim.Play("Attack");
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        // if(collider.tag == "BasicAtack" || collider.tag == "DashAttack")
        // {
        //     
        //     
        // }
    }

    public void instanceArrow()
    {
        Instantiate(arrow, target.position, target.rotation);
    }
    
}
