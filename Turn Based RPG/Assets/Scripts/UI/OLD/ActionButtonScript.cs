using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtonScript : MonoBehaviour
{

    public CombatAction action;
    //public ActionEvent actionEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void actionPicked()
    {
        //actionEvent.Invoke(action);
        EventManager.TriggerEvent(UIEvents.AbilityPicked, new UIEventData(action));
    }
}
