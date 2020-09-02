using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollPanelControl : MonoBehaviour
{
    ScrollRect scrollRect;

    float initialContentPosition;
    float distanceScrolled = 0;
    float spacing = 0;

    void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        initialContentPosition = scrollRect.content.anchoredPosition.y;


        if (scrollRect.content.gameObject.GetComponent<VerticalLayoutGroup>())
            spacing = scrollRect.content.gameObject.GetComponent<VerticalLayoutGroup>().spacing;

        else if (scrollRect.content.gameObject.GetComponent<GridLayoutGroup>())
            spacing = scrollRect.content.gameObject.GetComponent<GridLayoutGroup>().spacing.y;
    }

    void OnEnable()
    {
        distanceScrolled = 0;
        scrollRect.content.anchoredPosition = new Vector2(scrollRect.content.anchoredPosition.x, initialContentPosition);
        EventManager.StartListening(UIEvents.SelectedButtonChanged, SelectedButtonChanged);

    }

    void OnDisable()
    {
        
        EventManager.StopListening(UIEvents.SelectedButtonChanged, SelectedButtonChanged);
    }

    void SelectedButtonChanged(UIEventData data)
    {
        if (scrollRect.content.childCount > 0 && data.buttonSelected != null)
        {
            StartCoroutine(SelectedButtonChangedCoroutine(data.buttonSelected));
        }
        
    }

    IEnumerator SelectedButtonChangedCoroutine(GameObject selectedButton)
    {
        yield return null;
        

        RectTransform buttonRectTransform = selectedButton.GetComponent<RectTransform>();

        float buttonTop = buttonRectTransform.anchoredPosition.y;
        float buttonBottom = buttonTop - buttonRectTransform.rect.height;

        float buttonHeight = buttonRectTransform.rect.height + spacing;


        if (buttonTop > -1 * distanceScrolled)
        {
            //Scroll up
            distanceScrolled -= buttonHeight;

            scrollRect.content.anchoredPosition = new Vector2(
                scrollRect.content.anchoredPosition.x,
                scrollRect.content.anchoredPosition.y - buttonHeight
                );


        }
        else if (buttonBottom < -1 * (distanceScrolled + scrollRect.viewport.rect.height))
        {
            //Scroll Down
            distanceScrolled += buttonHeight;

            scrollRect.content.anchoredPosition = new Vector2(
                scrollRect.content.anchoredPosition.x,
                scrollRect.content.anchoredPosition.y + buttonHeight
                );
        }
    }
    
}
