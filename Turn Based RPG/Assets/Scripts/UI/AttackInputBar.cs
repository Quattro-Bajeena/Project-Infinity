using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackInputBar : MonoBehaviour
{

    [SerializeField] GameObject blankImagePrefab;
    [SerializeField] Sprite lightAttackSprite;
    [SerializeField] Sprite mediumAttackSprite;
    [SerializeField] Sprite strongAttackSprite;

    [SerializeField] List<GameObject> currenAttackSprites = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

	private void OnEnable()
	{
        EventManager.StartListening(UIEvents.AttackLaunched, AddButton);
        EventManager.StartListening(CombatEvents.ActionCompleted, ResetBar);
	}

	private void OnDisable()
	{
        EventManager.StopListening(UIEvents.AttackLaunched, AddButton);
        EventManager.StopListening(CombatEvents.ActionCompleted, ResetBar);
    }

	// Update is called once per frame
	void Update()
    {
        
    }

    void AddButton(UIEventData data)
	{
        GameObject newImage = Instantiate(blankImagePrefab);
        newImage.transform.SetParent(this.transform, false);

        currenAttackSprites.Add(newImage);
        Sprite buttonSprite = null;
		switch (data.action.BaseAttackType)
		{
            case BaseAttackType.Light:
                buttonSprite = lightAttackSprite;
                break;
            case BaseAttackType.Medium:
                buttonSprite = mediumAttackSprite;
                break;
            case BaseAttackType.Strong:
                buttonSprite = strongAttackSprite;
                break;
            
		}
        newImage.GetComponent<Image>().sprite = buttonSprite;
	}

    void ResetBar(CombatEventData data)
	{
		foreach (GameObject attackSprite in currenAttackSprites)
		{
            Destroy(attackSprite);
		}
        currenAttackSprites.Clear();
	}
}
