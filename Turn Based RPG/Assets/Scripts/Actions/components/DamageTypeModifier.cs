using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTypeModifier : MonoBehaviour
{
    public StatisticsModule.DamageType thisDamageType;
    public float CalculateElementalModifier(float statChange, StatisticsModule targetStatistics)
    {
        return statChange * (100f - targetStatistics.defenses[thisDamageType].Value)/100f;
    }
}
