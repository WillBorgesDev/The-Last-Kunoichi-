using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathImediateAction : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
      if (other.gameObject.tag == "Player")
      {
        var player = other.transform.GetComponent<PlayerController>();

        if (player != null)
        {
          player.death();
        }
      }
    }
}
