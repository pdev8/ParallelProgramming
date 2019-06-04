using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgramming
{
    public class MultiThreadingDemo
    {
        public static void Run()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var task1 = Task1(cts);
            var task2 = Task2(cts);
            var task3 = Task3(cts);

            try
            {
                Task.WaitAll(new[] {task1, task2, task3}, 20000, token);
            }
            catch (AggregateException ae)
            {
                ae.Handle(e =>
                {
                    if (!(e is OperationCanceledException))
                    {
                        return false;
                    }

                    Console.WriteLine("Invalid op!");

                    return true;

                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");

                Thread.Sleep(3000);
            }

            Console.WriteLine($"Task 'task1' status is {task1.Status}");
            Console.WriteLine($"Task 'task2' status is {task2.Status}");
            Console.WriteLine($"Task 'task3' status is {task3.Status}");
        }

        public static Task Task1(CancellationTokenSource cts)
        {

            var task = new Task(() =>
            {
                Console.WriteLine("Task1 is starting");

                for (var i = 1; i <= 20; i++)
                {
                    cts.Token.ThrowIfCancellationRequested();

                    Console.WriteLine($"Task1: {i}");

                    Thread.Sleep(1000);
                }
            }, cts.Token);

            cts.Token.Register(() => Console.WriteLine($"Task1 status: {task.Status}"));

            task.Start();

            return task;
        }

        public static Task Task2(CancellationTokenSource cts)
        {
            var task = new Task(() =>
            {
                Console.WriteLine("Task2 is starting");

                for (var i = 1; i <= 15; i++)
                {
                    cts.Token.ThrowIfCancellationRequested();

                    Console.WriteLine($"Task2: {i}");

                    Thread.Sleep(1000);
                }
            }, cts.Token);

            cts.Token.Register(() => Console.WriteLine($"Task2 status: {task.Status}"));

            task.Start();

            return task;
        }

        public static Task Task3(CancellationTokenSource cts)
        {
            var task = new Task(() =>
            {
                Console.WriteLine("Task3 is starting");

                for (var i = 1; i <= 10; i++)
                {
                    cts.Token.ThrowIfCancellationRequested();

                    Console.WriteLine($"Task3: {i}");

                    Thread.Sleep(1000);
                }
            }, cts.Token);

            cts.Token.Register(() => Console.WriteLine($"Task3 status: {task.Status}"));

            task.Start();

            Task.Factory.StartNew(() =>
                {
                    task.GetAwaiter().OnCompleted(() =>
                    {
                        Console.WriteLine("Task3 completed");
                        cts.Cancel();
                    });
                });

            return task;
        }
    }
}