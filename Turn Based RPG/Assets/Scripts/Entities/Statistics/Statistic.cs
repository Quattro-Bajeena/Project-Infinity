using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Statistic
{
    [SerializeField] int baseValue;
	List<StatisticModifier> modifiers;

    public int Value
	{
		get
		{
			float currentValue = baseValue;
			foreach (StatisticModifier modifier in modifiers)
			{
				if(modifier.valueType == StatisticModifier.ValueType.Multiplier)
				{
					currentValue *= 1 + modifier.value;
				}
				else if(modifier.valueType == StatisticModifier.ValueType.Flat)
				{
					currentValue += modifier.value;
				}
			}
			return Mathf.RoundToInt(currentValue);
		}
	}

	public void AddModifier(StatisticModifier modifier)
	{
		modifiers.Add(modifier);
	}

	public void RemoveModifier(StatisticModifier modifier)
	{
		modifiers.Remove(modifier);
	}

	public void ResetBattleModifiers()
	{
		modifiers.RemoveAll(modifier => modifier.durationType == StatisticModifier.DurationType.Battle);
	}
}
