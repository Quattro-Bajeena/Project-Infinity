using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public interface IUsesResourceStat
{
    void UseResourceStat(StatisticsModule attackerStats, int cost);
    bool IsEnoughResource(StatisticsModule attackerStats, int cost);
}

