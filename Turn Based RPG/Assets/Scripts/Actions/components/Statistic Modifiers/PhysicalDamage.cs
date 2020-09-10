using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalDamage : MonoBehaviour ,IStatisticModyfiyngAction
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

	public float CalculateStatChange(float power, StatisticsModule attackerStats, StatisticsModule targetStats)
	{
        return (attackerStats.lighstaber.Value - targetStats.selfDefence.Value) * power;
    }

	public void ApplyStatChange(float value, StatisticsModule targetStats)
	{
        targetStats.ApplyDamage(value);
	}
}
