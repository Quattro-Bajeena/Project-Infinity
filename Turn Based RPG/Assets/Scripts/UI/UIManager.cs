using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] InputSystemUIInputModule inputModule;
    [SerializeField] GameObject firstSelectedPanel;

    //Panels Present in the Scene
    //[SerializeField] GameObject itemPanel;
    [SerializeField] GameObject movePanel;
    [SerializeField] GameObject targetPanel;
    [SerializeField] GameObject abilityPanel;
    [SerializeField] GameObject attackPanel;

    [SerializeField] GameObject attackBar;
    [SerializeField] GameObject characterSection;

    [SerializeField] GameObject damageTextPrefab;

    [SerializeField] GameObject menuSelector;
    [SerializeField] GameObject currentlySelectedButton;
    [SerializeField] GameObject realCurrentlySelectedButton;
    
    GameObject currentluUsedPanel;

    

    Stack<GameObject> panelStack = new Stack<GameObject>();
    Stack<GameObject> previousPanelButtonStack = new Stack<GameObject>();


    [SerializeField] List<GameObject> previousPanelButtonList = new List<GameObject>();
    [SerializeField] List<GameObject> panelList = new List<GameObject>();

    PlayerControls controls;

    //GAMEPLAY
    //List<GameObject> entities = new List<GameObject>();
    Dictionary<string, GameObject> entities = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> enemies = new Dictionary<string, GameObject>();
    Dictionary<string, CombatModule> characters = new Dictionary<string, CombatModule>();
    

    [SerializeField] string currentCharacter;
    [SerializeField] List<string> currentActionTargets;
    [SerializeField] CombatAction currentAbilityPicked;

    enum UIMove
    {
        NULL,
        Attack,
        Ability,
        Item,
        Defend,
        Escape
    }

    UIMove currentUIMove;
    

    void Awake()
    {
        //Init Controls
        controls = new PlayerControls();

        controls.CombatUI.Cancel.performed += _ => ClosePanel();

        controls.Combat.CancelAttack.performed += _ => AttackCanceled();
        controls.Combat.LightAttack.performed += _ => BaseAttackPicked(BaseAttackType.Light);
        controls.Combat.MediumAttack.performed += _ => BaseAttackPicked(BaseAttackType.Medium);
        controls.Combat.StrongAttack.performed += _ => BaseAttackPicked(BaseAttackType.Strong);


        currentUIMove = UIMove.NULL;
    }


    void Start()
    {

        inputModule = FindObjectOfType<InputSystemUIInputModule>();
        //inputModule.actionsAsset = controls.asset;

        foreach (var entity in FindObjectsOfType<Entity>())
        {
            entities.Add( entity.gameObject.GetComponent<CombatModule>().EntityName, entity.gameObject);
            CombatModule entityCombat = entity.GetComponent<CombatModule>();
            if (entityCombat.IsCharacter == true)
            {
                characters.Add(entityCombat.EntityName, entityCombat);
            }
            else
            {
                enemies.Add(entityCombat.EntityName, entity.gameObject);
            }
        }

        //Init Panels
        InitItemPanel();
        InitTargetPanel();
        InitAbilityPanel();
        InitCharacterSection();
        
        //Init Selector
        panelStack.Push(firstSelectedPanel);

        LayoutRebuilder.ForceRebuildLayoutImmediate(panelStack.Peek().GetComponent<RectTransform>());

        currentlySelectedButton = panelStack.Peek().GetComponent<UIPanel>().GetFirstButton();
        EventSystem.current.SetSelectedGameObject(currentlySelectedButton);
        StartCoroutine(SelectedButtonChanged(currentlySelectedButton));

        menuSelector.SetActive(false);
    }

    void OnEnable()
    {
        controls.CombatUI.Enable();
        controls.Combat.Disable();

        //Events from BattleManager 
        EventManager.StartListening(CombatEvents.PermitAction, CharacterReady);
        EventManager.StartListening(CombatEvents.ActionCompleted, ActionCompletedCleanup);
        EventManager.StartListening(CombatEvents.DamageDealt, DisplayDamage);

        //Events from entities
        EventManager.StartListening(CombatEvents.EntityDied, EntityDied);
        EventManager.StartListening(CombatEvents.ComboLaunched, ComboLaunched);

    }

    void OnDisable()
    {
        controls.Disable();
        EventManager.StopListening(CombatEvents.PermitAction, CharacterReady);
        EventManager.StopListening(CombatEvents.ActionCompleted, ActionCompletedCleanup);
        EventManager.StopListening(CombatEvents.DamageDealt, DisplayDamage);

        //Events from entities
        EventManager.StopListening(CombatEvents.EntityDied, EntityDied);
        EventManager.StopListening(CombatEvents.ComboLaunched, ComboLaunched);
    }


    private void Update()
    {
        //uiControllsEnabled = controls.CombatUI.enabled;
        //combatControllsEnabled = controls.Combat.enabled;
        //realCurrentlySelectedButton = EventSystem.current.currentSelectedGameObject;

        //previousPanelButtonList.Clear();
        //previousPanelButtonList.AddRange(previousPanelButtonStack.ToArray());
    }

    //GAME LOGIC
    void CharacterReady(CombatEventData data)
    {
        if (characters.ContainsKey(data.id))
        {
            currentCharacter = data.id;
            menuSelector.SetActive(true);
            SetAttackControles(false); //ui controls
            EventSystem.current.SetSelectedGameObject(firstSelectedPanel.GetComponent<UIPanel>().GetFirstButton());
            movePanel.GetComponent<MovePanel>().SetActive(true);
            attackBar.GetComponent<AttackBar>().changeActiveCharacter(characters[data.id]);

        }
    }

    //Callbacks from panels
    public void TargetPicked(List<GameObject> targets)
    {
        menuSelector.SetActive(false);
        
        DisableControls();
        foreach (GameObject targetGo in targets)
        {
            currentActionTargets.Add(targetGo.GetComponent<CombatModule>().EntityName);
        }
        
        //Switch:
        //attacking -> start attack
        //casting a spell -> start spell action
        //using item -> use item
        switch(currentUIMove)
        {
            case UIMove.Ability:
                HidePanel();
                EventManager.TriggerEvent(UIEvents.ActionLaunched, new UIEventData(currentCharacter, currentActionTargets, currentAbilityPicked));
                break;

            case UIMove.Attack:
                OpenAttackPanel();
                break;

            case UIMove.Item:

                break;
        }

        
    }

    public void AbilityPicked(CombatAction ability)
    {
        currentAbilityPicked = ability;

        //get list of targets from ability
        List<string> targets = ability.GetTargets(currentCharacter, true, new List<GameObject>(entities.Values));
        if(targets.Count > 0)
        {
            List<GameObject> targetsGO = new List<GameObject>();
            foreach (string target in targets)
            {
                targetsGO.Add(entities[target]);
            }

            HidePanel();
            OpenTargetPanel(targetsGO, ability.actionRange);
        }
        
    }

    void AbilityLaunched(CombatAction ability)
    {

    }

    void BaseAttackPicked(BaseAttackType type)
    {
        if (currentCharacter != null && currentActionTargets.Count > 0)
        {
            
            CombatAction attack = characters[currentCharacter].baseAttacks[type];
            if (attack.IsEnoughResource(characters[currentCharacter].stats))
            {
                EventManager.TriggerEvent(UIEvents.ActionLaunched, new UIEventData(currentCharacter, currentActionTargets, attack));
                EventManager.TriggerEvent(UIEvents.AttackLaunched, new UIEventData(currentCharacter));
            }
            
        }
        else Debug.LogWarning("base attack picked but no current character or targets");
    }

    void AttackCanceled()
    {
        EventManager.TriggerEvent(UIEvents.AttackCanceled, new UIEventData(currentCharacter));
    }

    //Battle Manager -> damage delt event
    void DisplayDamage(CombatEventData data)
    {
        string targetID = data.targetID;
        float damage = data.damage;

        GameObject newDamageText = Instantiate(damageTextPrefab);
        newDamageText.SetActive(true);
        newDamageText.transform.SetParent(entities[targetID].transform, false);
        newDamageText.GetComponent<DamageTextScript>().Initialize(damage);

    }

    void ActionCompletedCleanup(CombatEventData data)
    {
        Debug.Log("ACTION COMPLETED CLEANUP");
        currentAbilityPicked = null;
        currentCharacter = null;
        currentActionTargets.Clear();

        currentUIMove = UIMove.NULL;
        SetAttackControles(false);
        menuSelector.SetActive(false);
        ResetPanels();

        movePanel.GetComponent<MovePanel>().SetActive(false);
        attackBar.GetComponent<AttackBar>().ResetActiveCharacter();
        
    }


    

    //Main Move Panel Option
    public void PickAttackMove()
    {
        //Get possible targets
        if(enemies.Count > 0)
        {
            currentUIMove = UIMove.Attack;
            EventManager.TriggerEvent(UIEvents.AttackMenuSelected, new UIEventData(currentCharacter));
            OpenTargetPanel(new List<GameObject>(enemies.Values), CombatAction.ActionRange.Single);
        }
        
    }

    public void OpenAbilityPanel()
    {
        //Get character abilities
        currentUIMove = UIMove.Ability;
        OpenCharacterAbilityPanel(new List<CombatAction>(characters[currentCharacter].abilities));
    }

    void OpenAttackPanel()
    {
        SetAttackControles(true);
        OpenPanel(attackPanel);
    }
    
    // Opening Panels With Data they need
    public void OpenTargetPanel(List<GameObject> entities, CombatAction.ActionRange range)
    {
        targetPanel.GetComponent<TargetSelectPanel>().Create(entities, range);
        OpenPanel(targetPanel);

    }

    public void OpenCharacterAbilityPanel(List<CombatAction> abilities)
    {
        
        abilityPanel.GetComponent<AbilityPanel>().Create(characters[currentCharacter].stats ,abilities);
        OpenPanel(abilityPanel);
    }

    public void OpenItemPanel()
    {
        currentUIMove = UIMove.Item;
    }

    void ComboLaunched(CombatEventData data)
    {
        DisableControls();
    }

    void DisableControls()
    {
        controls.Combat.Disable();
        controls.CombatUI.Disable();
    }

    void SetAttackControles(bool active)
    {
        if (active)
        {
            controls.Combat.Enable();
            controls.CombatUI.Disable();
        }
        else
        {
            controls.Combat.Disable();
            controls.CombatUI.Enable();
        }
    }


    

    void EntityDied(CombatEventData data)
    {
        //Characyer death
        if (characters.ContainsKey(data.id))
        {
            //Can be revied still
        }
        //Enemy death
        else
        {
            //Dead for good
            entities.Remove(data.id);
            enemies.Remove(data.id);
        }
    }

    void EntityRevived(CombatEventData data)
    {

    }


    //UI LOGIC
    void LateUpdate()
    {
        if (menuSelector.activeSelf == true)
        {
            if (currentlySelectedButton != EventSystem.current.currentSelectedGameObject)
            {
                currentlySelectedButton = EventSystem.current.currentSelectedGameObject;
                EventManager.TriggerEvent(UIEvents.SelectedButtonChanged, new UIEventData(currentlySelectedButton));
                StartCoroutine(SelectedButtonChanged(currentlySelectedButton));
            }
        }

    }

    IEnumerator SelectedButtonChanged(GameObject newButton)
    {
        yield return null;
        yield return new WaitForEndOfFrame();
        if (newButton != null)
        {

            menuSelector.transform.position = newButton.transform.Find("PointerPosition").position;

            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();

            menuSelector.transform.localScale = new Vector3(
                buttonText.fontScale,
                buttonText.fontScale,
                buttonText.fontScale
                );
        }

    }

    void OpenPanel(GameObject newPanel)
    {

        //Old panel
        panelStack.Peek().GetComponent<CanvasGroup>().interactable = false;
        previousPanelButtonStack.Push(currentlySelectedButton);

        //New Panel
        newPanel.gameObject.SetActive(true);
        newPanel.GetComponent<CanvasGroup>().interactable = true;
        newPanel.transform.SetAsLastSibling();


        EventSystem.current.SetSelectedGameObject(newPanel.GetComponent<UIPanel>().GetFirstButton());

        currentluUsedPanel = newPanel;
        panelStack.Push(newPanel);

        Canvas.ForceUpdateCanvases();
    }


    void ClosePanel()
    {
        if (panelStack.Count > 1)
        {
            

            //Closing panel
            panelStack.Peek().GetComponent<CanvasGroup>().interactable = false;
            panelStack.Peek().gameObject.SetActive(false);
            panelStack.Pop();

            


            //Panel underneath
            while (previousPanelButtonStack.Peek() == null)
                previousPanelButtonStack.Pop();

            panelStack.Peek().SetActive(true);
            panelStack.Peek().GetComponent<CanvasGroup>().interactable = true;
            EventSystem.current.SetSelectedGameObject(previousPanelButtonStack.Pop());
            currentluUsedPanel = panelStack.Peek();

            if (panelStack.Count == 1)
            {
                if(currentUIMove == UIMove.Attack)
                {
                    EventManager.TriggerEvent(UIEvents.AttackMenuCanceled, new UIEventData(currentCharacter));
                }

                currentUIMove = UIMove.NULL;
            }
        }
        
    }

    void HidePanel()
    {
        panelStack.Peek().GetComponent<CanvasGroup>().interactable = false;
        panelStack.Peek().gameObject.SetActive(false);
    }

    void ResetPanels()
    {
        while (panelStack.Count > 1)
        {
            ClosePanel();
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

    //Initialize Panels
    void InitItemPanel()
    {
        
    }

    void InitTargetPanel()
    {
        targetPanel.GetComponent<TargetSelectPanel>().Initialize(this);
        targetPanel.SetActive(false);
    }

    void InitAbilityPanel()
    {
        abilityPanel.GetComponent<AbilityPanel>().Initialize(this);
        abilityPanel.SetActive(false);
    }

    void InitCharacterSection()
    {
        characterSection.GetComponent<CharacterSection>().Initialize(characters, this);
        characterSection.SetActive(true);
    }
}
