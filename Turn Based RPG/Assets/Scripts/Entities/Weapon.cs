using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Transform weaponTransform;
    [SerializeField] Transform weaponHandle;
    [SerializeField]  Vector3 offset;

    void Awake()
    {
        
    }

	private void Start()
	{
        //offset = gameObject.transform.position - weaponHandle.transform.position;
    }

	// Update is called once per frame
	void Update()
    {
        gameObject.transform.position = weaponTransform.position;
        gameObject.transform.rotation = weaponTransform.rotation;

		if (weaponHandle)
		{
			gameObject.transform.position = weaponTransform.position + offset;
		}
	}
}
