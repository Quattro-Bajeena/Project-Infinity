using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Action", menuName = "ScriptableObjects/Action")]
public class CombatAction : ScriptableObject
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

    
    [SerializeField] float power;
    public int cost;
    [SerializeField] [Range(0,100)] int baseHitChance = 20;

    public string actionName;
    public string description;
    public AnimationClip animationClip;
    

    [SerializeField] TargetType TargetType;
    [SerializeField] UsesResourceStat ResourceType;
    [SerializeField] AbilityMovePosition movePosition;
    [SerializeField] List<StatisticModyfiyngAction> StatisticModifiers = new List<StatisticModyfiyngAction>();
    [SerializeField] List<ElementalModifier> Elements = new List<ElementalModifier>();

    [Space(10)]
    [Header("OPTIONAL")]
    [SerializeField] ComboAction comboAction; //optional
    [SerializeField] BaseAttackComponent baseAttack; //optional


    public List<BaseAttackType> ComboInput
    {
        get
        {
            if (comboAction != null) { return comboAction.requiredInput; }
            else return null;
        }
    }
    public BaseAttackType BaseAttackType
    {
		get { return baseAttack.attackType; }
    }

    public bool IsTargetPositionStationary
    {
        get
        {
            if (movePosition == null)
                return true;
            else return movePosition.position == AbilityMovePosition.Position.StartingPosition;
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

    public void ModyfiStatistics(StatisticsModule attackerStats, StatisticsModule targetStats, bool blocked)
    {

        foreach (StatisticModyfiyngAction modifier in StatisticModifiers)
        {
            float value = CalculateBaseModifierValue(modifier, attackerStats, targetStats);
            if(blocked == true)
			{
                value *= 0.5f;
			}
            modifier.ApplyStatChange(value, targetStats);
        }
    }


    public bool DodgedAction(StatisticsModule attackerStats, StatisticsModule targetStats)
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

    public bool BlockedAction(StatisticsModule attackerStats, StatisticsModule targetStats)
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

    float CalculateBaseModifierValue(StatisticModyfiyngAction modifier, StatisticsModule attackerStats, StatisticsModule targetStats)
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



    public List<ActionEffect> GetEffects()
    {
        return new List<ActionEffect>();
    }

}
