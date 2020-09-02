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

    public string entityName { get; set; }
    public Entity entity { get; set; }
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
    public float actionGauge;
    public EntityStatistics stats;


    public List<CombatAction> abilities = new List<CombatAction>();
    public Dictionary<BaseAttackType, CombatAction> baseAttacks = new Dictionary<BaseAttackType, CombatAction>();
    List<CombatAction> combos = new List<CombatAction>();


    [SerializeField] List<BaseAttackType> attackComboList = new List<BaseAttackType>();
    Queue<CombatAction> attackQueue = new Queue<CombatAction>();
    bool attackCanceled = false;

    IEnumerator currentPerformingAction;
    
    void Awake()
    {
        startPosition = transform.position;
        entity = gameObject.GetComponent<Entity>();
        entityName = entity.entityName;

        if (GetComponent<CharacterModule>())
        {
            IsCharacter = true;
        }
        else IsCharacter = false;

        //stats = entity.stats.getStatistics();
        stats = new EntityStatistics(entity.stats.GetStatistics());

    }
    void Start()
    {
        List<CombatAction> availableActions = new List<CombatAction>();
        availableActions.AddRange(gameObject.GetComponentsInChildren<CombatAction>());
        foreach (CombatAction action in availableActions)
        {
            
            entity.animations.AddClip(action.actionName, action.actionAnimation);
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
        switch (state)
        {
            case CombatState.Charging:
                
                UpdateActionGauge();
                if(actionGauge >= 1f) 
                {
                    EventManager.TriggerEvent(CombatEvents.ReadyForAction, new CombatEventData(entityName));
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
                    EventManager.TriggerEvent(CombatEvents.StartingAction, new CombatEventData(entityName));

                    currentPerformingAction = PerformAttackAction(position);
                    StartCoroutine(currentPerformingAction);

                }
                break;

            case CombatAction.ActionType.Ability:
                if(state == CombatState.ReadyForAction)
                {
                    state = CombatState.PerformingAction;
                    EventManager.TriggerEvent(CombatEvents.StartingAction, new CombatEventData(entityName));

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
            StartCoroutine(MoveTowardsTargetCoroutine(startPosition, 0f, 10f));
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
            EventManager.TriggerEvent(CombatEvents.EntityDied, new CombatEventData(entityName));
        }
    }

    

    void JumpOnBattlefield(UIEventData data)
    {
        if (data.id == entityName)
        {
            //TO DO: get the position better, maybe a empty and field on battle manager
            Vector3 battlefiedPosition = new Vector3(
                transform.position.x - 3,
                transform.position.y,
                transform.position.z);

            StartCoroutine(MoveTowardsTargetCoroutine(battlefiedPosition, 0, 10));
        }

    }

    void RetreatFromBattlefied(UIEventData data)
    {
        if (data.id == entityName)
        {
            StartCoroutine(MoveTowardsTargetCoroutine(startPosition, 0f, 10f));
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
                EventManager.TriggerEvent(CombatEvents.ComboLaunched, new CombatEventData(entityName));
            }
        }

        attackQueue.Enqueue(action);
    }

    
    IEnumerator PerformAttackAction(Vector3 targetPosition)
    {

        while (MoveTowardsTarget(targetPosition, 1.5f, 5f)) { yield return null; }

        while (true)
        {
            //there are attack in the queue
            if (attackQueue.Count != 0)
            {
                entity.animations.Play(attackQueue.Peek().actionName);
                while (entity.animations.IsPlaying == true) { yield return null; }
                EventManager.TriggerEvent(CombatEvents.CombatAnimationFinished, new CombatEventData(entityName, attackQueue.Peek()));
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

        while (MoveTowardsTarget(startPosition, 0, 5f)) { yield return null; }
        CompleteAction();
    }

    IEnumerator PerformAbilityAction(CombatAction action, Vector3 targetPosition)
    {
        while (MoveTowardsTarget(targetPosition, 1.5f, 5f)) { yield return null; }

        entity.animations.Play(action.actionName);
        while(entity.animations.IsPlaying == true) { yield return null; }

        EventManager.TriggerEvent(CombatEvents.CombatAnimationFinished, new CombatEventData(entityName, action));

        while (MoveTowardsTarget(startPosition, 0, 5f)) { yield return null; }


        CompleteAction();
    }


    void CompleteAction()
    {
        attackCanceled = false;
        stats.actionPoints = stats.maxActionPoints;

        actionGauge = 0;
        attackComboList.Clear();

        entity.animations.PlayIdle();
        EventManager.TriggerEvent(CombatEvents.ActionCompleted, new CombatEventData(entityName));

        state = CombatState.Charging;
    }



    IEnumerator MoveTowardsTargetCoroutine(Vector3 target, float threshold, float speed)
    {
        float distance = Vector3.Distance(transform.position, target);

        while (distance > threshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            distance = Vector3.Distance(transform.position, target);
            yield return null;
        } 
        
    }

    IEnumerator PerformDeath()
    {
        yield return null;
    }

    bool MoveTowardsTarget(Vector3 target, float threshold, float speed)
    {
        float distance = Vector3.Distance(transform.position, target);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        return distance > threshold;
    }

    

    void UpdateActionGauge()
    {
        actionGauge += stats.speed / 100f * Time.deltaTime;
    }

    

}
