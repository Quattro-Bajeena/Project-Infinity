using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



interface IUsesResourceStat
{
    void useResourceStat(EntityStatistics attackerStats, float cost);
    bool isEnoughResource(EntityStatistics attackerStats, float cost);
}

