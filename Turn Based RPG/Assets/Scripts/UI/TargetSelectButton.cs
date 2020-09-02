using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectButton : MonoBehaviour
{
    public GameObject target;
    TargetSelectPanel targetSelectPanel;

    void Start()
    {

        GetComponent<Button>().onClick.AddListener(TargetPicked);
        
    }

    public void Initialize(GameObject target, Camera overviewCamera, Vector2 buttonOffset, TargetSelectPanel targetSelectPanel)
    {
        this.target = target;
        this.targetSelectPanel = targetSelectPanel;

        name += (": " + target.name);
        RectTransform rectTransform = GetComponent<RectTransform>();

        Vector2 screenPoint = overviewCamera.WorldToScreenPoint(target.transform.position);
        
        float scaleFactor = GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;
        Vector2 offset = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta / 2;

        Vector2 finalPosition = new Vector2(
            (screenPoint.x / scaleFactor) - offset.x,
            (screenPoint.y / scaleFactor) - offset.y
            );

        //artificial offset so the buttons are not visible on screen
        finalPosition += buttonOffset;

        rectTransform.anchoredPosition = finalPosition;

    }

    void TargetPicked()
    {
        
        targetSelectPanel.TargetPicked(target);
    }
}
