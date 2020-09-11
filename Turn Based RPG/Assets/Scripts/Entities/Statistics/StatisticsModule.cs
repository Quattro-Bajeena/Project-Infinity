using System.Collections;
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
	[SerializeField] Statistic force;
	[Space(5)]

	[Header("Skills")]
	[SerializeField] Statistic melee;
	[SerializeField] Statistic lighstaber;
	[SerializeField] Statistic composure;
	[SerializeField] Statistic selfDefence;
	[SerializeField] Statistic speed;
	[Space(5)]

	[Header("Resources")]
	[SerializeField] ActionResource actionPoints;
	[SerializeField] ActionResource forcePoints;


	[Header("Resistances")]
	[SerializeField] Statistic forceResistance;
	[SerializeField] Statistic plasmaResistance;
	[SerializeField] Statistic physicalResistance;
	[SerializeField] Statistic fireResistance;
	[SerializeField] Statistic electricityResistance;


	public enum Atribute
	{
		Force
	}

	public enum Skill
	{
		Melee,
		Lightsaber,
		SelfDefence,
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
		Force,
		Plasma,
		Physical,
		Fire,
		Electricity
	}


	public Dictionary<Atribute, Statistic> atributes = new Dictionary<Atribute, Statistic>();
	public Dictionary<Skill, Statistic> skills = new Dictionary<Skill, Statistic>();
	public Dictionary<DamageType, Statistic> resistances = new Dictionary<DamageType, Statistic>();
	public Dictionary<Resource, ActionResource> resources = new Dictionary<Resource, ActionResource>();
	

	public Entity Entity { get; private set; }

	void Awake()
	{
		Entity = gameObject.GetComponent<Entity>();
		currentHealth = maxHealth;

		atributes.Add(Atribute.Force, force);

		skills.Add(Skill.Composure, composure);
		skills.Add(Skill.Melee, melee);
		skills.Add(Skill.Lightsaber, lighstaber);
		skills.Add(Skill.SelfDefence, selfDefence);
		skills.Add(Skill.Speed, speed);

		resistances.Add(DamageType.Force, forceResistance);
		resistances.Add(DamageType.Physical, physicalResistance);
		resistances.Add(DamageType.Plasma, plasmaResistance);
		resistances.Add(DamageType.Electricity, electricityResistance);
		resistances.Add(DamageType.Fire, fireResistance);

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
		//block / evade

		int finalValue = Mathf.RoundToInt(value);

		currentHealth -= finalValue;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

		Entity.combat.ReceivedDamage(finalValue);
		EventManager.TriggerEvent(CombatEvents.HealthChange, new CombatEventData(Entity.Name, -finalValue));
	}

	public void Heal(float value)
	{
		value *= 1 + Random.Range(-0.1f, 0.1f);
		value = Mathf.Clamp(value, 1, 9999);

		int finalValue = Mathf.RoundToInt(value);

		currentHealth += finalValue;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

		EventManager.TriggerEvent(CombatEvents.HealthChange, new CombatEventData(Entity.Name, finalValue));
	}



	public void ModifySkillValue(Skill skill, StatisticModifier modifier)
	{
		skills[skill].AddModifier(modifier);
	}
}
