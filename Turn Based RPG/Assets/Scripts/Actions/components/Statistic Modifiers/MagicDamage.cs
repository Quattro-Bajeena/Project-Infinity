using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDamage : MonoBehaviour, IStatisticModifier, IHealthModifier, IStatisticLowerer
{
    

    public float CalculateStatChange(float power, EntityStatistics attackerStats, EntityStatistics targetStats)
    {
        //float value = -1 * (5 * attackerStats.magic - 4 * targetStats.magicDefense) * power;
        return -1 * (5 * attackerStats.magic - 4 * targetStats.magicDefense) * power;
    }

    public void ApplyStatChange(float value, EntityStatistics targetStats)
    {
        targetStats.health += value;
        
    }

}
