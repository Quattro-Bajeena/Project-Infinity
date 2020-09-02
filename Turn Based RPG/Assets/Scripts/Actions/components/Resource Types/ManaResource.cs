using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaResource : MonoBehaviour, IUsesResourceStat
{
    public bool isEnoughResource(EntityStatistics attackerStats, float cost)
    {
        return attackerStats.mana >= cost;
    }

    public void useResourceStat(EntityStatistics attackerStats, float cost)
    {
        attackerStats.mana -= cost;
    }

    
}
