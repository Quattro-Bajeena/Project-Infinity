using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalModifier : MonoBehaviour
{


    public StatisticsModule.DamageType thisDamageType;
    public float CalculateElementalModifier(float statChange, StatisticsModule targetStatistics)
    {
        return statChange * (100f - targetStatistics.resistances[thisDamageType].Value)/100f;
    }
}
