using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using static System.Console; 

namespace ConsoleApp1
{
    internal class ThreadExample1
    {
        internal static void Test()
        {
            ThreadStart ts1 = new ThreadStart(Run);
            Thread th1 = new Thread(ts1);
            th1.Name = "First";
            th1.Start(); //pohysical OS thread is created at this point 
            //ThreadStart ts2 = Run;  ThreadStart ts2 = new ThreadStart(Run);
            Thread th2 = new Thread(Run);
            th2.Name = "Second";
            th2.Start();
            Thread th3 = new Thread(() =>
            {
                var name = Thread.CurrentThread.Name;
                WriteLine($"Thread {name} begins execution.");
                Thread.Sleep(millisecondsTimeout: 2000);
                WriteLine($"Thread {name} exiting...");
            });
            th3.Name = "Third";
            th3.Start();
            ParameterizedThreadStart ps1 = RunWithState!;
            Thread th4 = new Thread(ps1);
            th4.Name = "Fourth";
            th4.Start(9999); 

            WriteLine("All threads started. Press a key to terminate.");
            ReadKey(); 
        }
        static void RunWithState(object state)
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution.");
            if(int.TryParse(state.ToString(), out int number))
            {
                WriteLine($"Received number {number} as input"); 
            } else
            {
                WriteLine($"Received unknown type {number} as input."); 
            }
            Thread.Sleep(millisecondsTimeout: 2000);
            WriteLine($"Thread {name} exiting...");
        }
        static void Run()
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution.");
            Thread.Sleep(millisecondsTimeout: 2000);
            WriteLine($"Thread {name} exiting...");
        }
    }
}
