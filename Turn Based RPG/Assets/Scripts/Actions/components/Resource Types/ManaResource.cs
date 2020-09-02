using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaResource : MonoBehaviour, IUsesResourceStat
{
    public bool IsEnoughResource(EntityStatistics attackerStats, float cost)
    {
        return attackerStats.mana >= cost;
    }

    public void UseResourceStat(EntityStatistics attackerStats, float cost)
    {
        attackerStats.mana -= cost;
    }

    
}
