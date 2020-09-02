using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BaseAttackType
{
    Light,
    Medium,
    Strong,
    NULL
}

public class BaseAttack : MonoBehaviour
{
    public BaseAttackType attackType;
}
