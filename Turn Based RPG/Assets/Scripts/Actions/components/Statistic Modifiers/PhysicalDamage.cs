using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalDamage : MonoBehaviour ,IStatisticModifier, IHealthModifier , IStatisticLowerer
{
    

    public float CalculateStatChange(float power, EntityStatistics attackerStats, EntityStatistics targetStats)
    {
        //return -1 * (4 * attackerStats.attack - 3 * targetStats.defense) * power;
        return -1 * ( attackerStats.attack - targetStats.defense) * power;
    }

    public void ApplyStatChange(float value, EntityStatistics targetStats)
    {
        targetStats.health += value;
        
    }
}
