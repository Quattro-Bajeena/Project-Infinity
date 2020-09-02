using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{
    
    public Dictionary<string, CombatScript> entitiesInBattle = new Dictionary<string, CombatScript>();
    
    public List<string> entityQueue = new List<string>();


    enum BattleState
    {
        WaitingForAction,
        Action,
        WaitingForInput
    }

    BattleState state;

    public CombatScript currentAttacker;
    public List<CombatScript> currentTargets = new List<CombatScript>();
    
    void Awake()
    {
        
    }

    void Start()
    {
        var entities = FindObjectsOfType<CombatScript>();
        foreach (var entity in entities)
        {
            entitiesInBattle.Add(entity.entityName, entity);
        }

    }

    void OnEnable()
    {
        EventManager.StartListening(CombatEvents.ReadyForAction, queueAction);
        EventManager.StartListening(CombatEvents.CombatAnimationFinished, modifyEntityStatistics);
        EventManager.StartListening(CombatEvents.ActionCompleted, actionCompleted);
        EventManager.StartListening(CombatEvents.EntityDied, entityDeath);
        

        EventManager.StartListening(UIEvents.ActionLaunched, processAction);
        EventManager.StartListening(UIEvents.AttackCanceled, cancelAttack);
    }

    void OnDisable()
    {
        EventManager.StopListening(CombatEvents.ReadyForAction, queueAction);
        EventManager.StopListening(CombatEvents.CombatAnimationFinished, modifyEntityStatistics);
        EventManager.StopListening(CombatEvents.ActionCompleted, actionCompleted);
        EventManager.StopListening(CombatEvents.EntityDied, entityDeath);

        EventManager.StopListening(UIEvents.ActionLaunched, processAction);
    }

    void Update()
    {
        switch (state)
        {
            case BattleState.WaitingForAction:
                if(entityQueue.Count > 0)
                {      
                    EventManager.TriggerEvent(CombatEvents.PermitAction, new CombatEventData(entityQueue[0]));
                    currentAttacker = entitiesInBattle[entityQueue[0]];
                    state = BattleState.WaitingForInput;
                }
                break;
            case BattleState.Action:

                break;

            case BattleState.WaitingForInput:
                
                break;
        }
    }

    //UI -> action launched
    void processAction(UIEventData data)
    {
        //Data
        string attackerID = data.id;
        List<string> targetIDs = data.targetsId;
        CombatAction action = data.action;

        CombatScript attacker = entitiesInBattle[attackerID];

        if (state == BattleState.WaitingForInput)
        {
            state = BattleState.Action;
            EventManager.TriggerEvent(CombatEvents.SuspensionToggle, new CombatEventData());

            foreach (string targetID in targetIDs)
            {
                CombatScript target = entitiesInBattle[targetID];
                currentTargets.Add(target);
            }
        }

        Vector3 position = entitiesInBattle[targetIDs[0]].gameObject.transform.position;
        attacker.processAction(action, position);
    }

    void cancelAttack(UIEventData data)
    {
        
        string attackerID = data.id;
        //Shdould be the same but who knows ¯\_(ツ)_/¯ 
        if(attackerID == currentAttacker.entityName)
        {
            
            entitiesInBattle[attackerID].cancelAttack();
        }
    }

    

    //entity -> combat animation finished
    void modifyEntityStatistics(CombatEventData data)
    {
        CombatAction action = data.action;

        foreach (CombatScript target in currentTargets)
        {
            action.modyfiStatistics(currentAttacker.stats, target.stats);

            float healthChange = action.getHealthChange(currentAttacker.stats, target.stats);
            EventManager.TriggerEvent(CombatEvents.DamageDealt, new CombatEventData(target.entityName, healthChange));
        }
    }

    //Entity -> queu Action
    void queueAction(CombatEventData data)
    {
        //var readyEntity = entitiesInBattle.Find(entity => entity.entityName == entityName);
        entityQueue.Add(data.id);
    }

    
    //entity -> action completed
    void actionCompleted(CombatEventData data)
    {
        currentAttacker = null;
        currentTargets.Clear();

        EventManager.TriggerEvent(CombatEvents.SuspensionToggle, new CombatEventData());
        state = BattleState.WaitingForAction;
        entityQueue.RemoveAt(0);
    }

    void entityDeath(CombatEventData data)
    {
        CombatScript deadEntity = entitiesInBattle[data.id];
        deadEntity.gameObject.SetActive(false);

        entitiesInBattle.Remove(data.id);
        entityQueue.Remove(data.id);
        
    }


}

