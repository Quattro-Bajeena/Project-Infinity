using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelf : MonoBehaviour, ITargetType
{
    public List<CombatModule> GetTargets(CombatModule attacker, List<CombatModule> potentialTargets)
    {
        return new List<CombatModule>() { attacker };
    }

    public List<string> GetTargetsById(string attackerId, bool isCharacter, List<CombatModule> potentialTargets)
    {
        return new List<string>() { attackerId };
    }

    
}
