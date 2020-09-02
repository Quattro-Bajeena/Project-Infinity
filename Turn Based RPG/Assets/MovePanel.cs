using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePanel : MonoBehaviour
{


    [SerializeField] bool active;
    [SerializeField] [Range(0f, 1f)] float inactiveAlpha;

    CanvasGroup canvas;

    void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
        SetActive(false);

    }

    public void SetActive(bool active)
    {
        this.active = active;
        if (active)
        {
            canvas.alpha = 1;
            canvas.interactable = true;
        }
        else
        {
            canvas.alpha = inactiveAlpha;
            canvas.interactable = false;
        }
    }
}
