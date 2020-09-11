using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackBar : MonoBehaviour
{
    Slider slider;
    [SerializeField] CombatModule currentCharacter;


    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        EventManager.StartListening(UIEvents.AttackLaunched, updateAttackBar);
    }

    private void OnDisable()
    {
        EventManager.StopListening(UIEvents.AttackLaunched, updateAttackBar);
    }

    public void changeActiveCharacter(CombatModule newCharacter)
    {
        currentCharacter = newCharacter;
        slider.value = currentCharacter.Stats.resources[StatisticsModule.Resource.ActionPoints].CurrentPercentage;
    }

    public void ResetActiveCharacter()
    {
        currentCharacter = null;
        slider.value = 0;
    }

    void updateAttackBar(UIEventData data)
    {
        slider.value = currentCharacter.Stats.resources[StatisticsModule.Resource.ActionPoints].CurrentPercentage;
    }

}
