using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackBar : MonoBehaviour
{
    Slider slider;
    [SerializeField] CombatScript currentCharacter;


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

    public void changeActiveCharacter(CombatScript newCharacter)
    {
        currentCharacter = newCharacter;
        slider.value = currentCharacter.stats.actionPoints / currentCharacter.stats.maxActionPoints;
    }

    public void ResetActiveCharacter()
    {
        currentCharacter = null;
        slider.value = 0;
    }

    void updateAttackBar(UIEventData data)
    {
        slider.value = currentCharacter.stats.actionPoints / currentCharacter.stats.maxActionPoints;
    }

}
