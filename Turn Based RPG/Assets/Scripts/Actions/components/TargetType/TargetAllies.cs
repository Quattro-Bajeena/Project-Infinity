using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAllies : MonoBehaviour ,ITargetType
{
    public List<CombatModule> GetTargets(CombatModule attacker, List<CombatModule> potentialTargets)
    {
        List<CombatModule> targets = new List<CombatModule>();


        foreach (var potentialTarget in potentialTargets)
        {
            if (potentialTarget.IsCharacter == attacker.IsCharacter)
            {
                targets.Add(potentialTarget);
            }
        }

        return targets;
    }

    public List<string> GetTargetsById(string attackerId, bool isCharacter, List<CombatModule> potentialTargets)
    {
        List<string> targets = new List<string>();


        foreach (var potentialTarget in potentialTargets)
        {
            if (potentialTarget.IsCharacter == isCharacter)
            {
                targets.Add(potentialTarget.Entity.Name);
            }
        }

        return targets;
    }
}
