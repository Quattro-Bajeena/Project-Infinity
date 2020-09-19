using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetType : ScriptableObject
{

    public abstract List<CombatModule> GetTargets(CombatModule attacker, List<CombatModule> potentialTargets);
    public abstract List<string> GetTargetsById(string attackerId, bool isCharacter, List<CombatModule> potentialTargets);

}
