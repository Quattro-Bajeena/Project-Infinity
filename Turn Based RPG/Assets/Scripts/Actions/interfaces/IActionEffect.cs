using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IActionEffect
{

    void OnEnter(EntityStatistics stats);
    void OnEveryTurn(EntityStatistics stats);
    void OnExit(EntityStatistics stats);
      
}
