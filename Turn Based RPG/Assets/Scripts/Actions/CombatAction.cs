using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAction : MonoBehaviour
{
    public enum ActionType
    {
        Ability,
        Attack,
        Combo
    }

    public enum ActionRange
    {
        Single,
        All
    }

    public ActionRange actionRange;
    public ActionType actionType;

    public List<IStatisticModyfiyngAction> StatisticModifiers { get; private set; }
    public List<ElementalModifier> ElementalDamages { get; private set; }
    public ITargetType TargetType { get; private set; }
    public IUsesResourceStat ResourceType { get; private set; }
    public ComboAction comboAction { get; private set; }

    BaseAttack baseAttack;
    AbilityMovePosition movePosition;

    public bool IsTargetPositionStationary
	{
		get
		{
            if (movePosition == false)
                return true;
            else return movePosition.movePosition == AbilityMovePosition.TargetMovePosition.StartingPosition;
        }
	}

    public BaseAttackType BaseAttackType
    {
        get
        {
            if (baseAttack)
            {
                return baseAttack.attackType;
            }
            else
            {
                return BaseAttackType.NULL;
            }
        }
    }

    public string animationName;

    public string actionName;
    public float power;
    public float cost;
    


    //public List<ActionEffect> effects = new List<ActionEffect>();


    void Awake()
    {
        StatisticModifiers = new List<IStatisticModyfiyngAction>();
        ElementalDamages = new List<ElementalModifier>();


        StatisticModifiers.AddRange(GetComponents<IStatisticModyfiyngAction>());
        ResourceType = GetComponent<IUsesResourceStat>();
        ElementalDamages.AddRange(GetComponents<ElementalModifier>());
        comboAction = GetComponent<ComboAction>();
        baseAttack = GetComponent<BaseAttack>();
        TargetType = GetComponent<ITargetType>();
        movePosition = GetComponent<AbilityMovePosition>();

        if (actionType == ActionType.Attack || actionType == ActionType.Combo)
        {
            gameObject.AddComponent<TargetEnemies>();
            TargetType = GetComponent<ITargetType>();
            actionRange = ActionRange.Single;

        }
        if (actionType == ActionType.Combo)
        {
            cost = 0;
        }

        

    }

    public List<BaseAttackType> ComboInput
    {
        get
        {
            if (comboAction != null) { return comboAction.requiredInput; }
            else return null;
        }
    }
    
    public void UseResourceStat(EntityStatistics attackerStats)
    {
        if(ResourceType != null)
        {
            ResourceType.UseResourceStat(attackerStats, cost);
        }
        
    }

    public bool IsEnoughResource(EntityStatistics attackerStats)
    {
        if (ResourceType != null)
        {
            return ResourceType.IsEnoughResource(attackerStats, cost);
        }
        else return true;
    }

    public Vector3 GetMovePosition(Vector3 attackerPosition, Vector3 targetPosition, Vector3 battlefiedCenterPosition)
	{
        if (actionType == ActionType.Attack || actionType == ActionType.Combo)
            return targetPosition;

        else
		{
            if (movePosition == false)
                return attackerPosition;
            
			switch (movePosition.movePosition)
			{
                case AbilityMovePosition.TargetMovePosition.BattlefieldCenter:
                    return battlefiedCenterPosition;

                case AbilityMovePosition.TargetMovePosition.StartingPosition:
                    return attackerPosition;

                case AbilityMovePosition.TargetMovePosition.TargetPosition:
                    return targetPosition;
            }
		}

        return attackerPosition;

    }

    public void ModyfiStatistics(EntityStatistics attackerStats, EntityStatistics targetStats)
    {
        foreach (IStatisticModyfiyngAction modifier in StatisticModifiers)
        {

            float value = CalculateModifierValue(modifier, attackerStats, targetStats);
            modifier.ApplyStatChange(value, targetStats);
        }
  
    }

    //FUNCTION OBSOLETE EVENT HEALTH CHANGE WILL BE CALLED INSIDE STATISTIC MODULE
    public float GetHealthChange(EntityStatistics attackerStats, EntityStatistics targetStats)
    {
        float modifiedHealthValue = 0;

        foreach (IStatisticModyfiyngAction modifier in StatisticModifiers)
        {
            //if(modifier is IHealthModifier)
            //{
            //    modifiedHealthValue += CalculateModifierValue(modifier, attackerStats, targetStats);
            //}
        }

        return modifiedHealthValue;
    }

    float CalculateModifierValue(IStatisticModyfiyngAction modifier, EntityStatistics attackerStats, EntityStatistics targetStats)
    {
        float value = modifier.CalculateStatChange(power, attackerStats, targetStats);

        foreach (ElementalModifier elementalModifier in ElementalDamages)
        {
            value = elementalModifier.CalculateElementalModifier(value, targetStats);
        }
        

        return value;
    }


    public List<CombatModule> GetTargets(CombatModule attacker, List<CombatModule> potentialTargets)
    {
        return TargetType.GetTargets(attacker, potentialTargets);
    }

    public List<string> GetTargets(string attacker, bool isCharacter, List<CombatModule> potentialTargets)
    {
        return TargetType.GetTargetsById(attacker, isCharacter, potentialTargets);
    }

    public List<string> GetTargets(string attacker, bool isCharacter, List<GameObject> potentialTargetsGO)
    {
        List<CombatModule> potentialTargets = new List<CombatModule>();
        foreach (GameObject target in potentialTargetsGO)
        {
            potentialTargets.Add(target.GetComponent<CombatModule>());
        }
        return TargetType.GetTargetsById(attacker, isCharacter, potentialTargets);
    }



    public List<IActionEffect> GetEffects()
    {
        return new List<IActionEffect>();
    }

}
