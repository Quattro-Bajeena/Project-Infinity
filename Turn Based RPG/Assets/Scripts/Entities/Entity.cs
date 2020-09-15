using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public string Id
	{
		get { return name; }
	}
    [SerializeField] new string name;

    public StatisticsModule stats;
    public CombatModule combat;
    public AnimationModule animations;
    public MovementModule movement;


    void Awake()
    {
        stats = GetComponent<StatisticsModule>();
        combat = GetComponent<CombatModule>();
        animations = GetComponent<AnimationModule>();
        movement = GetComponent<MovementModule>();

        if (name == "")
            name = gameObject.name;
    }

    
}
