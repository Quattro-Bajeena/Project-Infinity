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

    
    //CombatScript entity;

    List<IStatisticModifier> statisticModifiers = new List<IStatisticModifier>();
    List<ElementalModifier> elementalDamages = new List<ElementalModifier>();
    ITargetType targetType;
    IUsesResourceStat resourceType;
    ActionRange actionRange;
    ComboAction comboAction;

    BaseAttack baseAttack;

    public BaseAttackType baseAttackType
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

    public AnimationClip actionAnimation;

    public string actionName;
    public float power;
    public float cost;
    public ActionType actionType;
    //public List<ActionEffect> effects = new List<ActionEffect>();


    void Awake()
    {
        statisticModifiers.AddRange(GetComponents<IStatisticModifier>());
        resourceType = GetComponent<IUsesResourceStat>();
        elementalDamages.AddRange(GetComponents<ElementalModifier>());
        actionRange = GetComponent<ActionRange>();
        comboAction = GetComponent<ComboAction>();
        baseAttack = GetComponent<BaseAttack>();
        targetType = GetComponent<ITargetType>();

        if (actionType == ActionType.Attack || actionType == ActionType.Combo)
        {
            gameObject.AddComponent<TargetEnemies>();
            targetType = GetComponent<ITargetType>();
            actionRange.thisRange = ActionRange.Range.Single;

        }
        if(actionType == ActionType.Combo)
        {
            cost = 0;
        }

        

    }

    public List<BaseAttackType> comboInput
    {
        get
        {
            if (comboAction != null) { return comboAction.requiredInput; }
            else return null;
        }
    }
    
    public void useResourceStat(EntityStatistics attackerStats)
    {
        if(resourceType != null)
        {
            resourceType.useResourceStat(attackerStats, cost);
        }
        
    }

    public bool isEnoughResource(EntityStatistics attackerStats)
    {
        if (resourceType != null)
        {
            return resourceType.isEnoughResource(attackerStats, cost);
        }
        else return true;
    }

    public void modyfiStatistics(EntityStatistics attackerStats, EntityStatistics targetStats)
    {
        foreach (IStatisticModifier modifier in statisticModifiers)
        {

            float value = calculateModifierValue(modifier, attackerStats, targetStats);
            modifier.applyStatChange(value, targetStats);
        }
  
    }

    public float getHealthChange(EntityStatistics attackerStats, EntityStatistics targetStats)
    {
        float modifiedHealthValue = 0;

        foreach (IStatisticModifier modifier in statisticModifiers)
        {
            if(modifier is IHealthModifier)
            {
                modifiedHealthValue += calculateModifierValue(modifier, attackerStats, targetStats);
            }
        }

        return modifiedHealthValue;
    }

    float calculateModifierValue(IStatisticModifier modifier, EntityStatistics attackerStats, EntityStatistics targetStats)
    {
        float value = modifier.calculateStatChange(power, attackerStats, targetStats);

        foreach (ElementalModifier elementalModifier in elementalDamages)
        {
            value = elementalModifier.calculateElementalModifier(value, targetStats);
        }
        value *=  1 + UnityEngine.Random.Range(0f, 0.1f);
        return (float)Math.Round(value);
    }


    public List<CombatScript> getTargets(CombatScript attacker, List<CombatScript> potentialTargets)
    {
        return targetType.getTargets(attacker, potentialTargets);
    }

    public List<string> getTargets(string attacker, bool isCharacter, List<CombatScript> potentialTargets)
    {
        return targetType.getTargetsById(attacker, isCharacter, potentialTargets);
    }

    public List<string> getTargets(string attacker, bool isCharacter, List<GameObject> potentialTargetsGO)
    {
        List<CombatScript> potentialTargets = new List<CombatScript>();
        foreach (GameObject target in potentialTargetsGO)
        {
            potentialTargets.Add(target.GetComponent<CombatScript>());
        }
        return targetType.getTargetsById(attacker, isCharacter, potentialTargets);
    }

    public List<IActionEffect> getEffects()
    {
        return new List<IActionEffect>();
    }

}
