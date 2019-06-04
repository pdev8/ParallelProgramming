using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParallelProgramming.Synchronization
{
    public class BankAccount
    {
        public object padlock = new object();

        public int Balance { get; set; }

        public void Deposit(int amount)
        {
            lock (padlock)
            {
                this.Balance += amount;
            }
        }

        public void Withdraw(int amount)
        {
            lock (padlock)
            {
                this.Balance -= amount;
            }
        }

        public void Transfer(BankAccount where, int amount)
        {
            this.Balance -= amount;
            where.Balance += amount;
        }

        public static void Run()
        {
            var tasks = new List<Task>();

            var bankAccount = new BankAccount();

            for (var i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < 1000; j++)
                    {
                        bankAccount.Deposit(100);
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < 1000; j++)
                    {
                        bankAccount.Withdraw(100);
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Final balance is {bankAccount.Balance}.");
        }
    }
}