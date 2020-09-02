using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyAction
{
    public CombatScript attacker;
    public List<CombatScript> targets;

    public CombatAction action;

    public ReadyAction(CombatScript attacker, List<CombatScript> targets, CombatAction action)
    {
        this.attacker = attacker;
        this.targets = targets;
        this.action = action;
    }
}
