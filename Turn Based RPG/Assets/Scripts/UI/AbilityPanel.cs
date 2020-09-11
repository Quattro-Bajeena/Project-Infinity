using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AbilityPanel : MonoBehaviour
{
    [SerializeField] GameObject buttonTemplate;

    UIManager uiManager;
    //List<GameObject> buttons = new List<GameObject>();
    UIPanel panelInfo;

    [SerializeField] Color notEnoughResourceNameColor;
    [SerializeField] Color notEnoughResourceCostColor;
    [SerializeField] Color highlightColor;

    StatisticsModule currentCharacterStats;

    ScrollRect scrollRect;
    TextMeshProUGUI resourceTypeText;
    TextMeshProUGUI resourceAmountText;
    TextMeshProUGUI descriptionText;

    public void Initialize(UIManager uiManager)
    {
        this.uiManager = uiManager;
        scrollRect = GetComponent<ScrollRect>();
        panelInfo = GetComponent<UIPanel>();
        resourceTypeText = transform.Find("Resource/ResourceType").GetComponent<TextMeshProUGUI>();
        resourceAmountText = transform.Find("Resource/ResourceAmount").GetComponent<TextMeshProUGUI>();
        descriptionText = transform.Find("Description/DescriptionText").GetComponent<TextMeshProUGUI>();
    }

    public void Create(StatisticsModule characterStats ,List<CombatAction> abilities)
    {
        panelInfo.ClearButtons();
        currentCharacterStats = characterStats;
            
        foreach (CombatAction ability in abilities)
        {
            GameObject newButton = Instantiate(buttonTemplate) as GameObject;
                
            panelInfo.AddButton(newButton);
            newButton.SetActive(true);

            bool enoughtResource = ability.IsEnoughResource(characterStats);

            newButton.GetComponent<AbilityButton>().Initialize(ability, enoughtResource, this, notEnoughResourceNameColor, notEnoughResourceCostColor, highlightColor);
            newButton.transform.SetParent(scrollRect.content.transform, false);
        }
        
    }

    public void AbilityPicked(CombatAction ability)
    {
        uiManager.AbilityPicked(ability);

    }

    public void HighlightedButtonChanged(CombatAction ability)
	{

        resourceAmountText.text = $"{currentCharacterStats.resources[StatisticsModule.Resource.ForcePoints].CurrentValue}/{currentCharacterStats.resources[StatisticsModule.Resource.ForcePoints].MaxValue}";
        descriptionText.text = ability.description;
	}

}
