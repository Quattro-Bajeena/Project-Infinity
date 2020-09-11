using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDamage : MonoBehaviour, IStatisticModyfiyngAction
{

	public float CalculateStatChange(float power, StatisticsModule attackerStats, StatisticsModule targetStats)
	{
        return (attackerStats.atributes[StatisticsModule.Atribute.Force].Value - targetStats.skills[StatisticsModule.Skill.Composure].Value) * power;
    }

	public void ApplyStatChange(float value, StatisticsModule targetStats)
	{
        targetStats.ApplyDamage(value);
	}
}
