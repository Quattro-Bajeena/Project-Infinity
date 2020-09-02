using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatisticModifier
{
    float CalculateStatChange(float power, EntityStatistics attackerStats, EntityStatistics targetStats);
    void ApplyStatChange(float value, EntityStatistics targetStats);
}
