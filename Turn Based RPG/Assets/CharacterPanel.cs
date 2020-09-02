using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{

    Image portrait;
    TextMeshProUGUI healthText;
    Slider timer;

    float maxHealth;

    private void Awake()
    {
        portrait = transform.Find("Portrait").GetComponent<Image>();
        healthText = transform.Find("Health").GetComponent<TextMeshProUGUI>();
        timer = transform.Find("Timer").GetComponent<Slider>();
    }

    public void Initialize(Sprite portraitSprite, float currentHealth ,float maxHealth)
    {
        //portrait.sprite = portraitSprite;
        
        this.maxHealth = maxHealth;
        healthText.text = $"{currentHealth}/{maxHealth}";

        timer.value = 0;
    }

    public void updateHealthText(float health)
    {
        healthText.text = $"{health}/{maxHealth}";
    }

    public void updateActionGauge(float percent)
    {
        timer.value = percent;
    }

    public void setActive(bool active)
    {
        if (active)
        {
            portrait.color = Color.yellow;
        }
        else
        {
            portrait.color = Color.white;
        }
    }
}
