using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Target Allies", menuName = "ScriptableObjects/Action Components/Target Types/Allies")]
public class TargetAllies : TargetType
{
    public override List<CombatModule> GetTargets(CombatModule attacker, List<CombatModule> potentialTargets)
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


	public override List<string> GetTargetsById(string attackerId, bool isCharacter, List<CombatModule> potentialTargets)
    {
        List<string> targets = new List<string>();


        foreach (var potentialTarget in potentialTargets)
        {
            if (potentialTarget.IsCharacter == isCharacter)
            {
                targets.Add(potentialTarget.Entity.Id);
            }
        }

        return targets;
    }

}
