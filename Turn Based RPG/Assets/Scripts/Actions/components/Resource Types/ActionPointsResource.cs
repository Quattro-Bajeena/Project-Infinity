using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPointsResource : MonoBehaviour, IUsesResourceStat
{
    public bool IsEnoughResource(StatisticsModule attackerStats, int cost)
    {
        return attackerStats.resources[StatisticsModule.Resource.ActionPoints].IsEnough(cost);
    }

    public void UseResourceStat(StatisticsModule attackerStats, int cost)
    {
        attackerStats.resources[StatisticsModule.Resource.ActionPoints].Use(cost);
    }
}
