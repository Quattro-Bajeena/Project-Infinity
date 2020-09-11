using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatisticModifier 
{
    public enum ValueType
	{
		Flat,
		Multiplier
	}

	public enum DurationType
	{
		Battle,
		Equip,
		Permament
	}

	public string name;
	public ValueType valueType;
	public DurationType durationType;
	public float value;
}
