using System;
using System.Threading;

namespace ParallelProgramming.Synchronization
{
    public class SpinLockOperation
    {
        private static SpinLock sl = new SpinLock(true);

        public static void Run()
        {
            ////var lockTaken = false;
            ////LockRecursionWorkAround(5, ref lockTaken);

            LockRecursion(5);
        }

        public static void LockRecursionWorkAround(int x, ref bool lockTaken)
        {
            try
            {
                if (sl.IsHeld)
                {
                    sl.Exit();
                    lockTaken = false;
                }

                sl.Enter(ref lockTaken);

                if (x == 0)
                {
                    return;
                }

                LockRecursionWorkAround(x - 1, ref lockTaken);
            }
            catch (LockRecursionException e)
            {
                Console.WriteLine($"Exception: {e}");
            }
            finally
            {
                if (lockTaken && sl.IsHeld)
                {
                    Console.WriteLine($"Took a lock, x = {x}");

                    sl.Exit();
                }
                else
                {
                    Console.WriteLine($"Failed to take a lock, x = {x}");
                }
            }
        }

        public static void LockRecursion(int x)
        {
            var lockTaken = false;

            try
            {
                sl.Enter(ref lockTaken);
            }
            catch (LockRecursionException e)
            {
                Console.WriteLine($"Exception: {e}");
            }
            finally
            {
                if (lockTaken)
                {
                    Console.WriteLine($"Took a lock, x = {x}");

                    LockRecursion(x - 1);

                    sl.Exit();
                }
                else
                {
                    Console.WriteLine($"Failed to take a lock, x = {x}");
                }
            }
        }
    }
}
