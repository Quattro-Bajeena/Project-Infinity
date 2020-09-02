using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPointsResource : MonoBehaviour, IUsesResourceStat
{
    public bool isEnoughResource(EntityStatistics attackerStats, float cost)
    {
        return attackerStats.actionPoints >= cost;
    }

    public void useResourceStat(EntityStatistics attackerStats, float cost)
    {
        attackerStats.actionPoints -= cost;
    }
}
