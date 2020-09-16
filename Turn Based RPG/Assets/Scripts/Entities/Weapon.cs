using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Transform weaponTransform;
    [SerializeField] Transform weaponHandle;
    [SerializeField]  Vector3 offset;


    [SerializeField] Vector3 posOffset;
    [SerializeField] Vector3 rotOffset;
    [SerializeField] Transform handTransform;

    void Awake()
    {
        
    }

	private void Start()
	{
        //offset = gameObject.transform.position - weaponHandle.transform.position;
        posOffset = transform.position - handTransform.position; //where were we relative to it?
        rotOffset = transform.eulerAngles - handTransform.eulerAngles; //how were we rotated relative to it?
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
