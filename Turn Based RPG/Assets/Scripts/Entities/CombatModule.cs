using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CombatModule : MonoBehaviour
{

    public string EntityName { get; set; }
    public Entity Entity { get; set; }
    public bool IsCharacter { get; set; }

    
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

    public float actionGauge;
    public EntityStatistics stats;


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
        EntityName = Entity.entityName;

        if (GetComponent<CharacterModule>())
        {
            IsCharacter = true;
        }
        else IsCharacter = false;

        //stats = entity.stats.getStatistics();
        stats = new EntityStatistics(Entity.stats.GetStatistics());

    }
    void Start()
    {
        List<CombatAction> availableActions = new List<CombatAction>();
        availableActions.AddRange(gameObject.GetComponentsInChildren<CombatAction>());
        foreach (CombatAction action in availableActions)
        {
            if(Entity.animationsOld_)
                Entity.animationsOld_.AddClip(action.actionName, action.actionAnimation);

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
        EventManager.StartListening(CombatEvents.DamageDealt, ClampHpMana);
        EventManager.StartListening(CombatEvents.ActionCompleted, HealthCheck);

        EventManager.StartListening(UIEvents.AttackMenuSelected, JumpOnBattlefield);
        EventManager.StartListening(UIEvents.AttackMenuCanceled, RetreatFromBattlefied);
    }

    void OnDisable()
    {
        EventManager.StopListening(CombatEvents.SuspensionToggle, SuspensionToggle);
        EventManager.StopListening(CombatEvents.DamageDealt, ClampHpMana);
        EventManager.StopListening(CombatEvents.ActionCompleted, HealthCheck);

        EventManager.StopListening(UIEvents.AttackMenuSelected, JumpOnBattlefield);
        EventManager.StopListening(UIEvents.AttackMenuCanceled, RetreatFromBattlefied);
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
                    EventManager.TriggerEvent(CombatEvents.ReadyForAction, new CombatEventData(EntityName));
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
        action.UseResourceStat(stats);
        switch (action.actionType)
        {
            case CombatAction.ActionType.Attack:
                QueueAttack(action);
                if(state == CombatState.ReadyForAction)
                {
                    state = CombatState.PerformingAction;
                    EventManager.TriggerEvent(CombatEvents.StartingAction, new CombatEventData(EntityName));

                    currentPerformingAction = PerformAttackAction(position);
                    StartCoroutine(currentPerformingAction);

                }
                break;

            case CombatAction.ActionType.Ability:
                if(state == CombatState.ReadyForAction)
                {
                    state = CombatState.PerformingAction;
                    EventManager.TriggerEvent(CombatEvents.StartingAction, new CombatEventData(EntityName));

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


    //Functions that respond to events

    void ClampHpMana(CombatEventData data)
    {
        stats.health = Mathf.Clamp(stats.health, 0, stats.maxHealth);
        stats.mana = Mathf.Clamp(stats.mana, 0, stats.maxMana);
    }



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

        if (stats.health <= 0)
        {
            //Play death animation
            //start courotine
            EventManager.TriggerEvent(CombatEvents.EntityDied, new CombatEventData(EntityName));
        }
    }

    

    void JumpOnBattlefield(UIEventData data)
    {
        if (data.id == EntityName)
        {


            StartCoroutine(MoveTowardsTargetCoroutine(battlefieldCenter, 10, 3f));
        }

    }

    void RetreatFromBattlefied(UIEventData data)
    {
        if (data.id == EntityName)
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
                EventManager.TriggerEvent(CombatEvents.ComboLaunched, new CombatEventData(EntityName));
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

                if(Entity.animationsOld_)
                    Entity.animationsOld_.Play(attackQueue.Peek().actionName);

                yield return null;
                while (Entity.animations.IsAnimationPlaying() == true) { yield return null; }
                

                EventManager.TriggerEvent(CombatEvents.CombatAnimationFinished, new CombatEventData(EntityName, attackQueue.Peek()));
                attackQueue.Dequeue();
            }
            //no attacks and attacking is canceled
            else if(attackCanceled == true)
            {
                break;
            }
            //No action points
            else if (stats.actionPoints == 0)
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
        while (MoveTowardsTarget(targetPosition, 5f)) { yield return null; }

        Entity.animationsOld_.Play(action.actionName); 
        while(Entity.animationsOld_.IsPlaying == true) { yield return null; }

        EventManager.TriggerEvent(CombatEvents.CombatAnimationFinished, new CombatEventData(EntityName, action));

        while (MoveTowardsTarget(startPosition, 5f)) { yield return null; }
        StartCoroutine(TurnInTime(startRotation, 0.3f));

        CompleteAction();
    }


    void CompleteAction()
    {
        attackCanceled = false;
        stats.actionPoints = stats.maxActionPoints;

        actionGauge = 0;
        attackComboList.Clear();


        EventManager.TriggerEvent(CombatEvents.ActionCompleted, new CombatEventData(EntityName));
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
        while (distance > 0.01f)
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
        float distance = Vector3.Distance(transform.position, targetGroundPosition);
        transform.LookAt(targetGroundPosition);

        //transform.rotation = Quaternion.LookRotation(new Vector3(target.x, transform.position.y, target.z) - transform.position);

        transform.position = Vector3.MoveTowards(transform.position, targetGroundPosition, speed * Time.deltaTime);
        return distance > 0.01f;
    }

    

    void UpdateActionGauge()
    {
        actionGauge += stats.speed / 100f * Time.deltaTime;
    }

    

}
