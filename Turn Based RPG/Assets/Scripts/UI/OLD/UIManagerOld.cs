using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManagerOld : MonoBehaviour
{
    //Prefabs
    [SerializeField] GameObject characterBarPrefab;
    [SerializeField] GameObject targetButtonPrefab;
    [SerializeField] GameObject selectorPrefab;
    [SerializeField] GameObject abilityButtonPrefab;
    [SerializeField] GameObject actionPanelPrefab;
    [SerializeField] GameObject damageTextPrefab;

    //Menus to open/close
    [SerializeField] GameObject movePanel;
    [SerializeField] GameObject enemiesListPanel;
    [SerializeField] GameObject attackPanel;

    //Pnales to attach things
    [SerializeField] Transform characterPanel;
    [SerializeField] Transform enemyPanel;
    

    Dictionary<string, GameObject> selectors = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> characterBars = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> magicPanels = new Dictionary<string, GameObject>();


    Dictionary<string, GameObject> enemyBars = new Dictionary<string, GameObject>();
    //Dictionary<string, GameObject> attackPanels = new Dictionary<string, GameObject>();




    enum MenuState
    {
        Idle,
        PickingMove,
        PickingAction,
        PickingTarget,
        PerformingAbility,
        PerformingAttack
    }
    MenuState menuState;

    [SerializeField]bool pickingAttack = false;


    [SerializeField] string currentPicker;
    [SerializeField] CombatAction currentAction;
    [SerializeField] string currentAttackTarget;


    [SerializeField]  List<CombatModule> entitiesInBattle = new List<CombatModule>();
    Dictionary<string, CombatModule> charactersInBattleDict = new Dictionary<string, CombatModule>();


    Stack<GameObject> menuLayers = new Stack<GameObject>();


    void Start()
    {

        enemiesListPanel.SetActive(true);
        movePanel.SetActive(false);
        attackPanel.SetActive(false);

        var entities = FindObjectsOfType<CombatModule>();

        List<CombatModule> charactersInBattle = new List<CombatModule>();
        List<string> enemiesInBattle = new List<string>();

        foreach (CombatModule entity in entities)
        {
            entitiesInBattle.Add(entity);
            if (entity.IsCharacter)
            {
                charactersInBattle.Add(entity);
                charactersInBattleDict.Add(entity.EntityName, entity);
            }
            else
            {
                enemiesInBattle.Add(entity.EntityName);
            }
        }

        createCharacterUI(charactersInBattle);
        createEnemyBars(enemiesInBattle);

        menuState = MenuState.Idle;

    }

    void OnEnable()
    {
        //Events from BattleManager 
        EventManager.StartListening(CombatEvents.PermitAction, characterReadyToPick);
        EventManager.StartListening(CombatEvents.ActionCompleted, actionCompleted);
        EventManager.StartListening(CombatEvents.DamageDealt, displayDamage);

        //Events from entities
        EventManager.StartListening(CombatEvents.EntityDied, removeEntityUI);
        EventManager.StartListening(CombatEvents.ComboLaunched, resetMenu);

        //Events from UI elements
        EventManager.StartListening(UIEvents.TargetPicked, targetPicked);
        EventManager.StartListening(UIEvents.AbilityPicked, abilityPicked);
        
    }

    void OnDisable()
    {
        //Events from BattleManager and Entites
        EventManager.StopListening(CombatEvents.PermitAction, characterReadyToPick);
        EventManager.StopListening(CombatEvents.ActionCompleted, actionCompleted);
        EventManager.StopListening(CombatEvents.DamageDealt, displayDamage);

        //Eevnts from enities
        EventManager.StopListening(CombatEvents.EntityDied, removeEntityUI);
        EventManager.StopListening(CombatEvents.ComboLaunched, resetMenu);

        //Events from UI elements
        EventManager.StopListening(UIEvents.TargetPicked, targetPicked);
        EventManager.StopListening(UIEvents.AbilityPicked, abilityPicked);
        //EventManager.StartListening(UIEvents.ActionLaunched)
    }


    void Update()
    {
        switch (menuState)
        {
            case MenuState.PickingTarget:

                break;
        }

        
    }



    //Triggered by BattleManger event PermitAction
    public void characterReadyToPick(CombatEventData data)
    {

        if (charactersInBattleDict.ContainsKey(data.id))
        {
            
            currentPicker = data.id;
            selectors[data.id].SetActive(true);

            enemiesListPanel.SetActive(false);
            movePanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(movePanel.transform.GetChild(0).gameObject);
            menuLayers.Push(movePanel);
            
        }

    }

    //Triggered by clicking attack in move panel
    public void attackMenu()
    {
        //List<string> possibleTargets = new List<CombatScript>(enemiesInBattle);
        List<string> possibleTargets = new List<string>();
        foreach (CombatModule entity in entitiesInBattle)
        {
            if(entity.IsCharacter == false) { possibleTargets.Add(entity.EntityName); }
        }

        if(possibleTargets.Count != 0)
        {
            movePanel.SetActive(false);
            pickingAttack = true;
            createTargetPanel(possibleTargets);
        }
        else
        {
            Debug.LogWarning("there are no possible targets");
            return;
        }

        
    }

    //Triggered by clicking a base attack button
    public void baseAttackPicked(BaseAttackType type)

    {   if (type != BaseAttackType.NULL)
        {
            CombatAction attack = charactersInBattleDict[currentPicker].baseAttacks[type];
            if (attack.IsEnoughResource(charactersInBattleDict[currentPicker].stats) == true)
            {
                currentAction = attack;
                EventManager.TriggerEvent(UIEvents.ActionLaunched, new UIEventData(currentPicker, new List<string>() { currentAttackTarget }, attack));
                
            }
        }
        else
        {
            
            EventManager.TriggerEvent(UIEvents.AttackCanceled, new UIEventData(currentPicker));
        }
    }

    //Triggered by cancel button? check that TO DO
    public void cancelAttack()
    {
        EventManager.TriggerEvent(UIEvents.AttackCanceled, new UIEventData(currentPicker));
    }

    

    //Triggered by button event
    public void magicMenu()
    {
        movePanel.SetActive(false);
        magicPanels[currentPicker].SetActive(true);
        EventSystem.current.SetSelectedGameObject(magicPanels[currentPicker].transform.GetChild(0).gameObject);
        menuLayers.Push(magicPanels[currentPicker]);
        magicPanels[currentPicker].transform.SetAsLastSibling();
        
    }

    //Triggrerd by clicking Ability button
    void abilityPicked(UIEventData data)
    {
        if(data.action.IsEnoughResource( charactersInBattleDict[currentPicker].stats) == true)
        {
            currentAction = data.action;
            List<string> possibleTargets = currentAction.GetTargets(currentPicker, true, entitiesInBattle);

            if (possibleTargets.Count != 0)
            {
                magicPanels[currentPicker].SetActive(false);
                createTargetPanel(possibleTargets);
            }
            else
            {
                Debug.LogWarning("There are no possible targets");
                return;
            }
        }

    }

    //Triggered by Target Buttons event
    void targetPicked(UIEventData data)
    {
        if(pickingAttack == true)
        {
            currentAttackTarget = data.id;
            
            

            attackPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(attackPanel.transform.GetChild(0).gameObject);
            menuLayers.Pop().SetActive(false);
            menuLayers.Push(attackPanel);
        }
        else
        {
            List<string> targets = new List<string>() { data.id };
            EventManager.TriggerEvent(UIEvents.ActionLaunched, new UIEventData(currentPicker, targets, currentAction));
            resetMenu();
        }

        
        
    }

    void actionCompleted(CombatEventData data)
    {
        resetMenu();

        selectors[data.id].SetActive(false);
        foreach (var bar in characterBars)
        {
            bar.Value.GetComponent<CharacterBarScript>().updateStatText();
        }

        currentAction = null;
        currentPicker = null;
        pickingAttack = false;
    }

    //Battle Manager -> damage delt event
    void displayDamage(CombatEventData data)
    {
        string targetID = data.targetID;
        float damage = data.damage;

        foreach (CombatModule entity in entitiesInBattle)
        {
            if(targetID == entity.EntityName)
            {
                GameObject newDamageText = Instantiate(damageTextPrefab);

                
                newDamageText.transform.SetParent(entity.gameObject.transform, false);
                newDamageText.GetComponent<DamageTextScript>().Initialize(damage);
            }
        }

    }

    

    void resetMenu()
    {
        while (menuLayers.Count > 0)
        {
            menuLayers.Pop().SetActive(false);
        }
        enemiesListPanel.SetActive(true);
    }

    //Entity -> combo launched
    void resetMenu(CombatEventData data)
    {
        if (menuLayers.Peek() == attackPanel)
        {
            resetMenu();
        }
    }

    void goBack()
    {

        if(menuLayers.Count > 1)
        {         
            menuLayers.Pop().SetActive(false);
            menuLayers.Peek().SetActive(true);
            EventSystem.current.SetSelectedGameObject(menuLayers.Peek().transform.GetChild(0).gameObject);

        }
        //TODO better do it
        if(menuLayers.Count ==1) { pickingAttack = false; }
        

    }


    void removeEntityUI(CombatEventData data)
    {
        string id = data.id;
        for(int i=0; i < entitiesInBattle.Count; i++)
        {
            if(entitiesInBattle[i].EntityName == id)
            {
                entitiesInBattle.RemoveAt(i);
                break;
            }
        }

        if (charactersInBattleDict.ContainsKey(id))
        {
            Destroy(selectors[id], 1);
            Destroy(characterBars[id], 1);
            Destroy(magicPanels[id], 1);

            charactersInBattleDict.Remove(id);
        }
        else
        {
            Destroy(enemyBars[id], 1);
        }
    }

    void createTargetPanel(List<string> targets)
    {
        GameObject newTargetPanel = Instantiate(actionPanelPrefab);
        newTargetPanel.transform.SetParent(enemyPanel, false);
        newTargetPanel.transform.SetAsLastSibling();
        newTargetPanel.name = "TargetPanel";
        newTargetPanel.GetComponent<ActionPanelScript>().persistant = false;

        foreach (string target in targets)
        {
            GameObject newButton = Instantiate(targetButtonPrefab);
       
            var buttonScript = newButton.GetComponent<TargetButtonScript>();
            buttonScript.target = target;

            Text buttonText = newButton.GetComponentInChildren<Text>();
            buttonText.text = target;

            newButton.transform.SetParent(newTargetPanel.transform, false);
        }

        menuLayers.Push(newTargetPanel);
        EventSystem.current.SetSelectedGameObject(newTargetPanel.transform.GetChild(0).gameObject); 
    }

    void createCharacterUI(List<CombatModule> charactersInBattle)
    {

        foreach (var character in charactersInBattle)
        {
            //Bars
            GameObject newBar = Instantiate(characterBarPrefab) as GameObject;
            newBar.name = "Character Bar: " + character.EntityName;

            var characterBarScript = newBar.GetComponent<CharacterBarScript>();
            characterBarScript.character = character;
            characterBarScript.initialize();

            newBar.transform.SetParent(characterPanel, false);
            characterBars.Add(character.EntityName, newBar);


            //Selector
            GameObject newSelector = Instantiate(selectorPrefab);
            newSelector.name = "Selector: " + character.EntityName;
            newSelector.SetActive(false);
            newSelector.transform.SetParent(character.gameObject.transform, false);

            selectors.Add(character.EntityName, newSelector);

            //Action Panels
            
            GameObject newMagicPanel = Instantiate(actionPanelPrefab);

            newMagicPanel.name = "magic panel: " + character.EntityName;
            newMagicPanel.transform.SetParent(enemyPanel, false);

            //attackPanels.Add(character.Key, newAttackPanel);
            magicPanels.Add(character.EntityName, newMagicPanel);

            var actions = character.abilities;
            

            foreach (CombatAction action in actions)
            {

                GameObject newActionButton = Instantiate(abilityButtonPrefab);
                newActionButton.name = action.actionName + "button";
                
                newActionButton.GetComponentInChildren<Text>().text = action.actionName;
                newActionButton.GetComponent<ActionButtonScript>().action = action;
                newActionButton.transform.SetParent(newMagicPanel.transform, false); 

            }
            newMagicPanel.SetActive(false);
        }
    }

    void createEnemyBars(List<string> enemyIDs)
    {

        foreach (string enemy in enemyIDs)
        {
            GameObject newButton = Instantiate(targetButtonPrefab) as GameObject;
            newButton.name = enemy + " Base button";
            newButton.GetComponent<Button>().interactable = false;
            var buttonScript = newButton.GetComponent<TargetButtonScript>();
            buttonScript.target = enemy;
            

            Text buttonText = newButton.GetComponentInChildren<Text>();
            buttonText.text = enemy;

            newButton.transform.SetParent(enemiesListPanel.transform, false);
            enemyBars.Add(enemy, newButton);

            

        }
    }

    
}    
