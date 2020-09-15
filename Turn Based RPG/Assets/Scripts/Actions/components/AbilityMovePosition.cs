using System;
using System.Collections.Generic;
using UnityEngine;


public class AbilityMovePosition : MonoBehaviour
{
    public enum Position
    {
        StartingPosition,
        BattlefieldCenter,
        TargetPosition
    }

    public Position position;
}

