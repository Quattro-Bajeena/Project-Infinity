using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatisticModifier
{
    float calculateStatChange(float power, EntityStatistics attackerStats, EntityStatistics targetStats);
    void applyStatChange(float value, EntityStatistics targetStats);
}
