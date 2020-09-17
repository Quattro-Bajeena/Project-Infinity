using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsaberBehaviour : MonoBehaviour, IWeaponBehaviour
{



	public void Activate()
	{
		StartCoroutine(ActivateLightsaberCoroutine(GetComponent<SkinnedMeshRenderer>()));
	}

	public void Attack()
	{
		
	}

	IEnumerator ActivateLightsaberCoroutine(SkinnedMeshRenderer lightsaberMesh)
	{
		float value = lightsaberMesh.GetBlendShapeWeight(0);
		while (value > 0)
		{
			value -= 250 * Time.deltaTime;
			lightsaberMesh.SetBlendShapeWeight(0, value);
			yield return null;
		}
		lightsaberMesh.SetBlendShapeWeight(0, 0);
	}
}
