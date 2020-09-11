using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour, IStatisticModyfiyngAction
{


	public float CalculateStatChange(float power, StatisticsModule attackerStats, StatisticsModule targetStats)
	{
		return targetStats.atributes[StatisticsModule.Atribute.Force].Value * power;
	}

	public void ApplyStatChange(float value, StatisticsModule targetStats)
	{
		targetStats.Heal(value);
	}

}
