using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{
    
    [SerializeField] Dictionary<string, CombatModule> entitiesInBattle = new Dictionary<string, CombatModule>();
    
    [SerializeField] List<string> entityQueue = new List<string>();
    [SerializeField] Transform middleOfBattlefied;

    Transform battlefieldCenter;

    enum BattleState
    {
        WaitingForAction,
        Action,
        WaitingForInput
    }

    BattleState state;

    public CombatModule currentAttacker;
    public List<CombatModule> currentTargets = new List<CombatModule>();
    
    void Awake()
    {
        battlefieldCenter = GameObject.Find("BattlefieldCenter").transform;
        
    }

    void Start()
    {
        var entities = FindObjectsOfType<CombatModule>();
        foreach (var entity in entities)
        {
            entity.battlefieldCenter = battlefieldCenter.position;
            entitiesInBattle.Add(entity.EntityName, entity);
        }

    }

    void OnEnable()
    {
        EventManager.StartListening(CombatEvents.ReadyForAction, QueueAction);
        EventManager.StartListening(CombatEvents.CombatAnimationFinished, ModifyEntityStatistics);
        EventManager.StartListening(CombatEvents.ActionCompleted, ActionCompleted);
        EventManager.StartListening(CombatEvents.EntityDied, EntityDeath);
        

        EventManager.StartListening(UIEvents.ActionLaunched, ProcessAction);
        EventManager.StartListening(UIEvents.AttackCanceled, CancelAttack);
    }

    void OnDisable()
    {
        EventManager.StopListening(CombatEvents.ReadyForAction, QueueAction);
        EventManager.StopListening(CombatEvents.CombatAnimationFinished, ModifyEntityStatistics);
        EventManager.StopListening(CombatEvents.ActionCompleted, ActionCompleted);
        EventManager.StopListening(CombatEvents.EntityDied, EntityDeath);

        EventManager.StopListening(UIEvents.ActionLaunched, ProcessAction);
        EventManager.StopListening(UIEvents.AttackCanceled, CancelAttack);
    }

    void Update()
    {
        switch (state)
        {
            case BattleState.WaitingForAction:
                if(entityQueue.Count > 0)
                {
                    currentAttacker = entitiesInBattle[entityQueue[0]];
                    state = BattleState.WaitingForInput;
                    EventManager.TriggerEvent(CombatEvents.SuspensionToggle, new CombatEventData());
                    EventManager.TriggerEvent(CombatEvents.PermitAction, new CombatEventData(entityQueue[0]));
                }
                break;
            case BattleState.Action:

                break;

            case BattleState.WaitingForInput:
                
                break;
        }
    }

    //UI -> action launched
    void ProcessAction(UIEventData data)
    {
        //Data
        string attackerID = data.id;
        List<string> targetIDs = data.targetsId;
        CombatAction action = data.action;

        
        CombatModule attacker = entitiesInBattle[attackerID];

        if (state == BattleState.WaitingForInput)
        {
            state = BattleState.Action;

            currentTargets.Clear();
            foreach (string targetID in targetIDs)
            {
                CombatModule target = entitiesInBattle[targetID];
                currentTargets.Add(target);
            }
        }

        Vector3 position = currentTargets[0].attackerPosition;
        attacker.ProcessAction(action, position);
    }

    void CancelAttack(UIEventData data)
    {
        
        string attackerID = data.id;
        //Shdould be the same but who knows ¯\_(ツ)_/¯ 
        if(attackerID == currentAttacker.EntityName)
        {
            
            entitiesInBattle[attackerID].CancelAttack();
        }
    }

    

    //entity -> combat animation finished
    void ModifyEntityStatistics(CombatEventData data)
    {
        CombatAction action = data.action;

        foreach (CombatModule target in currentTargets)
        {
            action.ModyfiStatistics(currentAttacker.stats, target.stats);

            float healthChange = action.GetHealthChange(currentAttacker.stats, target.stats);
            EventManager.TriggerEvent(CombatEvents.DamageDealt, new CombatEventData(target.EntityName, healthChange));
        }
    }

    //Entity -> queu Action
    void QueueAction(CombatEventData data)
    {
        //var readyEntity = entitiesInBattle.Find(entity => entity.entityName == entityName);
        entityQueue.Add(data.id);

    }

    
    //entity -> action completed
    void ActionCompleted(CombatEventData data)
    {
        currentAttacker = null;
        currentTargets.Clear();

        EventManager.TriggerEvent(CombatEvents.SuspensionToggle, new CombatEventData());
        state = BattleState.WaitingForAction;
        entityQueue.RemoveAt(0);

        
    }

    void EntityDeath(CombatEventData data)
    {
        
        CombatModule deadEntity = entitiesInBattle[data.id];
        if(deadEntity.IsCharacter == false)
        {
            deadEntity.gameObject.SetActive(false);

            entitiesInBattle.Remove(data.id);
            entityQueue.Remove(data.id);
        }
        
        
    }


}

