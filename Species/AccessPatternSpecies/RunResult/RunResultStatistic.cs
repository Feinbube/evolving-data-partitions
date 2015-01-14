using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Species
{
    public class RunResultStatistic
    {
        public int Rounds;

        public int CacheFriendlyAccesses;
        public int CoalescedAccesses;
        
        public int AccessesThatNeedToBeSynchronized; // => ConflictingAccesses ??

        public int TotalReads;
        public int TotalWrites;

        public RunResultStatistic(RunResult runResult)
        {

        }
    }
}
