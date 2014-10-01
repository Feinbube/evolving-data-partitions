using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExecutionEnvironment
{
    public class Array<T>
    {
        protected T[] memory;
        protected bool[] written;

        public int Size { get { return memory.Length; } }

        public int Length { get { return memory.Length; } }

        public int SizeX { get; protected set; }
        public int SizeY { get; protected set; }
        public int SizeZ { get; protected set; }

        public Array(int sizeX) : this(sizeX, 1, 1) { }

        public Array(int sizeX, int sizeY) : this(sizeX, sizeY, 1) { }

        public Array(int sizeX, int sizeY, int sizeZ)
        {
            this.SizeX = sizeX;
            this.SizeY = sizeY;
            this.SizeZ = sizeZ;

            memory = new T[sizeX * sizeY * sizeZ];
            written = new bool[memory.Length];
        }

        public void Write(T[] values) { Write(values, 0); }

        public void Write(T[] values, int offset)
        {
            for (int i = 0; i < values.Length; i++)
                this[i + offset] = values[i];
        }

        public virtual T Read(int threadId, int pos1D)
        {
            lock (memory)
            {
                checkRead(threadId, pos1D);
                return read(threadId, pos1D);
            }
        }

        protected virtual void checkRead(int threadId, int pos1D)
        {
            checkPos(threadId, pos1D);

            if(!written[pos1D])
                throw new ArgumentOutOfRangeException("pos1D", "Thread " + threadId + " tried to access pos " + pos1D + " which was never written to. Array size is " + Size + ".");
        }

        protected virtual void checkPos(int threadId, int pos1D)
        {
            if (pos1D < 0 || pos1D >= Size)
                throw new ArgumentOutOfRangeException("pos1D", "Thread " + threadId + " tried to access pos " + pos1D + ". Array size is " + Size + ".");
        }

        protected virtual T read(int threadId, int pos1D)
        {
            return memory[pos1D];
        }

        public virtual void Write(int threadId, int pos1D, T value)
        {
            lock (memory)
            {
                checkPos(threadId, pos1D);
                write(threadId, pos1D, value);
            }
        }

        protected virtual void write(int threadId, int pos1D, T value)
        {
            memory[pos1D] = value;
            written[pos1D] = true;
        }

        public T this[int index] { get { return this[index, 0, 0]; } set { this[index, 0, 0] = value; } }

        public T this[int indexX, int indexY] { get { return this[indexX, indexY, 0]; } set { this[indexX, indexY, 0] = value; } }

        public T this[int indexX, int indexY, int indexZ]
        {
            get { return Read(Thread.CurrentThread.ManagedThreadId, indexX + indexY * SizeX + indexZ * SizeX * SizeY); }
            set { Write(Thread.CurrentThread.ManagedThreadId, indexX + indexY * SizeX + indexZ * SizeX * SizeY, value); }
        }

        public string AsText(int pos1D)
        {
            lock (this)
            {
                if (pos1D < 0 || pos1D >= Size)
                    throw new ArgumentOutOfRangeException("pos1D", "Thread tried to access pos " + pos1D + ". Array size is " + Size + ".");

                if (!written[pos1D])
                    return "NULL";

                T value = memory[pos1D];
                return value is double || value is float ? String.Format("{0:0.00}", value).Substring(0, 4).TrimEnd(',') : value.ToString();
            }
        }

        public override bool Equals(object obj)
        {
            if (!this.GetType().Equals(obj.GetType()))
                return false;

            Array<T> other = (Array<T>)obj;
            if (this.Length != other.Length) return false;
            for (int i = 0; i < this.Length; i++)
                if (!this[i].Equals(other[i]))
                    return false;

            return true;
        }

        public override int GetHashCode()
        {
            int result = this.Length;
            for (int i = 0; i < this.Length; i++)
                result += this[i].GetHashCode();
            return result;
        }
    }
}
