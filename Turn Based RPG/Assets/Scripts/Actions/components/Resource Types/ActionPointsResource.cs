using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Action Point Resource", menuName = "ScriptableObjects/Action Components/Resource Types/Action Points")]
public class ActionPointsResource : UsesResourceStat
{
    public override bool IsEnoughResource(StatisticsModule attackerStats, int cost)
    {
        return attackerStats.resources[StatisticsModule.Resource.ActionPoints].IsEnough(cost);
    }

    public override void UseResourceStat(StatisticsModule attackerStats, int cost)
    {
        attackerStats.resources[StatisticsModule.Resource.ActionPoints].Use(cost);
    }
}
