using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour, IStatisticModifier, IHealthModifier
{
    

    public float calculateStatChange(float power, EntityStatistics attackerStats, EntityStatistics targetStats)
    {
        return targetStats.magic * power;
    }

    public void applyStatChange(float value, EntityStatistics targetStats)
    {
        targetStats.health += value;
        
    }

    //public float getModifiedHealth(float power, EntityStatistics attackerStats, EntityStatistics targetStats)
    //{
    //    return calculateStatChange(power, attackerStats, targetStats);
    //}
}
