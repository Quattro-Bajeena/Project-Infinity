using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatisticModyfiyngAction : ScriptableObject
{
    public abstract float CalculateStatChange(float power, StatisticsModule attackerStats, StatisticsModule targetStats);
    public abstract void ApplyStatChange(float value, StatisticsModule targetStats);
}
