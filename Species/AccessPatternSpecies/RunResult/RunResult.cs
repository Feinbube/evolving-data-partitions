using ExecutionEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Species
{
    public class RunResult
    {
        public bool IsValid { get; set; }

        public Arr<byte> IndexRange { get; set; }

        public List<Round> Rounds { get; set; }

        private Round currentRound { get; set; }
        private byte currentThreadId { get; set; }

        public RunResult(Arr<byte> indexRange, List<INotifyingArray> memory)
        {
            currentRound = new Round();
            currentThreadId = 0;

            foreach(INotifyingArray array in memory)
            { 
                //currentRound.ArrayInfos.Add(new ArrayInfo(){})
                array.OnRead += array_OnRead;
                array.OnWrite += array_OnWrite;
            }
        }

        void array_OnWrite(object sender, ArrayEventArgs e)
        {
            throw new NotImplementedException();
        }

        void array_OnRead(object sender, ArrayEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void SetThreadID(byte tId)
        {
            currentThreadId = tId;
        }

        public void Finish()
        {
            Rounds.Add(currentRound);
        }

        public RunResultStatistic Statistics()
        {
            return new RunResultStatistic(this);
        }
    }
}
