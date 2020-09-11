using System;
using System.Collections;
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
    public Vector3 attackerPosition;
    public Vector3 battlefieldCenter;

    [SerializeField][Range(0, 1f)] float actionGauge;
    public float ActionGauge { get { return actionGauge; } }

    public bool IsDefending { get; private set; }

    public List<CombatAction> abilities = new List<CombatAction>();
    public Dictionary<BaseAttackType, CombatAction> baseAttacks = new Dictionary<BaseAttackType, CombatAction>();
    List<CombatAction> combos = new List<CombatAction>();


    [SerializeField] List<BaseAttackType> attackComboList = new List<BaseAttackType>();
    Queue<CombatAction> attackQueue = new Queue<CombatAction>();
    [SerializeField] List<CombatAction> attackQueueList = new List<CombatAction>();
    bool attackCanceled = false;

    IEnumerator currentPerformingAction;
    
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
        List<CombatAction> availableActions = new List<CombatAction>();
        availableActions.AddRange(gameObject.GetComponentsInChildren<CombatAction>());
        foreach (CombatAction action in availableActions)
        {

            switch (action.actionType)
            {
                case CombatAction.ActionType.Ability:
                    abilities.Add(action);
                    break;
                case CombatAction.ActionType.Attack:
                    baseAttacks.Add(action.BaseAttackType, action);
                    break;
                case CombatAction.ActionType.Combo:
                    combos.Add(action);
                    break;
            }
        }
        
        

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
                if(actionGauge >= 1f) 
                {
                    EventManager.TriggerEvent(CombatEvents.ReadyForAction, new CombatEventData(Entity.Name));
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
    public void ProcessAction(CombatAction action, Vector3 position)
    {
        IsDefending = false;
        action.UseResourceStat(Entity.stats);
        switch (action.actionType)
        {
            case CombatAction.ActionType.Attack:
                QueueAttack(action);
                if(state == CombatState.ReadyForAction)
                {
                    state = CombatState.PerformingAction;
                    EventManager.TriggerEvent(CombatEvents.StartingAction, new CombatEventData(Entity.Name));

                    currentPerformingAction = PerformAttackAction(position);
                    StartCoroutine(currentPerformingAction);

                }
                break;

            case CombatAction.ActionType.Ability:
                if(state == CombatState.ReadyForAction)
                {
                    state = CombatState.PerformingAction;
                    EventManager.TriggerEvent(CombatEvents.StartingAction, new CombatEventData(Entity.Name));

                    currentPerformingAction = PerformAbilityAction(action, position);
                    StartCoroutine(currentPerformingAction);
                }
                break;
        }

    }

    public void CancelAttack()
    {
        attackCanceled = true;
        if(state == CombatState.ReadyForAction)
        {
            StartCoroutine(MoveTowardsTargetCoroutine(startPosition, 8f, 1f));
            CompleteAction();
        }
    }

    //functions accesed by entity modules

    public void ReceivedDamage(float value)
	{
        if(value > 1)
		{
            Entity.animations.ReceivedDamage();
		}
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
            EventManager.TriggerEvent(CombatEvents.EntityDied, new CombatEventData(Entity.Name));
        }
    }

    

    void JumpOnBattlefield(UIEventData data)
    {
        if (data.id == Entity.Name)
        {


            StartCoroutine(MoveTowardsTargetCoroutine(battlefieldCenter, 10, 3f));
        }

    }

    void RetreatFromBattlefied(UIEventData data)
    {
        if (data.id == Entity.Name)
        {
            StartCoroutine(MoveTowardsTargetCoroutine(startPosition, 10f, 3f));
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
                EventManager.TriggerEvent(CombatEvents.ComboLaunched, new CombatEventData(Entity.Name));
            }
        }

        attackQueue.Enqueue(action);
    }

    
    IEnumerator PerformAttackAction(Vector3 targetPosition)
    {
        Entity.animations.SetWalking(true);
        while (MoveTowardsTarget(targetPosition, 5f)) { yield return null; }
        Entity.animations.SetWalking(false);

        while (true)
        {
            //there are attack in the queue
            if (attackQueue.Count != 0)
            {
                Entity.animations.TriggerAttack(attackQueue.Peek().animationName);

                yield return null;
                while (Entity.animations.IsAnimationPlaying() == true) { yield return null; }
                

                EventManager.TriggerEvent(CombatEvents.CombatAnimationFinished, new CombatEventData(Entity.Name, attackQueue.Peek()));
                attackQueue.Dequeue();
            }
            //no attacks and attacking is canceled
            else if(attackCanceled == true)
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

        Entity.animations.SetWalking(true);
        while (MoveTowardsTarget(startPosition, 5f)) { yield return null; }
        Entity.animations.SetWalking(false);
        StartCoroutine(TurnInTime(startRotation, 0.3f));

        CompleteAction();
    }

    IEnumerator PerformAbilityAction(CombatAction action, Vector3 targetPosition)
    {
        if(action.IsTargetPositionStationary == false)
		{
            Entity.animations.SetWalking(true);
            while (MoveTowardsTarget(targetPosition, 5f)) { yield return null; }
            
            Entity.animations.SetWalking(false);
        }
        

        Entity.animations.PerformAbility(action.animationName);
        yield return null;
        while (Entity.animations.IsAnimationPlaying() == true) { yield return null; }
        
        EventManager.TriggerEvent(CombatEvents.CombatAnimationFinished, new CombatEventData(Entity.Name, action));

        Entity.animations.AbilityEnded();
        yield return new WaitForSeconds(0.5f);

        if (action.IsTargetPositionStationary == false)
		{
            Entity.animations.SetWalking(true);
            while (MoveTowardsTarget(startPosition, 5f)) { yield return null; }
            Entity.animations.SetWalking(false);
            StartCoroutine(TurnInTime(startRotation, 0.3f));
        }

        CompleteAction();
    }


    void CompleteAction()
    {
        attackCanceled = false;
        Entity.stats.resources[StatisticsModule.Resource.ActionPoints].FullRestore();

        actionGauge = 0;
        attackComboList.Clear();
        transform.position = startPosition;

        EventManager.TriggerEvent(CombatEvents.ActionCompleted, new CombatEventData(Entity.Name));
        state = CombatState.Charging;
    }



    

    IEnumerator PerformDeath()
    {
        yield return null;
    }

    

    IEnumerator TurnInTime(Quaternion targetRotation, float time)
	{
        Quaternion startRotation = transform.rotation;
        float percent = 0;
        while(percent < 1)
		{
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, percent);
            percent += Time.deltaTime / time;
            yield return null;

        }
        transform.rotation = targetRotation;
	}

    IEnumerator MoveTowardsTargetCoroutine(Vector3 target, float speed, float animationSpeed)
    {
        Entity.animations.SetWalking(true, animationSpeed);

        Vector3 targetGroundPosition = new Vector3(target.x, transform.position.y, target.z);
        float distance = Vector3.Distance(transform.position, targetGroundPosition);
        transform.LookAt(targetGroundPosition);
        while (distance > 0.02f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetGroundPosition, speed * Time.deltaTime);
            distance = Vector3.Distance(transform.position, targetGroundPosition);
            yield return null;
        }
        

        StartCoroutine(TurnInTime(startRotation, 0.1f));
        Entity.animations.SetWalking(false, animationSpeed);
    }

    bool MoveTowardsTarget(Vector3 target, float speed)
    {

        Vector3 targetGroundPosition = new Vector3(target.x, transform.position.y, target.z);
        transform.LookAt(targetGroundPosition);
        float distance = Vector3.Distance(transform.position, targetGroundPosition);
        

        //transform.rotation = Quaternion.LookRotation(new Vector3(target.x, transform.position.y, target.z) - transform.position);

        transform.position = Vector3.MoveTowards(transform.position, targetGroundPosition, speed * Time.deltaTime);
        return distance > 0.1f;
    }

    

    void UpdateActionGauge()
    {
        actionGauge += Entity.stats.skills[StatisticsModule.Skill.Speed].Value / 100f * Time.deltaTime;
    }

    void DefensePicked(UIEventData data)
	{
        if(data.id == Entity.Name)
		{
            IsDefending = true;
            Entity.animations.SetDefend(true);
            EventManager.TriggerEvent(CombatEvents.ActionCompleted, new CombatEventData(Entity.Name));
		}
	}

    

    

}
