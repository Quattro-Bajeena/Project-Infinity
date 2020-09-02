using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButtonScript : MonoBehaviour
{
    [SerializeField] BaseAttackType type;

    UIManagerOld uiManager;
    
    void Start()
    {
        uiManager = FindObjectOfType<UIManagerOld>();
    }

    public void attackPicked()
    {
        uiManager.baseAttackPicked(type);
        
    }
}
