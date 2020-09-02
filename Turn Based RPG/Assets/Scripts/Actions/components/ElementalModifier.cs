using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalModifier : MonoBehaviour
{

    public enum Element
    {
        Fire,
        Water,
        Earth,
        Wind,
        Light,
        Dark
    }

    public Element thisElement;
    public float calculateElementalModifier(float statChange, EntityStatistics targetStatistics)
    {
        return statChange * (1 - targetStatistics.resistances[thisElement]);
    }
}
