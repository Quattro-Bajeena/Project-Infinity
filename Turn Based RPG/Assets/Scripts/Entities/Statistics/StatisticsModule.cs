using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsModule : MonoBehaviour
{
	[Range(0, 1000)] 
	public int maxHealth;
	public int currentHealth { get; private set; }

	public int maxActionPoints;
	public int currentActionPoints { get; private set; }

	public Statistic force;

	public Statistic melee;
	public Statistic lighstaber;
	public Statistic composure;
	public Statistic selfDefence;
	public Statistic speed;



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

	
	public Dictionary<Atribute, Statistic> atributes = new Dictionary<Atribute, Statistic>();
	public Dictionary<Skill, Statistic> skills = new Dictionary<Skill, Statistic>();

	[SerializeField]
	private EntityStatistics baseStats;

	public Entity Entity { get; private set; }

	void Awake()
	{
		Entity = gameObject.GetComponent<Entity>();
		currentHealth = maxHealth;

		skills.Add(Skill.Composure, composure);
		skills.Add(Skill.Melee, melee);
		skills.Add(Skill.Lightsaber, lighstaber);
		skills.Add(Skill.SelfDefence, selfDefence);
		skills.Add(Skill.Speed, speed);

		atributes.Add(Atribute.Force, force);
	}

	public bool IsDead
	{
		get
		{
			return currentHealth <= 0;
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


		currentHealth -= Mathf.RoundToInt(value);
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

		EventManager.TriggerEvent(CombatEvents.HealthChange, new CombatEventData(Entity.entityName, -value));
	}

	public void Heal(float value)
	{
		value *= 1 + Random.Range(-0.1f, 0.1f);
		value = Mathf.Clamp(value, 1, 9999);

		currentHealth += Mathf.RoundToInt(value);
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

		EventManager.TriggerEvent(CombatEvents.HealthChange, new CombatEventData(Entity.entityName, value));
	}

	public void ModifySkillValue(Skill skill, StatisticModifier modifier)
	{
		skills[skill].AddModifier(modifier);
	}

	public EntityStatistics GetStatistics()
    {
		return baseStats;
    }
}
