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

    //CombatScript entity;

    public List<IStatisticModifier> StatisticModifiers { get; private set; }
    public List<ElementalModifier> ElementalDamages { get; private set; }
    public ITargetType TargetType { get; private set; }
    public IUsesResourceStat ResourceType { get; private set; }

    public ActionRange actionRange;
    public ComboAction comboAction;

    BaseAttack baseAttack;

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

    public AnimationClip actionAnimation;
    public string animationName;

    public string actionName;
    public float power;
    public float cost;
    public ActionType actionType;

    

    //public List<ActionEffect> effects = new List<ActionEffect>();


    void Awake()
    {
        StatisticModifiers = new List<IStatisticModifier>();
        ElementalDamages = new List<ElementalModifier>();


        StatisticModifiers.AddRange(GetComponents<IStatisticModifier>());
        ResourceType = GetComponent<IUsesResourceStat>();
        ElementalDamages.AddRange(GetComponents<ElementalModifier>());
        comboAction = GetComponent<ComboAction>();
        baseAttack = GetComponent<BaseAttack>();
        TargetType = GetComponent<ITargetType>();

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

    public void ModyfiStatistics(EntityStatistics attackerStats, EntityStatistics targetStats)
    {
        foreach (IStatisticModifier modifier in StatisticModifiers)
        {

            float value = CalculateModifierValue(modifier, attackerStats, targetStats);
            modifier.ApplyStatChange(value, targetStats);
        }
  
    }

    public float GetHealthChange(EntityStatistics attackerStats, EntityStatistics targetStats)
    {
        float modifiedHealthValue = 0;

        foreach (IStatisticModifier modifier in StatisticModifiers)
        {
            if(modifier is IHealthModifier)
            {
                modifiedHealthValue += CalculateModifierValue(modifier, attackerStats, targetStats);
            }
        }

        return modifiedHealthValue;
    }

    float CalculateModifierValue(IStatisticModifier modifier, EntityStatistics attackerStats, EntityStatistics targetStats)
    {
        float value = modifier.CalculateStatChange(power, attackerStats, targetStats);

        foreach (ElementalModifier elementalModifier in ElementalDamages)
        {
            value = elementalModifier.CalculateElementalModifier(value, targetStats);
        }
        value *=  1 + UnityEngine.Random.Range(0f, 0.1f);

        if(modifier is IStatisticLowerer)
        {
            value = Mathf.Clamp(value, -9999, -1);
        }
        else if(modifier is IStatisticRaiser)
        {
            value = Mathf.Clamp(value, 1, 9999);
        }

        return (float)Math.Round(value);
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
