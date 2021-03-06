﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CombatModule : MonoBehaviour
{
	public Entity Entity { get; set; }
	public bool IsCharacter { get; set; }
	public StatisticsModule Stats
	{
		get { return Entity.stats; }
	}


	enum CombatState
	{
		Charging,
		Suspension,
		ReadyForAction,
		PerformingAction
	}

	[SerializeField] CombatState state;

	//Statistics

	Vector3 startPosition;
	Quaternion startRotation;
	public Vector3 attackerPosition { get; private set; }
	public Vector3 battlefieldCenter { get; set; }
	public Vector3 enemiesPosition { get; set; }

	[SerializeField] [Range(0, 1f)] float actionGauge;
	public float ActionGauge { get { return actionGauge; } }

	[SerializeField] bool _defend = false;
	public bool IsDefending
	{
		get { return _defend; }
		private set { _defend = value; Entity.animations.SetDefend(value); }
	}

	bool blockedAttack = false;

	List<CombatAction> entityAbilities = new List<CombatAction>();
	List<CombatAction> weaponAbilities;
	public List<CombatAction> Abilities
	{ 
		get { return new List<CombatAction>(entityAbilities.Concat(weaponAbilities)); }
	}
	public Dictionary<BaseAttackType, CombatAction> BaseAttacks { get; private set; } = new Dictionary<BaseAttackType, CombatAction>();
	List<CombatAction> combos = new List<CombatAction>();


	[SerializeField] List<BaseAttackType> attackComboList = new List<BaseAttackType>();
	Queue<CombatAction> attackQueue = new Queue<CombatAction>();
	[SerializeField] List<CombatAction> attackQueueList = new List<CombatAction>();
	bool attackCanceled = false;


	void Awake()
	{

		startPosition = transform.position;
		startRotation = transform.rotation;
		attackerPosition = transform.Find("AttackerPosition").transform.position;

		Entity = gameObject.GetComponent<Entity>();


		if (GetComponent<CharacterModule>())
		{
			IsCharacter = true;
		}
		else IsCharacter = false;

		

	}
	void Start()
	{
		entityAbilities = Entity.abilities.Abilities;
	}

	void OnEnable()
	{
		EventManager.StartListening(CombatEvents.SuspensionToggle, SuspensionToggle);
		EventManager.StartListening(CombatEvents.ActionCompleted, HealthCheck);

		EventManager.StartListening(UIEvents.AttackMenuSelected, JumpOnBattlefield);
		EventManager.StartListening(UIEvents.AttackMenuCanceled, RetreatFromBattlefied);
		EventManager.StartListening(UIEvents.DefencePicked, DefensePicked);
	}

	void OnDisable()
	{
		EventManager.StopListening(CombatEvents.SuspensionToggle, SuspensionToggle);
		EventManager.StopListening(CombatEvents.ActionCompleted, HealthCheck);

		EventManager.StopListening(UIEvents.AttackMenuSelected, JumpOnBattlefield);
		EventManager.StopListening(UIEvents.AttackMenuCanceled, RetreatFromBattlefied);
		EventManager.StopListening(UIEvents.DefencePicked, DefensePicked);
	}


	void Update()
	{
		attackQueueList = attackQueue.ToList();

		switch (state)
		{
			case CombatState.Charging:

				UpdateActionGauge();
				if (actionGauge >= 1f)
				{
					EventManager.TriggerEvent(CombatEvents.ReadyForAction, new CombatEventData(Entity.Id));
					IsDefending = false;
					state = CombatState.ReadyForAction;
				}
				break;

			case CombatState.Suspension:

				break;

			case CombatState.ReadyForAction:

				break;

			case CombatState.PerformingAction:

				break;
		}
	}

	//Functions directly accesed by Battle Manager
	public void ProcessAction(CombatAction action, Vector3 movePosition, Vector3 lookPosition)
	{

		action.UseResourceStat(Entity.stats);
		switch (action.actionType)
		{
			case CombatAction.Type.Attack:
				QueueAttack(action);
				if (state == CombatState.ReadyForAction)
				{
					state = CombatState.PerformingAction;
					EventManager.TriggerEvent(CombatEvents.StartingAction, new CombatEventData(Entity.Id));

					StartCoroutine(PerformAttackAction(movePosition, lookPosition));

				}
				break;

			case CombatAction.Type.Ability:
				if (state == CombatState.ReadyForAction)
				{
					state = CombatState.PerformingAction;
					EventManager.TriggerEvent(CombatEvents.StartingAction, new CombatEventData(Entity.Id));

					StartCoroutine(PerformAbilityAction(action, movePosition, lookPosition));
				}
				break;
		}

	}



	public void CancelAttack()
	{
		attackCanceled = true;
		if (state == CombatState.ReadyForAction)
		{
			Entity.movement.ReturnToDefault(2);
			CompleteAction();
		}
	}

	//functions accesed by entity modules --
	public void ChangeWeapon(List<CombatAction> newAbilities, Dictionary<BaseAttackType, CombatAction> newAttacks, List<CombatAction> newCombos)
	{
		weaponAbilities = newAbilities;
		BaseAttacks = newAttacks;
		combos = newCombos;
	}

	public void ReceivedDamage(float value)
	{
		if (IsDefending == true || blockedAttack == true)
		{
			Entity.animations.BlockedAttack();
		}
		else if (value > 1)
		{
			Entity.animations.ReceivedDamage();
		}
		blockedAttack = false;
	}

	public void BlockedAttack()
	{
		blockedAttack = true;
	}

	public void AvoidedAttack()
	{
		Entity.animations.AvoidedAttack();
	}



	//Functions that respond to events
	void SuspensionToggle(CombatEventData data)
	{

		if (state == CombatState.Charging)
		{
			state = CombatState.Suspension;
		}
		else if (state == CombatState.Suspension)
		{
			state = CombatState.Charging;
		}
	}

	void HealthCheck(CombatEventData data)
	{

		if (Entity.stats.CurrentHealth <= 0)
		{
			//Play death animation
			//start courotine
			EventManager.TriggerEvent(CombatEvents.EntityDied, new CombatEventData(Entity.Id));
		}
	}



	void JumpOnBattlefield(UIEventData data)
	{
		if (data.id == Entity.Id)
		{
			Entity.movement.MoveToTarget(battlefieldCenter, enemiesPosition, 2);
		}

	}

	void RetreatFromBattlefied(UIEventData data)
	{
		if (data.id == Entity.Id)
		{

			Entity.movement.ReturnToDefault(2);

		}
	}



	//Internal functions
	void QueueAttack(CombatAction action)
	{

		attackComboList.Add(action.BaseAttackType);

		//Check if theres combo if yes do it!
		foreach (CombatAction combo in combos)
		{
			if (attackComboList.SequenceEqual(combo.ComboInput) == true)
			{

				action = combo;
				CancelAttack();
				EventManager.TriggerEvent(CombatEvents.ComboLaunched, new CombatEventData(Entity.Id));
			}
		}

		attackQueue.Enqueue(action);
	}


	IEnumerator PerformAttackAction(Vector3 targetPosition, Vector3 lookPosition)
	{

		Entity.movement.MoveToTarget(targetPosition, lookPosition);
		while (Entity.movement.IsMoving == true) { yield return null; }


		while (true)
		{
			//there are attack in the queue
			if (attackQueue.Count != 0)
			{
				if (attackQueue.Peek().actionType == CombatAction.Type.Attack)
					Entity.animations.PerformAttack(attackQueue.Peek().BaseAttackType);
				else if(attackQueue.Peek().actionType == CombatAction.Type.Combo)
					Entity.animations.PerformCombo(attackQueue.Peek().animationClip);

				yield return null;

				while (Entity.animations.IsAnimationPlaying() == true) { yield return null; }


				EventManager.TriggerEvent(CombatEvents.CombatAnimationFinished, new CombatEventData(Entity.Id, attackQueue.Peek()));
				attackQueue.Dequeue();
				yield return new WaitForSeconds(0.1f);
			}
			//no attacks and attacking is canceled
			else if (attackCanceled == true)
			{
				break;
			}
			//No action points
			else if (Entity.stats.resources[StatisticsModule.Resource.ActionPoints].CurrentValue == 0)
			{
				break;
			}
			//no attacks but still some Action Points
			else
			{
				yield return null;
			}

		}

		Entity.animations.CancelAttack();
		yield return new WaitForSeconds(0.5f);


		Entity.movement.ReturnToDefault(1.5f);
		while (Entity.movement.IsMoving == true) { yield return null; }


		CompleteAction();
	}

	IEnumerator PerformAbilityAction(CombatAction action, Vector3 targetPosition, Vector3 lookPosition)
	{
		if (action.IsTargetPositionStationary == false)
		{
			Entity.movement.MoveToTarget(targetPosition, lookPosition, 1.5f);
			while (Entity.movement.IsMoving == true) { yield return null; }
		}


		Entity.animations.PerformAbility(action.animationClip, weaponAbilities.Contains(action) );
		yield return null;
		while (Entity.animations.IsAnimationPlaying() == true) { yield return null; }

		EventManager.TriggerEvent(CombatEvents.CombatAnimationFinished, new CombatEventData(Entity.Id, action));

		Entity.animations.AbilityEnded();
		yield return new WaitForSeconds(0.5f);

		if (action.IsTargetPositionStationary == false)
		{
			Entity.movement.ReturnToDefault(1.5f);
			while (Entity.movement.IsMoving == true) { yield return null; }
		}

		CompleteAction();
	}


	void CompleteAction()
	{
		attackCanceled = false;
		Entity.stats.resources[StatisticsModule.Resource.ActionPoints].FullRestore();

		//TO DO: change to sometinh like reset becasue both hands may not be default but not reall cos it doesnt do anything if there are no tarrgets...
		Entity.weapon.OnWeaponBothHands();

		actionGauge = 0;
		attackComboList.Clear();
		transform.position = startPosition;

		EventManager.TriggerEvent(CombatEvents.ActionCompleted, new CombatEventData(Entity.Id));
		state = CombatState.Charging;
	}





	IEnumerator PerformDeath()
	{
		yield return null;
	}


	void UpdateActionGauge()
	{
		actionGauge += Entity.stats.skills[StatisticsModule.Skill.Speed].Value / 100f * Time.deltaTime;
	}

	void DefensePicked(UIEventData data)
	{
		if (data.id == Entity.Id)
		{

			IsDefending = true;
			CompleteAction();
		}
	}






}
