using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CombatScript : MonoBehaviour
{

    public string entityName { get; set; }
    public EntityScript entity { get; set; }
    public bool isCharacter { get; set; }

    
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
        entity = gameObject.GetComponent<EntityScript>();
        entityName = entity.entityName;

        if (GetComponent<CharacterScript>())
        {
            isCharacter = true;
        }
        else isCharacter = false;

        //stats = entity.stats.getStatistics();
        stats = new EntityStatistics(entity.stats.getStatistics());

    }
    void Start()
    {
        List<CombatAction> availableActions = new List<CombatAction>();
        availableActions.AddRange(gameObject.GetComponentsInChildren<CombatAction>());
        foreach (CombatAction action in availableActions)
        {
            
            entity.animations.addClip(action.actionName, action.actionAnimation);
            switch (action.actionType)
            {
                case CombatAction.ActionType.Ability:
                    abilities.Add(action);
                    break;
                case CombatAction.ActionType.Attack:
                    baseAttacks.Add(action.baseAttackType, action);
                    break;
                case CombatAction.ActionType.Combo:
                    combos.Add(action);
                    break;
            }
        }
        
        

    }

    void OnEnable()
    {
        EventManager.StartListening(CombatEvents.SuspensionToggle, suspensionToggle);
        EventManager.StartListening(CombatEvents.DamageDealt, clampHpMana);
        EventManager.StartListening(CombatEvents.ActionCompleted, healthCheck);

        EventManager.StartListening(UIEvents.AttackMenuSelected, JumpOnBattlefield);
        EventManager.StartListening(UIEvents.AttackMenuCanceled, RetreatFromBattlefied);
    }

    void OnDisable()
    {
        EventManager.StopListening(CombatEvents.SuspensionToggle, suspensionToggle);
        EventManager.StopListening(CombatEvents.ActionCompleted, healthCheck);
        EventManager.StopListening(CombatEvents.ActionCompleted, healthCheck);

        EventManager.StopListening(UIEvents.AttackMenuSelected, JumpOnBattlefield);
        EventManager.StopListening(UIEvents.AttackMenuCanceled, RetreatFromBattlefied);
    }

    
    void Update()
    {
        switch (state)
        {
            case CombatState.Charging:
                
                updateActionGauge();
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

    public void processAction(CombatAction action, Vector3 position)
    {
        action.useResourceStat(stats);
        switch (action.actionType)
        {
            case CombatAction.ActionType.Attack:
                queueAttack(action);
                if(state == CombatState.ReadyForAction)
                {
                    state = CombatState.PerformingAction;
                    EventManager.TriggerEvent(CombatEvents.StartingAction, new CombatEventData(entityName));

                    currentPerformingAction = performAttackAction(position);
                    StartCoroutine(currentPerformingAction);

                }
                break;

            case CombatAction.ActionType.Ability:
                if(state == CombatState.ReadyForAction)
                {
                    state = CombatState.PerformingAction;
                    EventManager.TriggerEvent(CombatEvents.StartingAction, new CombatEventData(entityName));

                    currentPerformingAction = performAbilityAction(action, position);
                    StartCoroutine(currentPerformingAction);
                }
                break;
        }

    }

    public void cancelAttack()
    {
        attackCanceled = true;
        if(state == CombatState.ReadyForAction)
        {
            completeAction();
        }
    }

    void queueAttack(CombatAction action)
    {

        attackComboList.Add(action.baseAttackType);
        
        //Check if theres combo if yes do it!
        foreach (CombatAction combo in combos)
        {
            if (attackComboList.SequenceEqual(combo.comboInput) == true)
            {
                
                action = combo;
                cancelAttack();
                EventManager.TriggerEvent(CombatEvents.ComboLaunched, new CombatEventData(entityName));
            }
        }

        attackQueue.Enqueue(action);
    }

    
    
    

    IEnumerator performAttackAction(Vector3 targetPosition)
    {

        while (MoveTowardsTarget(targetPosition, 1.5f, 5f)) { yield return null; }

        while (true)
        {
            //there are attack in the queue
            if (attackQueue.Count != 0)
            {
                entity.animations.play(attackQueue.Peek().actionName);
                while (entity.animations.isPlaying == true) { yield return null; }
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
        completeAction();
    }

    IEnumerator performAbilityAction(CombatAction action, Vector3 targetPosition)
    {
        while (MoveTowardsTarget(targetPosition, 1.5f, 5f)) { yield return null; }

        entity.animations.play(action.actionName);
        while(entity.animations.isPlaying == true) { yield return null; }

        EventManager.TriggerEvent(CombatEvents.CombatAnimationFinished, new CombatEventData(entityName, action));

        while (MoveTowardsTarget(startPosition, 0, 5f)) { yield return null; }


        completeAction();
    }


    void completeAction()
    {
        attackCanceled = false;
        stats.actionPoints = stats.maxActionPoints;

        actionGauge = 0;
        attackComboList.Clear();

        entity.animations.playIdle();
        EventManager.TriggerEvent(CombatEvents.ActionCompleted, new CombatEventData(entityName));
        
        state = CombatState.Charging;
    }

    void JumpOnBattlefield(UIEventData data)
    {
        if(data.id == entityName)
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
        if(data.id == entityName)
        {
            StartCoroutine(MoveTowardsTargetCoroutine(startPosition, 0f, 10f));
        }
        
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

    bool MoveTowardsTarget(Vector3 target, float threshold, float speed)
    {
        float distance = Vector3.Distance(transform.position, target);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        return distance > threshold;
    }

    void clampHpMana(CombatEventData data)
    {
        stats.health = Mathf.Clamp(stats.health, 0, stats.maxHealth);
        stats.mana = Mathf.Clamp(stats.mana, 0, stats.maxMana);
    }

    void healthCheck(CombatEventData data)
    {

        if (stats.health <= 0)
        {
            //Play death animation
            //start courotine
            EventManager.TriggerEvent(CombatEvents.EntityDied, new CombatEventData(entityName));
        }
    }

    IEnumerator performDeath()
    {
        yield return null;
    }

    void suspensionToggle(CombatEventData data)
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

    void updateActionGauge()
    {
        actionGauge += stats.speed / 100f * Time.deltaTime;
    }

    

}
