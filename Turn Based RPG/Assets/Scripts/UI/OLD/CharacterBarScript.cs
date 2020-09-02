using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBarScript : MonoBehaviour
{
    public CombatModule character;
    Image bar;
    Text hpText;
    Text mpText;
    Text apText;
    

    void Start()
    {
        
    }


    void Update()
    {
        updateBar();
        updateAPText();
        //updateStatText();

    }

    public void initialize()
    {
        bar = transform.Find("ProgressBar/Bar").GetComponent<Image>();
        Text name = transform.Find("CharacterName").GetComponent<Text>();
        name.text = character.EntityName;

        hpText = transform.Find("Stats/HPText/HPValue").GetComponent<Text>();
        mpText = transform.Find("Stats/MPText/MPValue").GetComponent<Text>();

        apText = transform.Find("ActionPoints/APValue").GetComponent<Text>();

        updateStatText();
        updateBar();
    }

    public void updateStatText()
    {
        
        hpText.text = $"{character.stats.health}/{character.stats.maxHealth}";
        mpText.text = $"{character.stats.mana}/{character.stats.maxMana}";

    }

    void updateAPText()
    {
        apText.text = $"{character.stats.actionPoints}/{character.stats.maxActionPoints}";
    }

    void updateBar()
    {
        bar.transform.localScale = new Vector3(
            Mathf.Clamp(character.actionGauge, 0, 1),
            bar.transform.localScale.y,
            bar.transform.localScale.z);
    }
}
