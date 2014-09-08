using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExecutionEnvironment
{
    public class Parallel
    {
        public class RunAction
        {
            int index = 0;
            Action<int> action = null;

            public RunAction(int index, Action<int> action)
            {
                this.index = index;
                this.action = action;
            }

            public static void Run(object runAction)
            {
                (runAction as RunAction).action((runAction as RunAction).index);
            }
        }

        public static void ForSerial(int fromInclusive, int toExclusive, Action<int> action)
        {
            int threadCount = toExclusive - fromInclusive;
            for (int i = 0; i < threadCount; i++)
                action(i + fromInclusive);
        }

        public static void For(int fromInclusive, int toExclusive, Action<int> action)
        {
            int threadCount = toExclusive - fromInclusive;
            Thread[] threads = new Thread[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                RunAction runAction = new RunAction(i + fromInclusive, action);
                threads[i] = new Thread(RunAction.Run);
                threads[i].Start(runAction);
            }

            for (int i = 0; i < threadCount; i++)
                threads[i].Join();

            Memory.Instance.MemoryFence();
        }

        public static void For(int fromInclusive, int toExclusive, Action<int> action, bool fence)
        {
            int threadCount = toExclusive - fromInclusive;
            Thread[] threads = new Thread[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                RunAction runAction = new RunAction(i + fromInclusive, action);
                threads[i] = new Thread(RunAction.Run);
                threads[i].Start(runAction);
            }

            for (int i = 0; i < threadCount; i++)
                threads[i].Join();

            if (fence)
                Memory.Instance.MemoryFence();
        }

        public static void For(int fromInclusiveX, int toExclusiveX, int fromInclusiveY, int toExclusiveY, Action<int, int> action)
        {
            For(fromInclusiveX, toExclusiveX, delegate(int x)
            {
                For(fromInclusiveY, toExclusiveY, delegate(int y)
                {
                    action(x + fromInclusiveX, y + fromInclusiveY);
                }, false);
            }, false);
            Memory.Instance.MemoryFence();
        }
    }
}
