using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAction : MonoBehaviour
{
    public enum Type
    {
        Ability,
        Attack,
        Combo
    }

    public enum Range
    {
        Single,
        All
    }

    

    public Range actionRange;
    public Type actionType;
    public StatisticsModule.DamageType damageType;
    [SerializeField] bool unavoidable = false;

    public ActionAnimationInfo animationInfo;
    public AnimationClip animationClip;

    public string actionName;
    [SerializeField] float power;
    public int cost;
    [SerializeField] [Range(0,100)] int baseHitChance = 20;

    public string description;


    ITargetType TargetType;
    IUsesResourceStat ResourceType;
    ComboAction comboAction;
    BaseAttackComponent baseAttack;
    AbilityMovePosition movePosition;

    List<IStatisticModyfiyngAction> StatisticModifiers;
    List<ElementalModifier> Elements;


    public bool IsTargetPositionStationary
	{
		get
		{
            if (movePosition == null)
                return true;
            else return movePosition.position == AbilityMovePosition.Position.StartingPosition;
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

    public bool IsAvoided { get; private set; }
    public bool IsBlocked { get; private set; }


    void Awake()
    {
        StatisticModifiers = new List<IStatisticModyfiyngAction>();
        Elements = new List<ElementalModifier>();


        StatisticModifiers.AddRange(GetComponents<IStatisticModyfiyngAction>());
        ResourceType = GetComponent<IUsesResourceStat>();
        Elements.AddRange(GetComponents<ElementalModifier>());
        comboAction = GetComponent<ComboAction>();
        baseAttack = GetComponent<BaseAttackComponent>();
        TargetType = GetComponent<ITargetType>();
        movePosition = GetComponent<AbilityMovePosition>();

        if (actionType == Type.Attack || actionType == Type.Combo)
        {
            if(TargetType == null)
                gameObject.AddComponent<TargetEnemies>();
            if(movePosition == null)
                gameObject.AddComponent<AbilityMovePosition>().position = AbilityMovePosition.Position.TargetPosition;
            TargetType = GetComponent<ITargetType>();
            actionRange = Range.Single;

        }
        if (actionType == Type.Combo)
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
        //if (actionType == ActionType.Attack || actionType == ActionType.Combo)
        //    return targetPosition;

        
		
        if (movePosition == null)
            return attackerPosition;
            
		switch (movePosition.position)
		{
            case AbilityMovePosition.Position.BattlefieldCenter:
                return battlefiedCenterPosition;

            case AbilityMovePosition.Position.StartingPosition:
                return attackerPosition;

            case AbilityMovePosition.Position.TargetPosition:
                return targetPosition;

            default:
                return attackerPosition;
                
        }
	
    }

    public void CalculateOutcome(StatisticsModule attackerStats, StatisticsModule targetStats)
	{
        IsAvoided = DodgedAction(attackerStats, targetStats);
        IsBlocked = BlockedAction(attackerStats, targetStats);
	}

    public void ModyfiStatistics(StatisticsModule attackerStats, StatisticsModule targetStats)
    {

        foreach (IStatisticModyfiyngAction modifier in StatisticModifiers)
        {
            float value = CalculateBaseModifierValue(modifier, attackerStats, targetStats);
            if(IsBlocked == true)
			{
                value *= 0.5f;
			}
            modifier.ApplyStatChange(value, targetStats);
        }
    }


    bool DodgedAction(StatisticsModule attackerStats, StatisticsModule targetStats)
    {
        if (unavoidable == false && damageType != StatisticsModule.DamageType.Force)
        {

            float chance = baseHitChance - targetStats.atributes[StatisticsModule.Atribute.Perception].Value;
            int diceRoll = UnityEngine.Random.Range(0, 100);


            if (diceRoll < chance)
            {
                return true;
            }
            else return false;
        }
        else return false;
        

    }

    bool BlockedAction(StatisticsModule attackerStats, StatisticsModule targetStats)
	{


        if (unavoidable == false && damageType != StatisticsModule.DamageType.Blaster)
        {
            //10 to 0 % chance
            float chance = baseHitChance - targetStats.skills[StatisticsModule.Skill.SelfDefence].Value / 10f;
            if (targetStats.Entity.combat.IsDefending == true)
                chance = 90f;

            int diceRoll = UnityEngine.Random.Range(0, 100);

            if (diceRoll < chance)
            {
                return true;
            }
            else return false;
        }
        else return false;
        
		
    }

    float CalculateBaseModifierValue(IStatisticModyfiyngAction modifier, StatisticsModule attackerStats, StatisticsModule targetStats)
    {
        float value = modifier.CalculateStatChange(power, attackerStats, targetStats);

        foreach (ElementalModifier damageTypeModifier in Elements)
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
