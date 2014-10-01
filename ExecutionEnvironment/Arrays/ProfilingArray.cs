using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExecutionEnvironment
{
    public class ProfilingArray<T> : Array3D<T>, IArray
    {
        public Dictionary<int, List<int>> ReadsByThreadId { get; protected set; }
        public Dictionary<int, List<int>> WritesByThreadId { get; protected set; }

        public bool OnlyWritten { get { return ReadsByThreadId.Count == 0 && WritesByThreadId.Count > 0; } }

        public bool OnlyRead { get { return WritesByThreadId.Count == 0 && ReadsByThreadId.Count > 0; } }

        public bool NotUsed { get { return ReadsByThreadId.Count == 0 && WritesByThreadId.Count == 0; } }

        public bool ReadAndWritten { get { return ReadsByThreadId.Count > 0 && WritesByThreadId.Count > 0; } }

        public ProfilingArray(int sizeX) : this(sizeX, 1, 1) { }

        public ProfilingArray(int sizeX, int sizeY) : this(sizeX, sizeY, 1) { }

        public ProfilingArray(int sizeX, int sizeY, int sizeZ)
            : base(sizeX, sizeY, sizeZ)
        {
            NewRound();
            Memory.Instance.Add(this);
        }

        ~ProfilingArray() { Memory.Instance.Remove(this); }

        protected override void checkRead(int threadId, int pos1D)
        {
            base.checkRead(threadId, pos1D);
            assertCellWasNotWrittenToByAnotherThread(threadId, pos1D);
        }

        protected override T read(int threadId, int pos1D)
        {
            insert(ReadsByThreadId, threadId, pos1D);
            return base.read(threadId, pos1D);
        }

        protected override void write(int threadId, int pos1D, T value)
        {
            insert(WritesByThreadId, threadId, pos1D);
            base.write(threadId, pos1D, value);
        }

        protected void assertCellWasNotWrittenToByAnotherThread(int threadId, int pos1D)
        {
            foreach (int tId in WritesByThreadId.Keys)
                if (tId != threadId && WritesByThreadId[tId].Contains(pos1D))
                    throw new InvalidOperationException("Thread " + threadId + " is not allowed to read cell " + pos1D + ". It was already written to by thread " + tId + " in this round.");
        }

        protected void assertCellWasNotWrittenTo(int threadId, int pos1D)
        {
            foreach (int tId in WritesByThreadId.Keys)
                if (WritesByThreadId[tId].Contains(pos1D))
                    throw new InvalidOperationException("Thread " + threadId + " is not allowed to read cell " + pos1D + ". It was already written to by thread " + tId + " in this round.");
        }

        public void NewRound()
        {
            lock (this)
            {
                ReadsByThreadId = new Dictionary<int, List<int>>();
                WritesByThreadId = new Dictionary<int, List<int>>();
            }
        }

        protected void insert(Dictionary<int, List<int>> dictionary, int threadId, int pos1D)
        {
            if (!dictionary.ContainsKey(threadId))
                dictionary.Add(threadId, new List<int>());
            if (!dictionary[threadId].Contains(pos1D))
                dictionary[threadId].Add(pos1D);
        }

        public List<int> ReadsAtPosition(int pos1D)
        {
            return ReadsByThreadId.Where(a => a.Value.Contains(pos1D)).Select(a => a.Key).ToList();
        }

        public List<int> WritesAtPosition(int pos1D)
        {
            return WritesByThreadId.Where(a => a.Value.Contains(pos1D)).Select(a => a.Key).ToList();
        }
    }
}
