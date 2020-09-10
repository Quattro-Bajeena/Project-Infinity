using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatisticModyfiyngAction
{
    float CalculateStatChange(float power, EntityStatistics attackerStats, EntityStatistics targetStats);
    void ApplyStatChange(float value, EntityStatistics targetStats);

    float CalculateStatChange(float power, StatisticsModule attackerStats, StatisticsModule targetStats);
    void ApplyStatChange(float value, StatisticsModule targetStats);
}
