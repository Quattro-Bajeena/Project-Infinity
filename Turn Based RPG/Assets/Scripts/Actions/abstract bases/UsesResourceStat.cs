using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class UsesResourceStat : ScriptableObject
{
    public abstract void UseResourceStat(StatisticsModule attackerStats, int cost);
    public abstract bool IsEnoughResource(StatisticsModule attackerStats, int cost);
}

