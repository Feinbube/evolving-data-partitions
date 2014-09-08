using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

    public class Array<T> : IArray
    {
        public Dictionary<int, List<int>> ReadsByThreadId { get; private set; }
        public Dictionary<int, List<int>> WritesByThreadId { get; private set; }

        Dictionary<int, T> memory;

        public int Size { get { return SizeX * SizeY * SizeZ; } }

        public int Length { get { return Size; } }

        public int SizeX { get; private set; }
        public int SizeY { get; private set; }
        public int SizeZ { get; private set; }

        public bool OnlyWritten { get { return ReadsByThreadId.Count == 0 && WritesByThreadId.Count > 0; } }

        public bool OnlyRead { get { return WritesByThreadId.Count == 0 && ReadsByThreadId.Count > 0; } }

        public bool NotUsed { get { return ReadsByThreadId.Count == 0 && WritesByThreadId.Count == 0; } }

        public bool ReadAndWritten { get { return ReadsByThreadId.Count > 0 && WritesByThreadId.Count > 0; } }

        public Array(int sizeX) : this(sizeX, 1, 1) { }

        public Array(int sizeX, int sizeY) : this(sizeX, sizeY, 1) { }

        public Array(int sizeX, int sizeY, int sizeZ)
        {
            this.SizeX = sizeX;
            this.SizeY = sizeY;
            this.SizeZ = sizeZ;

            memory = new Dictionary<int, T>();
            NewRound();

            Memory.Instance.Add(this);
        }

        public T Read(int threadId, int pos1D)
        {
            lock (this)
            {
                if (pos1D < 0 || pos1D >= Size)
                    throw new ArgumentOutOfRangeException("pos1D", "Thread " + threadId + " tried to access pos " + pos1D + ". Array size is " + Size + ".");

                if (!memory.ContainsKey(pos1D))
                    throw new ArgumentOutOfRangeException("pos1D", "Thread " + threadId + " tried to access pos " + pos1D + " which was never written to. Array size is " + Size + ".");

                assertCellWasNotWrittenToByAnotherThread(threadId, pos1D);

                insert(ReadsByThreadId, threadId, pos1D);
                return memory[pos1D];
            }
        }

        public void Write(int threadId, int pos1D, T value)
        {
            lock (this)
            {
                if (pos1D < 0 || pos1D > Size)
                    throw new ArgumentOutOfRangeException("pos1D", "Thread " + threadId + " tried to access pos " + pos1D + ". Array size is " + Size + ".");

                insert(WritesByThreadId, threadId, pos1D);
                if (!memory.ContainsKey(pos1D))
                    memory.Add(pos1D, value);
                else
                    memory[pos1D] = value;
            }
        }

        public T this[int index] { get { return this[index, 0, 0]; } set { this[index, 0, 0] = value; } }

        public T this[int indexX, int indexY] { get { return this[indexX, indexY, 0]; } set { this[indexX, indexY, 0] = value; } }

        public T this[int indexX, int indexY, int indexZ]
        {
            get { return Read(Thread.CurrentThread.ManagedThreadId, indexX + indexY * SizeX + indexZ * SizeX * SizeY); }
            set { Write(Thread.CurrentThread.ManagedThreadId, indexX + indexY * SizeX + indexZ * SizeX * SizeY, value); }
        }

        private void assertCellWasNotWrittenToByAnotherThread(int threadId, int pos1D)
        {
            foreach (int tId in WritesByThreadId.Keys)
                if (tId != threadId && WritesByThreadId[tId].Contains(pos1D))
                    throw new InvalidOperationException("Thread " + threadId + " is not allowed to read cell " + pos1D + ". It was already written to by thread " + tId + " in this round.");
        }
        private void assertCellWasNotWrittenTo(int threadId, int pos1D)
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

        private void insert(Dictionary<int, List<int>> dictionary, int threadId, int pos1D)
        {
            if (!dictionary.ContainsKey(threadId))
                dictionary.Add(threadId, new List<int>());
            if (!dictionary[threadId].Contains(pos1D))
                dictionary[threadId].Add(pos1D);
        }


        public string AsText(int pos1D)
        {
            lock (this)
            {
                if (pos1D < 0 || pos1D >= Size)
                    throw new ArgumentOutOfRangeException("pos1D", "Thread tried to access pos " + pos1D + ". Array size is " + Size + ".");

                if (!memory.ContainsKey(pos1D))
                    return "NULL";

                T value = memory[pos1D];
                return value is double || value is float ? String.Format("{0:0.00}", value).Substring(0, 4).TrimEnd(',') : value.ToString();
            }
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
