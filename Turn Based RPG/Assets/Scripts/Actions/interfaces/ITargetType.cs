using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetType
{

    List<CombatScript> getTargets(CombatScript attacker, List<CombatScript> potentialTargets);
    List<string> getTargetsById(string attackerId, bool isCharacter, List<CombatScript> potentialTargets);

}
