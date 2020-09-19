using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BaseAttackType
{
    NULL,
    Light,
    Medium,
    Strong
}

[System.Serializable]
public class BaseAttackComponent
{
    public BaseAttackType attackType;
}
