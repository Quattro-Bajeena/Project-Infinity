using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public interface IUsesResourceStat
{
    void UseResourceStat(EntityStatistics attackerStats, float cost);
    bool IsEnoughResource(EntityStatistics attackerStats, float cost);
}

