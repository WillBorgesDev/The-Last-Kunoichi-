using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canEndLevel : MonoBehaviour
{
    public GameObject end;
    public static bool bossDie;
    // public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        bossDie = true;
        end.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindWithTag("Boss") == null)
        {
            end.SetActive(true);
        }
    }
    
}
    
