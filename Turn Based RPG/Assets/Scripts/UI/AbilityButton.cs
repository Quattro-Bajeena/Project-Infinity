using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{

    TextMeshProUGUI nameText;
    TextMeshProUGUI costText;


    AbilityPanel abilityPanel;
    CombatAction ability;

    bool enoughResource;

    Color nameDefaultColor;
    Color costDefaultColor;

    Color highlightColor;

    void Awake()
    {

        nameText = transform.Find("AbilityNameText").GetComponent<TextMeshProUGUI>();
        costText = transform.Find("AbilityCostText").GetComponent<TextMeshProUGUI>();

        nameDefaultColor = nameText.color;
        costDefaultColor = costText.color;
    }

    void OnEnable()
    {
        EventManager.StartListening(UIEvents.SelectedButtonChanged, ButtonHighlited);
    }
    void OnDisable()
    {
        EventManager.StopListening(UIEvents.SelectedButtonChanged, ButtonHighlited);
    }

    public void Initialize(CombatAction ability, bool enoughResource,  AbilityPanel abilityPanel, Color nameColor, Color costColor, Color highlightColor)
    {
        this.abilityPanel = abilityPanel;
        this.ability = ability;
        this.enoughResource = enoughResource;
        this.highlightColor = highlightColor;

        name = "Ability Button " + ability.actionName + " " + Math.Round( UnityEngine.Random.value, 2);

        if(enoughResource == false)
        {
            nameText.color = nameColor;
            costText.color = costColor;
            
        }

        nameDefaultColor = nameText.color;
        costDefaultColor = costText.color;

        nameText.text = ability.actionName;
        costText.text = ability.cost.ToString();

    }

    public void ButtonHighlited(UIEventData data)
    {
        
        if(data.buttonSelected == this.gameObject)
        {
            abilityPanel.HighlightedButtonChanged(ability);
            nameText.color *= highlightColor;
            costText.color *= highlightColor;
        }
        else
        {
            nameText.color = nameDefaultColor;
            costText.color = costDefaultColor;
        }
    }

    public void AbilityPicked()
    {
        if (enoughResource)
        {
            abilityPanel.AbilityPicked(ability);
        }

        
    }

    
}
