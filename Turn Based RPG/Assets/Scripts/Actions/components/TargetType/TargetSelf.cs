using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Target Self", menuName = "ScriptableObjects/Action Components/Target Types/Self")]
public class TargetSelf : TargetType
{
    public override List<CombatModule> GetTargets(CombatModule attacker, List<CombatModule> potentialTargets)
    {
        return new List<CombatModule>() { attacker };
    }

    public override List<string> GetTargetsById(string attackerId, bool isCharacter, List<CombatModule> potentialTargets)
    {
        return new List<string>() { attackerId };
    }

    
}
