using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceResource : MonoBehaviour, IUsesResourceStat
{
    public bool IsEnoughResource(StatisticsModule attackerStats, int cost)
    {
        return attackerStats.resources[StatisticsModule.Resource.ForcePoints].IsEnough(cost);
    }

    public void UseResourceStat(StatisticsModule attackerStats, int cost)
    {
        attackerStats.resources[StatisticsModule.Resource.ForcePoints].Use(cost);
    }

    
}
