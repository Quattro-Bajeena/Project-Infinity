using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsModule : MonoBehaviour
{
	[SerializeField]
	private EntityStatistics baseStats;

	public Entity entity;

	// Start is called before the first frame update
	void Start()
	{
		entity = gameObject.GetComponent<Entity>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public EntityStatistics GetStatistics()
    {
		return baseStats;
    }
}
