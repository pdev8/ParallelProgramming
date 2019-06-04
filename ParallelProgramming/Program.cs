using System;
using System.Threading.Tasks;
using ParallelProgramming.Synchronization;

namespace ParallelProgramming
{
    class Program
    {
        static void Main(string[] args)
        {
            MutexOperation.Run();

            Console.WriteLine("Main program finished.");
            Console.ReadKey();
        }

        static int TextLength(object o)
        {
            Console.WriteLine($"\nTask with id {Task.CurrentId} processing object {o}...");

            return o.ToString().Length;
        }
    }
}