using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEnvironment
{
    public interface INotifyingArray
    {
        event EventHandler<ArrayEventArgs> OnRead;
        event EventHandler<ArrayEventArgs> OnWrite;
    }
}
