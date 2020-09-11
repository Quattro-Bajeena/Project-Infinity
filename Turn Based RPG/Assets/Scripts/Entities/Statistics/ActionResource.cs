using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionResource
{

	[SerializeField] int maxValue;
	public int MaxValue
	{
		get { return maxValue; }
	}

	[SerializeField] int currentValue;
	public int CurrentValue
	{
		get { return currentValue; }
	}


	public float CurrentPercentage
	{
		get { return (float)CurrentValue / maxValue; }
	}

	public bool IsEnough(int cost)
	{
		return cost <= CurrentValue;
	}


	public void Use(int value)
	{
		currentValue -= value;
		currentValue = Mathf.Clamp(CurrentValue, 0, maxValue);
	}

	public void Restore(int value)
	{
		currentValue += value;
		currentValue = Mathf.Clamp(CurrentValue, 0, maxValue);
	}

	public void FullRestore()
	{
		currentValue = maxValue;
	}

}
