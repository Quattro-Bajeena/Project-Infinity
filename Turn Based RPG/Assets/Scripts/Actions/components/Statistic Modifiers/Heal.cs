using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour, IStatisticModyfiyngAction
{
    

    public float CalculateStatChange(float power, EntityStatistics attackerStats, EntityStatistics targetStats)
    {
        return targetStats.magic * power;
    }

    public void ApplyStatChange(float value, EntityStatistics targetStats)
    {
        targetStats.health += value;
        
        
    }

	public float CalculateStatChange(float power, StatisticsModule attackerStats, StatisticsModule targetStats)
	{
		return targetStats.force.Value * power;
	}

	public void ApplyStatChange(float value, StatisticsModule targetStats)
	{
		targetStats.Heal(value);
	}

}
