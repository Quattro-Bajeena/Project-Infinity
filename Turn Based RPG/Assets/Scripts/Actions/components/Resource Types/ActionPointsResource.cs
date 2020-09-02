using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPointsResource : MonoBehaviour, IUsesResourceStat
{
    public bool IsEnoughResource(EntityStatistics attackerStats, float cost)
    {
        return attackerStats.actionPoints >= cost;
    }

    public void UseResourceStat(EntityStatistics attackerStats, float cost)
    {
        attackerStats.actionPoints -= cost;
    }
}
