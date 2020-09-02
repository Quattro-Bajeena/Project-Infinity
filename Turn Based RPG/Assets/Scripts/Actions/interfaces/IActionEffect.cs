using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IActionEffect
{

    void onEnter(EntityStatistics stats);
    void onEveryTurn(EntityStatistics stats);
    void onExit(EntityStatistics stats);
      
}
