using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEnvironment
{
    public class NotifyingArr<T> : Arr<T>, INotifyingArray
    {
        public event EventHandler<ArrayEventArgs> OnRead;
        public event EventHandler<ArrayEventArgs> OnWrite;

        public NotifyingArr(int sizeX) : this(sizeX, 1, 1) { }

        public NotifyingArr(int sizeX, int sizeY) : this(sizeX, sizeY, 1) { }

        public NotifyingArr(int sizeX, int sizeY, int sizeZ) : base(sizeX, sizeY, sizeZ) { }

        public NotifyingArr(T[] values) : this(values.Length) { this.Write(values); }

        public NotifyingArr(T[] values, int w, int h) : this(w, h) { this.Write(values); }

        public new NotifyingArr<T> Clone()
        {
            NotifyingArr<T> clone = new NotifyingArr<T>(this.SizeX, this.SizeY, this.SizeZ);
            clone.memory = (T[])this.memory.Clone();
            clone.written = (bool[])this.written.Clone();
            return clone;
        }        

        protected override T read(int threadId, int pos1D)
        {
            if (OnRead != null)
                OnRead(this, new ArrayEventArgs(threadId, pos1D));
            return base.read(threadId, pos1D);
        }

        protected override void write(int threadId, int pos1D, T value)
        {
            if (OnWrite != null)
                OnWrite(this, new ArrayEventArgs(threadId, pos1D));
            base.write(threadId, pos1D, value);
        }
    }
}
