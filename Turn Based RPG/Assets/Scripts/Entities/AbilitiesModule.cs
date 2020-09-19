using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesModule : MonoBehaviour
{

    Entity entity;
    [SerializeField] List<CombatAction> _abilities = new List<CombatAction>();
    public List<CombatAction> Abilities
	{
		get { return _abilities; }
	}


    void Awake()
    {
        entity = GetComponent<Entity>();
    }

    
}
