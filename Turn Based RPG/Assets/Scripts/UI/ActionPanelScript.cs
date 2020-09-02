using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionPanelScript : MonoBehaviour
{

    public bool persistant = true;

    void Start()
    {
        
    }

    void OnDisable()
    {
        if(persistant == false)
        {
            Destroy(gameObject);
        }
    }
}
