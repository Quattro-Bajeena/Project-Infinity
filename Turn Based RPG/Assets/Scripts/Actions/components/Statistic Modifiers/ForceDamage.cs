using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Force Damage", menuName = "ScriptableObjects/Action Components/Statistic Modifiers/Force Damage")]
public class ForceDamage : StatisticModyfiyngAction
{

	public override float CalculateStatChange(float power, StatisticsModule attackerStats, StatisticsModule targetStats)
	{
        return (attackerStats.atributes[StatisticsModule.Atribute.Force].Value - targetStats.skills[StatisticsModule.Skill.Composure].Value) * power;
    }

	public override void ApplyStatChange(float value, StatisticsModule targetStats)
	{
        targetStats.ApplyDamage(value);
	}
}
