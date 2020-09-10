using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAIModule : MonoBehaviour
{

    Entity entity;
    List<CombatModule> entitiesInBattle = new List<CombatModule>();


    enum Goals
    {
        Attack,
        AttackPowerful,
        AttackAll,
        HealItself,
        HealAlly
    }

    Queue<Goals> goals;

    void Awake()
    {
        entity = GetComponent<Entity>();
    }
    private void Start()
    {
        entitiesInBattle.AddRange(FindObjectsOfType<CombatModule>());
    }

    private void OnEnable()
    {
        EventManager.StartListening(CombatEvents.PermitAction, LaunchAbility);
    }

    private void OnDisable()
    {
        EventManager.StopListening(CombatEvents.PermitAction, LaunchAbility);
    }



    void DecideGoals()
    {

    }

    void PickAction()
    {

    }

    void LaunchAbility(CombatEventData data)
    {
        if(data.id == entity.entityName)
        {
            DecideGoals();
            PickAction();

            int index = Random.Range(0, entity.combat.abilities.Count - 1);

            CombatAction ability = entity.combat.abilities[index];

            List<string> potentialTargets = ability.GetTargets(entity.entityName, false, entitiesInBattle);

            if(ability.actionRange == CombatAction.ActionRange.Single)
			{
                index = Random.Range(0, potentialTargets.Count - 1);
                string target = potentialTargets[index];
                EventManager.TriggerEvent(UIEvents.ActionLaunched, new UIEventData(entity.entityName, new List<string>() { target }, ability));
            }
            else if(ability.actionRange == CombatAction.ActionRange.All)
			{
                EventManager.TriggerEvent(UIEvents.ActionLaunched, new UIEventData(entity.entityName, new List<string>(potentialTargets), ability));
            }
            


            
        }
    }
}
