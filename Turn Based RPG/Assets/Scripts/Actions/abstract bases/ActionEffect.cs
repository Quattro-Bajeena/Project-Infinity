using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ActionEffect : ScriptableObject
{
    public abstract void OnEnter(StatisticsModule stats);
    public abstract void OnEveryTurn(StatisticsModule stats);
    public abstract void OnExit(StatisticsModule stats);  
}
