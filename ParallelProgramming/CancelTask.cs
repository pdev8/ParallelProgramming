using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgramming
{
    public class CancelTask
    {
        public static void Run()
        {
            LinkedCancellationToken();
        }

        public static void SoftExit()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            token.Register(() =>
            {
                Console.WriteLine("Cancellation has been requested."); 
            });

            var task = new Task(() =>
            {
                var i = 0;

                while (true)
                {
                    token.ThrowIfCancellationRequested();

                    Console.WriteLine($"{i++}");
                }
            }, token);

            task.Start();

            Task.Factory.StartNew(() =>
            {
                token.WaitHandle.WaitOne();

                Console.WriteLine("Wait handle released, cancellation was requested");
            }, token);

            Console.ReadKey();
            cts.Cancel();
        }

        public static void LinkedCancellationToken()
        {
            var planned = new CancellationTokenSource();
            var preventative = new CancellationTokenSource();
            var emergency = new CancellationTokenSource();

            var paranoid =
                CancellationTokenSource.CreateLinkedTokenSource(planned.Token, preventative.Token, emergency.Token);

            planned.Token.Register(() => { Console.WriteLine("Planned cancellation token requested."); });
            preventative.Token.Register(() => { Console.WriteLine("Preventative cancellation token requested."); });
            emergency.Token.Register(() => { Console.WriteLine("Emergency cancellation token requested."); });

            Task.Factory.StartNew(() =>
            {
                var i = 0;

                while (true)
                {
                    paranoid.Token.ThrowIfCancellationRequested();

                    Console.WriteLine($"{i++}");

                    Thread.Sleep(1000);
                }
            }, paranoid.Token);

            var keyed = Console.ReadKey();

            if (keyed.KeyChar.ToString().Equals("P", StringComparison.OrdinalIgnoreCase))
            {
                planned.Cancel();
            }
            else if (keyed.KeyChar.ToString().Equals("R", StringComparison.OrdinalIgnoreCase))
            {
                preventative.Cancel();
            }
            else if (keyed.KeyChar.ToString().Equals("E", StringComparison.OrdinalIgnoreCase))
            {
                emergency.Cancel();
            }
        }
    }
}