using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public enum Type
	{
        Blaster,
        Rifle,
        Blade,
        Staff,
        Lightsaber
	}

    public Type type;

    [SerializeField] Transform secondHandHandle = null;
    Transform handTransform;

    WeaponStatistics statistics;
    IWeaponBehaviour weaponBehaviour;

    void Awake()
    {
        statistics = GetComponent<WeaponStatistics>();
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
