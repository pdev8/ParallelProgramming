using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgramming.Synchronization
{
    public class MutexOperation
    {
        public static void Run()
        {
            var tasks = new List<Task>();
            var ba = new BankAccount();
            var ba2 = new BankAccount();

            var mutex = new Mutex();
            var mutex2 = new Mutex();

            for (var i = 0; i < 1; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < 5; j++)
                    {
                        var hasLock = mutex.WaitOne();

                        try
                        {
                            ba.Deposit(1);
                        }
                        finally
                        {
                            if (hasLock)
                            {
                                mutex.ReleaseMutex();
                            }
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < 5; j++)
                    {
                        var hasLock = mutex2.WaitOne();

                        try
                        {
                            ba2.Deposit(1);
                        }
                        finally
                        {
                            if (hasLock)
                            {
                                mutex2.ReleaseMutex();
                            }
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < 5; j++)
                    {
                        var hasLock = WaitHandle.WaitAll(new[] { mutex, mutex2 });

                        try
                        {
                            ////ba2.Transfer(ba, 1);
                            Console.WriteLine($"Balance: Account1 - {ba.Balance} | Account2 - {ba2.Balance}");
                        }
                        finally
                        {
                            if (hasLock)
                            {
                                mutex.ReleaseMutex();
                                mutex2.ReleaseMutex();
                            }
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Final balance of bank account 2: {ba2.Balance}");
            Console.WriteLine($"Final balance of bank account: {ba.Balance}");
        }
    }
}
