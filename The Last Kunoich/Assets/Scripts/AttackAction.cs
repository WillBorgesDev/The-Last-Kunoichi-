using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : MonoBehaviour
{
    private BoxCollider2D colliderATK;
    void Start()
    {
        colliderATK = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.targetSide == 1)
        {
            colliderATK.offset = new Vector2(0.09f, 0.02f);
        } if (PlayerController.targetSide == -1)
        {
            colliderATK.offset = new Vector2(-0.09f, 0.02f);
        }
    }
}
