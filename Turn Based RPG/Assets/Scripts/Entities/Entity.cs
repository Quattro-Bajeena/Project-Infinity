using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public string entityName;

    public StatisticsModule stats;
    public CombatModule combat;
    public AnimationModule animations;


    void Awake()
    {
        stats = GetComponent<StatisticsModule>();
        combat = GetComponent<CombatModule>();
        animations = GetComponent<AnimationModule>();
    }

    
}
