using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEnvironment
{
    public interface IArray
    {
        Dictionary<int, List<int>> ReadsByThreadId { get; }
        Dictionary<int, List<int>> WritesByThreadId { get; }

        int Size { get; }

        void NewRound();

        string AsText(int pos1D);

        bool OnlyWritten { get; }

        bool OnlyRead { get; }

        bool NotUsed { get; }

        bool ReadAndWritten { get; }

        List<int> ReadsAtPosition(int pos);

        List<int> WritesAtPosition(int pos);
    }
}
