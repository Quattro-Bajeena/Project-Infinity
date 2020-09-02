using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDamage : MonoBehaviour, IStatisticModifier, IHealthModifier
{
    

    public float calculateStatChange(float power, EntityStatistics attackerStats, EntityStatistics targetStats)
    {
        return -1 * (5 * attackerStats.magic - 4 * targetStats.magicDefense) * power;
    }

    public void applyStatChange(float value, EntityStatistics targetStats)
    {
        targetStats.health += value;
        
    }

    //public float getModifiedHealth(float power, EntityStatistics attackerStats, EntityStatistics targetStats)
    //{
    //    return -1 * calculateStatChange(power, attackerStats, targetStats);
    //}
}
