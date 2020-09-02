using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelectPanel : MonoBehaviour
{
    
    [SerializeField] GameObject buttonTemplate;
    [SerializeField] Vector2 buttonsOffset;
    [SerializeField] float selectorVerticalOffset;
    [SerializeField] GameObject mainSelector;
    [SerializeField] List<GameObject> selectors = new List<GameObject>();
    [SerializeField] List<GameObject> availableTargets;
    UIManager uiManager;
    UIPanel panelInfo;

    GameObject currentlySelectedEnemy;
    GameObject currentlySelectedButton;

    [SerializeField] bool active;
    [SerializeField] Camera overviewCamera;

    CombatAction.ActionRange selectingRange;

    void Start()
    {
        
    }

    void OnEnable()
    {
        EventManager.StartListening(UIEvents.SelectedButtonChanged, SelectedTargetChanged);
    }

    void OnDisable()
    {
        EventManager.StopListening(UIEvents.SelectedButtonChanged, SelectedTargetChanged);
        ClearTargets();
    }

    public void Initialize(UIManager uiManager)
    {
        this.uiManager = uiManager;
        panelInfo = GetComponent<UIPanel>();
        active = false;
    }


    void SelectedTargetChanged(UIEventData data)
    {
        if(active == true)
        {
            currentlySelectedButton = data.buttonSelected;
            currentlySelectedEnemy = currentlySelectedButton.GetComponent<TargetSelectButton>().target;
            SelectedNewTarget(currentlySelectedEnemy);
        }
        
    }

    public void Create(List<GameObject> targets, CombatAction.ActionRange range)
    {
        availableTargets = targets;
        ClearTargets();
        active = true;

        foreach (GameObject target in targets)
        {
            GameObject newButton = Instantiate(buttonTemplate) as GameObject;
            
            panelInfo.AddButton(newButton);
            newButton.SetActive(true);
            

            newButton.GetComponent<TargetSelectButton>().Initialize(target, overviewCamera, buttonsOffset, this);

            newButton.transform.SetParent(buttonTemplate.transform.parent, false);
        }

        selectingRange = range;
        switch (range)
        {
            case CombatAction.ActionRange.Single:
                mainSelector.SetActive(true);
                break;

            case CombatAction.ActionRange.All:
                foreach (GameObject target in targets)
                {
                    GameObject newSelector = Instantiate(mainSelector, mainSelector.transform.parent);
                    newSelector.SetActive(true);
                    ChangeSelectorPosition(newSelector, target);
                    selectors.Add(newSelector);
                }
                break;
        }

        

    }

    public void SelectedNewTarget(GameObject target)
    {
        switch (selectingRange)
        {
            case CombatAction.ActionRange.Single:
                ChangeSelectorPosition(mainSelector, target);
                break;

            case CombatAction.ActionRange.All:
                
                break;
        }
        
    }

    void ChangeSelectorPosition(GameObject selector, GameObject target)
    {
        Bounds targetBounds = target.GetComponentInChildren<Renderer>().bounds;
        selector.transform.position = new Vector3(
            targetBounds.center.x,
            targetBounds.center.y + targetBounds.extents.y + selectorVerticalOffset,
            targetBounds.center.z);
    }

    public void TargetPicked(GameObject target)
    {
        switch (selectingRange)
        {
            case CombatAction.ActionRange.Single:
                uiManager.TargetPicked(new List<GameObject>() { target });
                break;

            case CombatAction.ActionRange.All:
                uiManager.TargetPicked(new List<GameObject>(availableTargets));
                break;
        }
        

        selectingRange = CombatAction.ActionRange.Single;
        availableTargets = null;
    }

    public void ClearTargets()
    {
        active = false;
        if(mainSelector != null)
            mainSelector.SetActive(false);

        foreach (GameObject selector in selectors)
        {
            Destroy(selector);
        }
        selectors.Clear();
        panelInfo.ClearButtons();
    }


    
}
