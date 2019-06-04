using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgramming
{
    public class WaitTask
    {
        public static void Run()
        {
            WaitAll();
        }

        public static void DisarmBomb()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var task = new Task(() =>
            {
                Console.WriteLine("Press any key to disarm; you have 5 seconds");

                var cancelled = token.WaitHandle.WaitOne(5000);

                Console.WriteLine(cancelled ? "Bomb disarmed" : "Boom!!!");
            }, token);

            task.Start();

            Console.ReadKey();
            cts.Cancel();
        }

        public static void WaitAll()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var task = new Task(() =>
            {
                Console.WriteLine("I take 5 seconds");

                for (var i = 0; i < 5; i++)
                {
                    token.ThrowIfCancellationRequested();

                    Thread.Sleep(1000);
                }

                Console.WriteLine("I'm done.");
            }, token);

            task.Start();

            var task2 = Task.Factory.StartNew(() => Thread.Sleep(3000), token);

            Task.WaitAll(new[] {task, task2}, 4000, token);

            Console.WriteLine($"Task 'task' status is {task.Status}");
            Console.WriteLine($"Task 'task2' status is {task2.Status}");
        }
    }
}