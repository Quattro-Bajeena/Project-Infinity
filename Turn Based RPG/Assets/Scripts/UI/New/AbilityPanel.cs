using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityPanel : MonoBehaviour
{
    [SerializeField] GameObject buttonTemplate;

    UIManagerTest uiManager;
    //List<GameObject> buttons = new List<GameObject>();
    UIPanel panelInfo;


    [SerializeField] List<CombatAction> testAbilities = new List<CombatAction>();
    [SerializeField] Color notEnoughResourceNameColor;
    [SerializeField] Color notEnoughResourceCostColor;
    [SerializeField] Color highlightColor;

    ScrollRect scrollRect;

    public void Initialize(UIManagerTest uiManager)
    {
        this.uiManager = uiManager;
        scrollRect = GetComponent<ScrollRect>();
        panelInfo = GetComponent<UIPanel>();

    }

    public void Create(EntityStatistics characterStats ,List<CombatAction> abilities)
    {
        panelInfo.ClearButtons();
       
            
        foreach (CombatAction ability in abilities)
        {
            GameObject newButton = Instantiate(buttonTemplate) as GameObject;
                
            panelInfo.AddButton(newButton);
            newButton.SetActive(true);

            bool enoughtResource = ability.isEnoughResource(characterStats);

            newButton.GetComponent<AbilityButton>().Initialize(ability, enoughtResource, this, notEnoughResourceNameColor, notEnoughResourceCostColor, highlightColor);
            newButton.transform.SetParent(scrollRect.content.transform, false);
        }
        
    }

    public void AbilityPicked(CombatAction ability)
    {
        uiManager.AbilityPicked(ability);
    }

    void OnDisable()
    {
           
    }

}
