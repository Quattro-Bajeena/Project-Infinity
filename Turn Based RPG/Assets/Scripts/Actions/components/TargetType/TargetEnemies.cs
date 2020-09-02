using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetEnemies : MonoBehaviour ,ITargetType
{
    public List<CombatScript> getTargets(CombatScript attacker, List<CombatScript> potentialTargets)
    {
        List<CombatScript> targets = new List<CombatScript>();
        

        foreach (var potentialTarget in potentialTargets)
        {
            if(potentialTarget.isCharacter != attacker.isCharacter)
            {
                targets.Add(potentialTarget);
            }
        }

        return targets;
    }

    public List<string> getTargetsById(string attackerId, bool isCharacter, List<CombatScript> potentialTargets)
    {
        List<string> targets = new List<string>();


        foreach (var potentialTarget in potentialTargets)
        {
            if (potentialTarget.isCharacter != isCharacter)
            {
                targets.Add(potentialTarget.entityName);
            }
        }

        return targets;
    }
}
