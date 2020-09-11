using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IActionEffect
{

    void OnEnter(StatisticsModule stats);
    void OnEveryTurn(StatisticsModule stats);
    void OnExit(StatisticsModule stats);
      
}
