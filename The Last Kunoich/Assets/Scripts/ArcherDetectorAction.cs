using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherDetectorAction : MonoBehaviour
{
    [SerializeField] private LayerMask layerPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // private bool isPlay()
    // {
    //     RaycastHit2D playerR = Physics2D.BoxCast(bc2d.bounds.center, bc2d.bounds.size, 0, UnityEngine.Vector2.right, 0.1f, layerPlayer);
    //     RaycastHit2D playerL = Physics2D.BoxCast(bc2d.bounds.center, bc2d.bounds.size, 0, UnityEngine.Vector2.left, 0.1f, layerPlayer);
    //     if()
    //     return (playerR.collider != null) || (playerL.collider != null);
    // } 

    // void OnTriggerEnter2D(Collider2D collider)
    // {
    //     if(collider.tag == "Player")
    //     {
    //         ArcherController.detected();
    //     }
    // }
}
