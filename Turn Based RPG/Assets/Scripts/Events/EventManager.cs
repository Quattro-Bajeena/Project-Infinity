using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EventManager : MonoBehaviour
{

    private Dictionary<CombatEvents, Action<CombatEventData>> combatEventDictionary;
    private Dictionary<UIEvents, Action<UIEventData>> uiEventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogWarning("OOPS there has to at least one Event Manger in a scene, but proapbly unity deleted it before other scripts while quitting");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (combatEventDictionary == null)
        {
            combatEventDictionary = new Dictionary<CombatEvents, Action<CombatEventData>>();
        }
        if(uiEventDictionary == null)
        {
            uiEventDictionary = new Dictionary<UIEvents, Action<UIEventData>>();
        }
    }

    //Combat Events
    public static void StartListening(CombatEvents eventName, Action<CombatEventData> listener)
    {
        if (instance.combatEventDictionary.ContainsKey(eventName))
        {
            instance.combatEventDictionary[eventName] += listener;
        }
        else
        {
            instance.combatEventDictionary.Add(eventName, listener);
        }
    }

    public static void StopListening(CombatEvents eventName, Action<CombatEventData> listener)
    {
        if (instance)
        {
            if (instance.combatEventDictionary.ContainsKey(eventName))
            {
                instance.combatEventDictionary[eventName] -= listener;
                if (instance.combatEventDictionary[eventName] == null)
                {
                    instance.combatEventDictionary.Remove(eventName);
                }
            }
        }
        
    }

    public static void TriggerEvent(CombatEvents eventName, CombatEventData data)
    {

        if (instance.combatEventDictionary.ContainsKey(eventName))
        {
            Debug.Log("Combat: " + eventName.ToString());
            instance.combatEventDictionary[eventName].Invoke(data);
        }

        else Debug.LogWarning("Combat Warning: " + eventName.ToString());
    }


    //UI Events
    public static void StartListening(UIEvents eventName, Action<UIEventData> listener)
    {
        if (instance.uiEventDictionary.ContainsKey(eventName))
        {
            
            instance.uiEventDictionary[eventName] += listener;
        }
        else
        {
            instance.uiEventDictionary.Add(eventName, listener);
        }
    }


    public static void StopListening(UIEvents eventName, Action<UIEventData> listener)
    {
        if (instance)
        {
            if (instance.uiEventDictionary.ContainsKey(eventName))
            {
                instance.uiEventDictionary[eventName] -= listener;
                if (instance.uiEventDictionary[eventName] == null)
                {
                    instance.uiEventDictionary.Remove(eventName);
                }
            }
        }
        
        
    }


    public static void TriggerEvent(UIEvents eventName, UIEventData data)
    {

        if (instance.uiEventDictionary.ContainsKey(eventName))
        {
            Debug.Log("UI: " + eventName.ToString());
            instance.uiEventDictionary[eventName].Invoke(data);
        }
        else Debug.LogWarning("UI Warning: " + eventName.ToString());
    }

    
}