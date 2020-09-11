using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalDamage : MonoBehaviour ,IStatisticModyfiyngAction
{

	public float CalculateStatChange(float power, StatisticsModule attackerStats, StatisticsModule targetStats)
	{
        return (attackerStats.skills[StatisticsModule.Skill.Lightsaber].Value - targetStats.skills[StatisticsModule.Skill.SelfDefence].Value) * power;
    }

	public void ApplyStatChange(float value, StatisticsModule targetStats)
	{
        targetStats.ApplyDamage(value);
	}
}
