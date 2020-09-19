using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ElementalModifier
{
    [SerializeField] StatisticsModule.Elements element;
    public float CalculateElementalModifier(float statChange, StatisticsModule targetStatistics)
    {
        return statChange * (100f - targetStatistics.resistances[element].Value)/100f;
    }
}
