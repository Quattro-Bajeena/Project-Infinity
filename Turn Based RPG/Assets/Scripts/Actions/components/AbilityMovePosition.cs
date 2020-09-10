using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMovePosition : MonoBehaviour
{
    public enum TargetMovePosition
	{
		StartingPosition,
		BattlefieldCenter,
		TargetPosition
	}

	public TargetMovePosition movePosition;
}
