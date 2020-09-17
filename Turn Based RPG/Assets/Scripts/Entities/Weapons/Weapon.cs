using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public enum Range
	{
        Melee,
        Gun
	}

    public enum Handle
	{
        OneHanded,
        TwoHanded
	}

    public enum Type
	{
        Rifle,
        Lightsaber
	}
    [SerializeField] Transform secondHandHandle = null;

    [SerializeField] Range range;
    [SerializeField] Handle handle;
    public Type type;
    IWeaponBehaviour weaponBehaviour;

    [SerializeField] Transform handTransform;
    

    void Awake()
    {
        weaponBehaviour = GetComponent<IWeaponBehaviour>();
    }

    public Transform SetHand(Transform hand)
	{
        handTransform = hand;
		transform.position = handTransform.position;
		transform.rotation = handTransform.rotation;
		transform.SetParent(handTransform);
        return secondHandHandle;
	}

    public void ActivateWeapon()
	{
        weaponBehaviour.Activate();
	}

}
