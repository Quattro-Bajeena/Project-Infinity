using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour, IStatisticModifier, IHealthModifier, IStatisticRaiser
{
    

    public float CalculateStatChange(float power, EntityStatistics attackerStats, EntityStatistics targetStats)
    {
        return targetStats.magic * power;
    }

    public void ApplyStatChange(float value, EntityStatistics targetStats)
    {
        targetStats.health += value;
        
    }

    //public float getModifiedHealth(float power, EntityStatistics attackerStats, EntityStatistics targetStats)
    //{
    //    return calculateStatChange(power, attackerStats, targetStats);
    //}
}
