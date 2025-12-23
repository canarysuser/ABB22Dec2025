using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    internal class ResetEventSynch
    {
        static AutoResetEvent even = new AutoResetEvent(false);
        static AutoResetEvent odd = new AutoResetEvent(false);

        static void GenerateOddNumbers()
        {
            var name = Thread.CurrentThread.Name;
            for (int i = 1; i < 100; i += 2)
            {
                Console.Write($"{name}:{i:000} ");
                odd.Set();
                even.WaitOne();
            }
        }
        static void GenerateEvenNumbers()
        {
            var name = Thread.CurrentThread.Name;
            for (int i = 2; i <= 100; i += 2)
            {
                odd.WaitOne();
                Console.Write($"{name}:{i:000} ");
                even.Set();
            }
        }
        internal static void Test()
        {
            Console.WriteLine("Reset Event Synchronization Test...");
            Thread oddThread = new Thread(GenerateOddNumbers) { Name = "1" };
            Thread evenThread = new Thread(GenerateEvenNumbers) { Name = "2" };
            oddThread.Start();
            evenThread.Start();
            Console.WriteLine("All threads started. Press a key to terminate...");
            Console.ReadKey();
            even.Close();
            odd.Close();
        }
    }
}
