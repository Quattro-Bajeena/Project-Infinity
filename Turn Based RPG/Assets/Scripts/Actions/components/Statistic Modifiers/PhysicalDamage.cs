using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalDamage : MonoBehaviour ,IStatisticModifier, IHealthModifier 
{
    

    public float calculateStatChange(float power, EntityStatistics attackerStats, EntityStatistics targetStats)
    {

        return -1 * (4 * attackerStats.attack - 3 * targetStats.defense) * power; ;
    }

    public void applyStatChange(float value, EntityStatistics targetStats)
    {
        targetStats.health += value;
        
    }

    //public float getModifiedHealth(float power, EntityStatistics attackerStats, EntityStatistics targetStats)
    //{
    //    return -1 * calculateStatChange(power, attackerStats, targetStats); ;
    //}
}
