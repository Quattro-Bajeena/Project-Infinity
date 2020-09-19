using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModule : MonoBehaviour
{
	Entity entity;
	Animator animator;

	[SerializeField] Weapon defaultWeapon;
	public Weapon currentWeapon;
    [SerializeField] List<Weapon> avaiableWeapons = new List<Weapon>();

	[SerializeField] Transform handRiflePos;
	[SerializeField] Transform handLightsaberPos;



	[Space(15)]

	[SerializeField] bool secondHandIKActive = false;
	Transform leftHandTarget = null;
	Transform rightHandTarget = null;

	void Awake()
    {
		entity = GetComponent<Entity>();
        animator = GetComponent<Animator>();
        avaiableWeapons.AddRange(GetComponentsInChildren<Weapon>());

		
    }

	private void Start()
	{
		if (defaultWeapon)
			ChangeWeapon(defaultWeapon);
		else
			ChangeWeapon(avaiableWeapons[0]);

		foreach (Weapon weapon in avaiableWeapons)
		{
			if (weapon != currentWeapon)
				weapon.gameObject.SetActive(false);
		}
	}

	void ChangeWeapon(Weapon newWeapon)
	{
		currentWeapon = newWeapon;
		switch (newWeapon.type)
		{
			case Weapon.Type.Lightsaber:
				rightHandTarget = currentWeapon.SetHand(handLightsaberPos);
				break;

			case Weapon.Type.Rifle:
				leftHandTarget = currentWeapon.SetHand(handRiflePos);
				break;
		}
		entity.animations.ChangeWeapon(currentWeapon.animatorOverride);
		entity.combat.ChangeWeapon(currentWeapon.Abilities, currentWeapon.Attacks, currentWeapon.Combos);
	}


    void Update()
    {
        
    }

	//Events from Animation clips
    void OnWeaponRelease()
	{
		secondHandIKActive = false;
	}

    public void OnWeaponMainHand()
	{
		secondHandIKActive = false;
	}

    public void OnWeaponBothHands()
	{
		secondHandIKActive = true;
	}

    void OnActivateWeapon()
	{
		
		currentWeapon.ActivateWeapon();
	}

	void OnAnimatorIK()
	{

		//if the IK is active, set the position and rotation directly to the goal. 
		if (secondHandIKActive)
		{

			// Set the right hand target position and rotation, if one has been assigned
			if (leftHandTarget != null)
			{
				animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
				animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
				animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
				animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);


			}

			if (rightHandTarget != null)
			{
				animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
				animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
				animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
				animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);

			}

		}

		//if the IK is not active, set the position and rotation of the hand and head back to the original position
		else
		{
			animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
			animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
			animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
			animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);

		}

	}
}
