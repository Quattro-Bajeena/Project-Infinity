using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public string Id
	{
		get { return name; }
	}
    [SerializeField] new string name = "";

    public StatisticsModule stats { get; private set; }
    public CombatModule combat { get; private set; }
    public AnimationModule animations { get; private set; }
    public MovementModule movement { get; private set; }
    public WeaponModule weapon { get; private set; }
    public AbilitiesModule abilities { get; private set; }


    void Awake()
    {
        stats = GetComponent<StatisticsModule>();
        combat = GetComponent<CombatModule>();
        animations = GetComponent<AnimationModule>();
        movement = GetComponent<MovementModule>();
        weapon = GetComponent<WeaponModule>();
        abilities = GetComponent<AbilitiesModule>();

        if (name == "")
            name = gameObject.name;
    }

    
}
