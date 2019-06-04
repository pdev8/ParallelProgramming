using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParallelProgramming.Synchronization
{
    public class NonAtomicOperation
    {
        private readonly object lockObj = new object();

        private int Num { get; set; }

        public static void Run()
        {
            for (var i = 0; i < 200; i++)
            {
                RunTasks();
            }
        }

        public static void RunTasks()
        {
            var tasks = new List<Task>();

            var nonAtomic = new NonAtomicOperation();

            tasks.AddRange(new[]
            {
                new Task(() => {
                    lock (nonAtomic.lockObj)
                    {
                        nonAtomic.Num++;
                    }
                }),
                new Task(() => {
                    lock (nonAtomic.lockObj)
                    {
                        nonAtomic.Num++;
                    }
                }),
                new Task(() => {
                    lock (nonAtomic.lockObj)
                    {
                        nonAtomic.Num++;
                    }
                }),
                new Task(() => {
                    lock (nonAtomic.lockObj)
                    {
                        nonAtomic.Num++;
                    }
                }),
            });

            foreach (var task in tasks)
            {
                task.Start();
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine(nonAtomic.Num);
        }
    }
}