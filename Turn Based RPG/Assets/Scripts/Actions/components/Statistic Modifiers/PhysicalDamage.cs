using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Physical Damage", menuName = "ScriptableObjects/Action Components/Statistic Modifiers/Physical Damage")]
public class PhysicalDamage : StatisticModyfiyngAction
{

	public override float CalculateStatChange(float power, StatisticsModule attackerStats, StatisticsModule targetStats)
	{
        return (attackerStats.skills[StatisticsModule.Skill.Lightsaber].Value - targetStats.skills[StatisticsModule.Skill.SelfDefence].Value) * power;
    }

	public override void ApplyStatChange(float value, StatisticsModule targetStats)
	{
        targetStats.ApplyDamage(value);
	}
}
