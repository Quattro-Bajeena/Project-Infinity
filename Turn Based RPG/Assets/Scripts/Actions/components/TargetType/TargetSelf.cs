using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelf : MonoBehaviour, ITargetType
{
    public List<CombatScript> getTargets(CombatScript attacker, List<CombatScript> potentialTargets)
    {
        return new List<CombatScript>() { attacker };
    }

    public List<string> getTargetsById(string attackerId, bool isCharacter, List<CombatScript> potentialTargets)
    {
        return new List<string>() { attackerId };
    }

    
}
