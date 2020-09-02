using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSection : MonoBehaviour
{
    [SerializeField] GameObject characterPanelPrefab;
    UIManager UIManager;
    Dictionary<string, CombatModule> characters;
    Dictionary<string, CharacterPanel> characterPanels = new Dictionary<string, CharacterPanel>();

    void Start()
    {
        
    }

    private void OnEnable()
    {
        EventManager.StartListening(CombatEvents.ActionCompleted, updateCharacterHealth);
        EventManager.StartListening(CombatEvents.PermitAction, CharacterReady);
        EventManager.StartListening(CombatEvents.ActionCompleted, ActionCompleted);
    }

    private void OnDisable()
    {
        EventManager.StopListening(CombatEvents.ActionCompleted, updateCharacterHealth);
        EventManager.StopListening(CombatEvents.PermitAction, CharacterReady);
        EventManager.StopListening(CombatEvents.ActionCompleted, ActionCompleted);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var panel in characterPanels)
        {
            panel.Value.updateActionGauge(characters[panel.Key].actionGauge);
        }
    }

    void updateCharacterHealth(CombatEventData data)
    {
        foreach (var panel in characterPanels)
        {
            panel.Value.updateHealthText(characters[panel.Key].stats.health);
        }
    }

    void CharacterReady(CombatEventData data)
    {
        if (characterPanels.ContainsKey(data.id))
        {
            characterPanels[data.id].setActive(true);
        }
        
    }

    void ActionCompleted(CombatEventData data)
    {
        if (characterPanels.ContainsKey(data.id))
        {
            characterPanels[data.id].setActive(false);
        }
    }

    public void Initialize(Dictionary<string, CombatModule> characters, UIManager UIManager)
    {
        this.UIManager = UIManager;
        this.characters = characters;
        foreach (KeyValuePair<string, CombatModule> character in characters)
        {
            GameObject newCharacterPanel = Instantiate(characterPanelPrefab);
            newCharacterPanel.SetActive(true);
            CharacterPanel characterPanelScript = newCharacterPanel.GetComponent<CharacterPanel>();

            characterPanelScript.Initialize(null, character.Value.stats.health, character.Value.stats.maxHealth);
            
            newCharacterPanel.transform.SetParent(this.gameObject.transform, false);

            characterPanels.Add(character.Key, characterPanelScript);
        }
    }



    public void DeleteCharacterPanel(CombatEventData data)
    {
        if (characterPanels.ContainsKey(data.id))
        {
            string characterId = data.id;
            Destroy(characterPanels[characterId]);
        }

    }
}
