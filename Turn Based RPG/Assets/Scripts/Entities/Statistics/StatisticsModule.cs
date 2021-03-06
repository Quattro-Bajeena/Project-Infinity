﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatisticsModule : MonoBehaviour
{
	[SerializeField][Range(0, 1000)] 
	int maxHealth;
	public int MaxHealth
	{
		get{return maxHealth;}
	}
	[SerializeField] int currentHealth;
	public int CurrentHealth 
	{
		get { return currentHealth; }
	}

	[Header("Atributes")]
	[SerializeField] Statistic fitness;
	[SerializeField] Statistic inteligence;
	[SerializeField] Statistic perception;
	[SerializeField] Statistic force;
	[Space(5)]

	[Header("Skills")]
	[SerializeField] Statistic melee;
	[SerializeField] Statistic selfDefence;
	[Space(1)]
	[SerializeField] Statistic guns;
	[SerializeField] Statistic analysis;
	[Space(1)]
	[SerializeField] Statistic computers;
	[SerializeField] Statistic tinkering;
	[Space(1)]
	[SerializeField] Statistic lighstaber;
	[SerializeField] Statistic composure;
	[Space(1)]
	[SerializeField] Statistic speed;
	[Space(5)]

	[Header("Resources")]
	[SerializeField] ActionResource actionPoints;
	[SerializeField] ActionResource forcePoints;
	[Space(5)]

	[Header("Defences")]
	[SerializeField] Statistic physicalDefence;
	[SerializeField] Statistic lighsaberDefence;
	[SerializeField] Statistic blasterDefence;
	[SerializeField] Statistic techDefence;
	[SerializeField] Statistic forceDefence;
	[Space(5)]

	[Header("Resistances")]
	[SerializeField] Statistic fireResistance;
	[SerializeField] Statistic iceResistance;
	[SerializeField] Statistic electricityResistance;
	[SerializeField] Statistic lightSideResistance;
	[SerializeField] Statistic darkSideResistance;


	public enum Atribute
	{
		Fitness,
		Perception,
		Inteligence,
		Force
	}

	public enum Skill
	{
		Melee,
		Guns,
		Analysis,
		Computers,
		Tinkering,
		SelfDefence,
		Lightsaber,
		Composure,
		Speed
	}

	public enum Resource
	{
		ActionPoints,
		ForcePoints,
		OtherPoints
	}

	public enum DamageType
	{
		Physical,
		Lightsaber,
		Blaster,
		Tech,
		Force
	}

	public enum Elements
	{
		Fire,
		Ice,
		Electricity,
		LightSide,
		DarkSide
	}




	public Dictionary<Atribute, Statistic> atributes = new Dictionary<Atribute, Statistic>();
	public Dictionary<Skill, Statistic> skills = new Dictionary<Skill, Statistic>();
	public Dictionary<DamageType, Statistic> defenses = new Dictionary<DamageType, Statistic>();
	public Dictionary<Elements, Statistic> resistances = new Dictionary<Elements, Statistic>();
	public Dictionary<Resource, ActionResource> resources = new Dictionary<Resource, ActionResource>();
	

	public Entity Entity { get; private set; }

	void Awake()
	{
		Entity = gameObject.GetComponent<Entity>();
		currentHealth = maxHealth;

		atributes.Add(Atribute.Fitness, fitness);
		atributes.Add(Atribute.Perception, perception);
		atributes.Add(Atribute.Inteligence, inteligence);
		atributes.Add(Atribute.Force, force);

		skills.Add(Skill.Melee, melee);
		skills.Add(Skill.SelfDefence, selfDefence);
		skills.Add(Skill.Guns, guns);
		skills.Add(Skill.Analysis, analysis);
		skills.Add(Skill.Computers, computers);
		skills.Add(Skill.Tinkering, tinkering);
		skills.Add(Skill.Lightsaber, lighstaber);
		skills.Add(Skill.Composure, composure);
		skills.Add(Skill.Speed, speed);

		defenses.Add(DamageType.Force, forceDefence);
		defenses.Add(DamageType.Lightsaber, lighsaberDefence);
		defenses.Add(DamageType.Physical, physicalDefence);
		defenses.Add(DamageType.Blaster, blasterDefence);
		defenses.Add(DamageType.Tech, techDefence);

		resistances.Add(Elements.Fire, fireResistance);
		resistances.Add(Elements.Ice, iceResistance);
		resistances.Add(Elements.Electricity, electricityResistance);
		resistances.Add(Elements.LightSide, lightSideResistance);
		resistances.Add(Elements.DarkSide, darkSideResistance);

		resources.Add(Resource.ActionPoints, actionPoints);
		resources.Add(Resource.ForcePoints, forcePoints);
		actionPoints.FullRestore();
		forcePoints.FullRestore();
	}

	public bool IsDead
	{
		get
		{
			return CurrentHealth <= 0;
		}
	}

	public void ApplyDamage(float value)
	{
		//Random change
		//Clamp value
		//Chance for block or evade based on stats -> public method in animation module
		//Apply
		//Clamp health

		value *= 1 + Random.Range(-0.1f, 0.1f);

		value = Mathf.Clamp(value, 1, 9999);
		int finalValue = Mathf.RoundToInt(value);

		currentHealth -= finalValue;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

		Entity.combat.ReceivedDamage(finalValue);
		EventManager.TriggerEvent(CombatEvents.HealthChange, new CombatEventData(Entity.Id, -finalValue));
	}

	public void Heal(float value)
	{
		value *= 1 + Random.Range(-0.1f, 0.1f);
		value = Mathf.Clamp(value, 1, 9999);

		int finalValue = Mathf.RoundToInt(value);

		currentHealth += finalValue;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

		EventManager.TriggerEvent(CombatEvents.HealthChange, new CombatEventData(Entity.Id, finalValue));
	}



	public void ModifySkillValue(Skill skill, StatisticModifier modifier)
	{
		skills[skill].AddModifier(modifier);
	}
}
