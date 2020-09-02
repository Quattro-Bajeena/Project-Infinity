using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButtonScript : MonoBehaviour
{
    [SerializeField] BaseAttackType type;

    UIManager uiManager;
    
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    public void attackPicked()
    {
        uiManager.baseAttackPicked(type);
        
    }
}
