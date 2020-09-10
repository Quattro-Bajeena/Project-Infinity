using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class EntityStatistics
{
	public float health;
	public float maxHealth;

	public float mana;
	public float maxMana;

	public float actionPoints;
	public float maxActionPoints;


	public float attack;
	public float hit;
	public float defense;
	public float evade;
	public float magic;
	public float magicDefense;
	public float speed;
	

	public Dictionary<ElementalModifier.Element, float> resistances = new Dictionary<ElementalModifier.Element, float>();
	

	public EntityStatistics()
    {
		var elements = Enum.GetValues(typeof(ElementalModifier.Element)).Cast<ElementalModifier.Element>();
        foreach (var element in elements)
        {
			
			resistances.Add(element, 0f);
        }

		
    }

	public EntityStatistics(EntityStatistics oldStats)
    {
		health = oldStats.health;
		mana = oldStats.mana;
		actionPoints = oldStats.actionPoints;

		maxHealth = oldStats.maxHealth;
        maxMana = oldStats.maxMana;
		maxActionPoints = oldStats.maxActionPoints;
		

		attack = oldStats.attack;
		hit = oldStats.hit;
		defense = oldStats.defense;
		evade = oldStats.evade;
		magic = oldStats.magic;
		magicDefense = oldStats.magicDefense;
		speed = oldStats.speed;
		
		resistances = oldStats.resistances;

	}
}
