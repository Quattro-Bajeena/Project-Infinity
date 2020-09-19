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
        if(data.id == entity.Id)
        {
            DecideGoals();
            PickAction();

			
            int index = Random.Range(0, entity.combat.Abilities.Count - 1);

            CombatAction ability = entity.combat.Abilities[index];

            if (ability.IsEnoughResource(entity.stats))
            {
                List<string> potentialTargets = ability.GetTargets(entity.Id, false, entitiesInBattle);

                if (ability.actionRange == CombatAction.Range.Single)
                {
                    index = Random.Range(0, potentialTargets.Count - 1);
                    string target = potentialTargets[index];
                    EventManager.TriggerEvent(UIEvents.ActionLaunched, new UIEventData(entity.Id, new List<string>() { target }, ability));
                }
                else if (ability.actionRange == CombatAction.Range.All)
                {
                    EventManager.TriggerEvent(UIEvents.ActionLaunched, new UIEventData(entity.Id, new List<string>(potentialTargets), ability));
                }
            }
            else EventManager.TriggerEvent(CombatEvents.ActionCompleted, new CombatEventData(entity.Id));

            
            


            
        }
    }
}
