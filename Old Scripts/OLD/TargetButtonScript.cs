using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TargetButtonScript : MonoBehaviour
{

    public string target;

    public void targetPicked()
    {
        //targetEvent.Invoke(target);
        EventManager.TriggerEvent(UIEvents.TargetPicked ,new UIEventData(target));
    }
}
