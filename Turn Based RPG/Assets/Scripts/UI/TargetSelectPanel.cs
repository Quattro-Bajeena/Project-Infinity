using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelectPanel : MonoBehaviour
{
    
    [SerializeField] GameObject buttonTemplate;
    [SerializeField] Vector2 buttonsOffset;
    [SerializeField] float selectorVerticalOffset;
    [SerializeField] GameObject selector;

    UIManager uiManager;
    UIPanel panelInfo;

    //List<GameObject> buttons = new List<GameObject>();
    GameObject currentlySelectedEnemy;
    GameObject currentlySelectedButton;

    [SerializeField] bool active;
    [SerializeField] Camera overviewCamera;

    bool selectAll = false;

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

    public void Create(List<GameObject> targets)
    {
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

        selector.SetActive(true);

    }

    public void SelectedNewTarget(GameObject target)
    {
        
        Bounds targetBounds = target.GetComponentInChildren<Renderer>().bounds;
        selector.transform.position = new Vector3(
            targetBounds.center.x,
            targetBounds.center.y + targetBounds.extents.y + selectorVerticalOffset,
            targetBounds.center.z);

        
    }

    public void TargetPicked(GameObject target)
    {
        uiManager.TargetPicked(new List<GameObject>() { target });
    }

    public void ClearTargets()
    {
        active = false;
        if(selector != null)
            selector.SetActive(false);

        panelInfo.ClearButtons();

        
    }


    
}
