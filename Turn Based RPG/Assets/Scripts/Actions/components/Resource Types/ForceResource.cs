using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Force Point Resource", menuName = "ScriptableObjects/Action Components/Resource Types/Force Points")]
public class ForceResource : UsesResourceStat
{
    public override bool IsEnoughResource(StatisticsModule attackerStats, int cost)
    {
        return attackerStats.resources[StatisticsModule.Resource.ForcePoints].IsEnough(cost);
    }

    public override void UseResourceStat(StatisticsModule attackerStats, int cost)
    {
        attackerStats.resources[StatisticsModule.Resource.ForcePoints].Use(cost);
    }

    
}
