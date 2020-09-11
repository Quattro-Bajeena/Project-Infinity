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
    public List<DamageTypeModifier> DamageTypes { get; private set; }
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
    public int cost;

    public string description;

    //public List<ActionEffect> effects = new List<ActionEffect>();


    void Awake()
    {
        StatisticModifiers = new List<IStatisticModyfiyngAction>();
        DamageTypes = new List<DamageTypeModifier>();


        StatisticModifiers.AddRange(GetComponents<IStatisticModyfiyngAction>());
        ResourceType = GetComponent<IUsesResourceStat>();
        DamageTypes.AddRange(GetComponents<DamageTypeModifier>());
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
    
    public void UseResourceStat(StatisticsModule attackerStats)
    {
        if(ResourceType != null)
        {
            ResourceType.UseResourceStat(attackerStats, cost);
        }
        
    }

    public bool IsEnoughResource(StatisticsModule attackerStats)
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

    public void ModyfiStatistics(StatisticsModule attackerStats, StatisticsModule targetStats)
    {

        foreach (IStatisticModyfiyngAction modifier in StatisticModifiers)
        {

            float value = CalculateModifierValue(modifier, attackerStats, targetStats);
            modifier.ApplyStatChange(value, targetStats);
        }
    }
    public bool DodgedAction(StatisticsModule attackerStats, StatisticsModule targetStats)
    {
        int diceRoll = UnityEngine.Random.Range(0, 100);

        //base chance for hit 95% - perception
        if(diceRoll > 80 - attackerStats.atributes[StatisticsModule.Atribute.Perception].Value)
		{
            return true;
		}
        else return false;

    }

    float CalculateModifierValue(IStatisticModyfiyngAction modifier, StatisticsModule attackerStats, StatisticsModule targetStats)
    {
        float value = modifier.CalculateStatChange(power, attackerStats, targetStats);

        foreach (DamageTypeModifier damageTypeModifier in DamageTypes)
        {
            value = damageTypeModifier.CalculateElementalModifier(value, targetStats);
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
