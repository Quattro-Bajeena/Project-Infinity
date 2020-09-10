using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public enum CombatEvents
{
    ReadyForAction, // id
    PermitAction, //id
    StartingAction, //id ReadyAction
    HealthChange, // target id, damage
    CombatAnimationFinished, //id
    ActionCompleted, // id
    SuspensionToggle,
    EntityDied, //id
    ComboLaunched
    
}



public struct CombatEventData
{
    public string id;
    //public List<string> targetIDs;
    public string targetID;
    public float healthChange;
    public CombatAction action;

    //permit action, ready for action, starting action, action completed, entity died
    public CombatEventData(string id)
    {
        this.id = id;
        this.targetID = null;
        this.healthChange = 0;
        this.action = null;
    }

    //Combat animation finished, Combo launched
    public CombatEventData(string id, CombatAction action)
    {
        this.id = id;
        this.targetID = null;
        this.healthChange = 0;
        this.action = action;
    }

    //Damage delt
    public CombatEventData(string targetID, float damage)
    {
        this.id = null;
        this.targetID = targetID;
        this.healthChange = damage;
        this.action = null;
    }

}

public enum UIEvents
{
    TargetPicked, //target id
    AbilityPicked, //CombatAction
    AttackLaunched, //CombatAction
    ActionLaunched, //attacker id, target id, CombatAction
    AttackCanceled, //id
    SelectedButtonChanged, //buttonSelected
    AttackMenuSelected, //id
    AttackMenuCanceled

}
public struct UIEventData
{
    public string id;
    public List<string> targetsId;
    public CombatAction action;
    public GameObject buttonSelected;
    
    //target picked
    public UIEventData(string id)
    {
        this.id = id;
        this.targetsId = null;
        this.action = null;
        this.buttonSelected = null;
        
    }

    //action picked, attack launched
    public UIEventData(CombatAction action)
    {
        this.id = null;
        this.targetsId = null;
        this.action = action;
        this.buttonSelected = null;
    }

    //action launched
    public UIEventData(string id, List<string> targetsId, CombatAction action)
    {
        this.id = id;
        this.targetsId = targetsId;
        this.action = action;
        this.buttonSelected = null;
    }



    public UIEventData(GameObject buttonSelected)
    {
        this.id = null;
        this.targetsId = null;
        this.action = null;
        this.buttonSelected = buttonSelected;
    }

    
}
