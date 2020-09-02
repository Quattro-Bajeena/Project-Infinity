using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetType
{

    List<CombatModule> GetTargets(CombatModule attacker, List<CombatModule> potentialTargets);
    List<string> GetTargetsById(string attackerId, bool isCharacter, List<CombatModule> potentialTargets);

}
