using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalModifier : MonoBehaviour
{
    [SerializeField] StatisticsModule.Elements element;
    public float CalculateElementalModifier(float statChange, StatisticsModule targetStatistics)
    {
        return statChange * (100f - targetStatistics.resistances[element].Value)/100f;
    }
}
