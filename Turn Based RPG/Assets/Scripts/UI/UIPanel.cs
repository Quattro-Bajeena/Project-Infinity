using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    [SerializeField] List<GameObject> availableButtons = new List<GameObject>();
    [SerializeField] GameObject firstButton;

    public List<GameObject> Buttons
    {
        get
        {
            return availableButtons;
        }
    }

    public GameObject GetFirstButton()
    {
        if (firstButton)
            return firstButton;
        else if (availableButtons.Count > 0)
            return availableButtons[0];
        else
            return null;

    }

    public void AddButton(GameObject button)
    {
        availableButtons.Add(button);
    }

    public void ClearButtons()
    {
        foreach (GameObject button in availableButtons)
        {
            Destroy(button);
        }
        availableButtons.Clear();
    }
}
