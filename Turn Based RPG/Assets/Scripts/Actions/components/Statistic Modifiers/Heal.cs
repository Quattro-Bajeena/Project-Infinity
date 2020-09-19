using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObjects/Action Components/Statistic Modifiers/Heal")]
public class Heal : StatisticModyfiyngAction
{

	public override float CalculateStatChange(float power, StatisticsModule attackerStats, StatisticsModule targetStats)
	{
		return attackerStats.atributes[StatisticsModule.Atribute.Force].Value * power;
	}

	public override void ApplyStatChange(float value, StatisticsModule targetStats)
	{
		targetStats.Heal(value);
	}

}
