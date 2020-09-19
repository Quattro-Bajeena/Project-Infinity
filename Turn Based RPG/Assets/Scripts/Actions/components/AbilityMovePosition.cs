using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityMovePosition
{
    public enum Position
    {
        StartingPosition,
        BattlefieldCenter,
        TargetPosition
    }

    public Position position;
}

