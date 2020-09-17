using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModule : MonoBehaviour
{
	Animator animator;

	[SerializeField] Weapon currentWeapon;
    [SerializeField] List<Weapon> avaiableWeapons = new List<Weapon>();

	[SerializeField] Transform handRiflePos;
	[SerializeField] Transform handLightsaberPos;

	[SerializeField] RuntimeAnimatorController lightsaberAnimatorController;
	[SerializeField] RuntimeAnimatorController rifleAnimatorController;


	[SerializeField] bool secondHandIKActive = false;
	[SerializeField] Transform leftHandTarget = null;
	[SerializeField] Transform rightHandTarget = null;

	void Start()
    {
        animator = GetComponent<Animator>();
        avaiableWeapons.AddRange(GetComponentsInChildren<Weapon>());

        currentWeapon = avaiableWeapons[0];

		if(currentWeapon.type == Weapon.Type.Lightsaber)
		{
			animator.runtimeAnimatorController = lightsaberAnimatorController;
			rightHandTarget = currentWeapon.SetHand(handLightsaberPos);
			

		}
		else if(currentWeapon.type == Weapon.Type.Rifle)
		{
			animator.runtimeAnimatorController = rifleAnimatorController;
			leftHandTarget = currentWeapon.SetHand(handRiflePos);
		}
    }

    
    void Update()
    {
        
    }

    void OnWeaponRelease()
	{
		secondHandIKActive = false;
	}

    void OnWeaponMainHand()
	{
		secondHandIKActive = false;
	}

    void OnWeaponBothHands()
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
