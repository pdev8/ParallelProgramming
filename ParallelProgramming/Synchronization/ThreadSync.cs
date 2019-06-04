using System;
using System.Threading;

namespace ParallelProgramming.Synchronization
{
    public class ThreadSync
    {
        private int counter = 0;

        public static void Run()
        {
            new ThreadSync().RunTest();
        }

        public void RunTest()
        {
            var t1 = new Thread(Incrementer)
            {
                IsBackground = true,
                Name = "ThreadOne"
            };

            t1.Start();
            Console.WriteLine($"Started thread {t1.Name} | {t1.ManagedThreadId}");

            var t2 = new Thread(Incrementer)
            {
                IsBackground = true,
                Name = "ThreadTwo"
            };

            t2.Start();
            Console.WriteLine($"Started thread {t2.Name} | {t2.ManagedThreadId}");

            t1.Join();
            t2.Join();

            Console.WriteLine("All my threads are finished.");
        }

        public void Incrementer()
        {
            try
            {
                while (this.counter < 10)
                {
                    lock (this)
                    {
                        var temp = this.counter;
                        ++temp;
                        this.counter = temp;

                        Thread.Sleep(1);

                        Console.WriteLine($"Thread {Thread.CurrentThread.Name}. Incrementer: {counter}. Thread Id: {Thread.CurrentThread.ManagedThreadId}");
                    }
                }
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.Name}! Cleaning up...");
            }
            finally
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.Name} Exiting.");
            }
        }
    }
}