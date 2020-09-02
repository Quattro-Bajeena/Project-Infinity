using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsScript : MonoBehaviour
{
	[SerializeField]
	private EntityStatistics baseStats;

	public EntityScript entity;

	// Start is called before the first frame update
	void Start()
	{
		entity = gameObject.GetComponent<EntityScript>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public EntityStatistics getStatistics()
    {
		return baseStats;
    }
}
