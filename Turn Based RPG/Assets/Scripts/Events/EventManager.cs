using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EventManager : MonoBehaviour
{

    private Dictionary<CombatEvents, Action<CombatEventData>> combatEventDictionary;
    private Dictionary<UIEvents, Action<UIEventData>> uiEventDictionary;

    private static EventManager eventManager;

    public static EventManager Instance
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
        if (Instance.combatEventDictionary.ContainsKey(eventName))
        {
            Instance.combatEventDictionary[eventName] += listener;
        }
        else
        {
            Instance.combatEventDictionary.Add(eventName, listener);
        }
    }

    public static void StopListening(CombatEvents eventName, Action<CombatEventData> listener)
    {
        if (Instance)
        {
            if (Instance.combatEventDictionary.ContainsKey(eventName))
            {
                Instance.combatEventDictionary[eventName] -= listener;
                if (Instance.combatEventDictionary[eventName] == null)
                {
                    Instance.combatEventDictionary.Remove(eventName);
                }
            }
        }
        
    }

    public static void TriggerEvent(CombatEvents eventName, CombatEventData data)
    {

        if (Instance.combatEventDictionary.ContainsKey(eventName))
        {
            Debug.Log("Combat: " + eventName.ToString());
            Instance.combatEventDictionary[eventName].Invoke(data);
        }

        else Debug.LogWarning("Combat Warning: " + eventName.ToString());
    }


    //UI Events
    public static void StartListening(UIEvents eventName, Action<UIEventData> listener)
    {
        if (Instance.uiEventDictionary.ContainsKey(eventName))
        {
            
            Instance.uiEventDictionary[eventName] += listener;
        }
        else
        {
            Instance.uiEventDictionary.Add(eventName, listener);
        }
    }


    public static void StopListening(UIEvents eventName, Action<UIEventData> listener)
    {
        if (Instance)
        {
            if (Instance.uiEventDictionary.ContainsKey(eventName))
            {
                Instance.uiEventDictionary[eventName] -= listener;
                if (Instance.uiEventDictionary[eventName] == null)
                {
                    Instance.uiEventDictionary.Remove(eventName);
                }
            }
        }
        
        
    }


    public static void TriggerEvent(UIEvents eventName, UIEventData data)
    {

        if (Instance.uiEventDictionary.ContainsKey(eventName))
        {
            Debug.Log("UI: " + eventName.ToString());
            Instance.uiEventDictionary[eventName].Invoke(data);
        }
        else Debug.LogWarning("UI Warning: " + eventName.ToString());
    }

    
}