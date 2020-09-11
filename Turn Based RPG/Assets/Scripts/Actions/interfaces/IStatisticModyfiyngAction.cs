using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatisticModyfiyngAction
{
    float CalculateStatChange(float power, StatisticsModule attackerStats, StatisticsModule targetStats);
    void ApplyStatChange(float value, StatisticsModule targetStats);
}
