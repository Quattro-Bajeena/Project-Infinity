using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDamage : MonoBehaviour, IStatisticModyfiyngAction
{
    

    public float CalculateStatChange(float power, EntityStatistics attackerStats, EntityStatistics targetStats)
    {
        //float value = -1 * (5 * attackerStats.magic - 4 * targetStats.magicDefense) * power;
        return -1 * ( attackerStats.magic - targetStats.magicDefense) * power;
    }

    public void ApplyStatChange(float value, EntityStatistics targetStats)
    {
        targetStats.health += value;
        
    }

	public float CalculateStatChange(float power, StatisticsModule attackerStats, StatisticsModule targetStats)
	{
        return (attackerStats.force.Value - targetStats.composure.Value) * power;
    }

	public void ApplyStatChange(float value, StatisticsModule targetStats)
	{
        targetStats.ApplyDamage(value);
	}
}
