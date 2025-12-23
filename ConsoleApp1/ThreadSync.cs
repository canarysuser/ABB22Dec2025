using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static System.Console; 

namespace ConsoleApp1
{
    internal class ThreadSync
    {
        static List<string> threadNames = ["_First_", "_Second_", "_Third_", "_Fourth_", "_Fifth_"];
        private int counter = 0; 
        private Object syncLockObject = new Object();
        static Mutex mtx; 
        static Semaphore sem;

        internal static void Test()
        {
            WriteLine("Starting Thread Synchronization Test...");
            ReadKey(); 

           /* mtx = new Mutex(
                initiallyOwned: false,
                //initialleyOwned: true if the calling thread should have initial ownership of the mutex
                //otherwise false
                name: "SystemWideName", 
                //Named mutex can be used across processes
                //Unnamed mutex can be used within a single process
                createdNew: out bool createdNew
                //true if the calling thread created and owns the mutex
                //false if the named mutex already existed
                );*/

            sem = new Semaphore(
                initialCount: 0,
                //The initial number of requests that can be granted concurrently
                maximumCount: 5,
                //The maximum number of requests that can be granted concurrently
                name: "SystemWideSemaphore",
                //Named semaphore can be used across processes
                //Unnamed semaphore can be used within a single process
                createdNew: out bool createdNew
                //true if the calling thread created and owns the semaphore
                //false if the named semaphore already existed
                );

            ThreadSync ts = new ThreadSync();
            Thread[] thArray = new Thread[threadNames.Count];
            for (int i = 0; i < threadNames.Count; i++)
            {
                //thArray[i] = new Thread(ts.RunDefault);
                //thArray[i] = new Thread(ts.RunInterlocked);
                //thArray[i] = new Thread(ts.RunLock);
                //thArray[i] = new Thread(ts.RunMutex);
                //thArray[i] = new Thread(ts.RunSemaphore);
                thArray[i] = new Thread(ts.RunWithAttribute);
                thArray[i].Name = threadNames[i];
                thArray[i].Start();
            }
            //mtx.ReleaseMutex();
            sem.Release(2);
            WriteLine("Main thread waiting for worker threads to complete...");
            ReadKey();
        }

        [
            System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)
        ]
        private void RunWithAttribute()
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution of {nameof(RunWithAttribute)}.");
            
            //CRITICAL SECTION STARTS
            Process p = Process.GetCurrentProcess();
            WriteLine($"{name} => Process ID: {p.Id}, {p.ProcessName}, {p.PeakWorkingSet64}");
            Thread.Sleep(millisecondsTimeout: 5000);
            WriteLine($"Thread {name} exiting the critical section in {p.ProcessName}");
            //CRITICAL SECTION ENDS

            WriteLine($"Thread {name} exiting {nameof(RunWithAttribute)}.");
        }
        private void RunSemaphore()
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution of {nameof(RunSemaphore)}.");
            sem.WaitOne(); //Acquire ownership of the semaphore

            //CRITICAL SECTION STARTS
            Process p = Process.GetCurrentProcess();
            WriteLine($"{name} => Process ID: {p.Id}, {p.ProcessName}, {p.PeakWorkingSet64}");
            Thread.Sleep(millisecondsTimeout: 5000);
            WriteLine($"Thread {name} exiting the critical section in {p.ProcessName}");
            //CRITICAL SECTION ENDS

            sem.Release(releaseCount: 1); //Release ownership of the semaphore
            WriteLine($"Thread {name} exiting {nameof(RunSemaphore)}.");
        }
        private void RunMutex()
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution of {nameof(RunMutex)}.");
            mtx.WaitOne(); //Acquire ownership of the mutex
            
            //CRITICAL SECTION STARTS
            Process p = Process.GetCurrentProcess();
            WriteLine($"{name} => Process ID: {p.Id}, {p.ProcessName}, {p.PeakWorkingSet64}");
            Thread.Sleep(millisecondsTimeout: 5000);
            WriteLine($"Thread {name} exiting the critical section in {p.ProcessName}");
            //CRITICAL SECTION ENDS

            mtx.ReleaseMutex(); //Release ownership of the mutex
            WriteLine($"Thread {name} exiting {nameof(RunMutex)}.");
        }
        private void RunLock()
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution of {nameof(RunLock)}.");
            while (counter < 100)
            {
                //Monitor.Enter and Monitor.Exit are called implicitly
                lock (syncLockObject)
                { 
                    int temp = counter;
                    temp++;
                    WriteLine($"Thread {name} incremented counter to {counter}");
                    Thread.Sleep(1);
                    counter = temp;
                };
            }
            WriteLine($"Thread {name} exiting {nameof(RunLock)}.");
        }
        private void RunMonitor()
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution of {nameof(RunMonitor)}.");
            while (counter < 100)
            {
                //Monitor.Enter(syncLockObject); //Set the SyncLockBit of the Object
                if (Monitor.TryEnter(syncLockObject, millisecondsTimeout: 50))
                {
                    int temp = counter;
                    temp++;
                    WriteLine($"Thread {name} incremented counter to {counter}");
                    Thread.Sleep(1);
                    counter = temp;
                    Monitor.PulseAll(syncLockObject); //Notify other waiting threads
                    Monitor.Exit(syncLockObject); //Unset the SyncLockBit of the Object
                } else
                {
                    WriteLine($"Thread {name} could not acquire the lock, retrying...");
                    break;
                }
            }
            WriteLine($"Thread {name} exiting {nameof(RunMonitor)}.");
        }
        private void RunInterlocked()
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution of {nameof(RunDefault)}.");
            while (counter < 100)
            {
                Interlocked.Increment(ref counter);
                //Increment, Decrement, Add, Exchange, CompareExchange methods of Interlocked class
                WriteLine($"Thread {name} incremented counter to {counter}");
                Thread.Sleep(1);
            }
            WriteLine($"Thread {name} exiting {nameof(RunDefault)}.");
        }

        private void RunDefault()
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution of {nameof(RunDefault)}.");
            while(counter < 100)
            {
                int temp = counter;
                temp++; 
                WriteLine($"Thread {name} incremented counter to {counter}");
                Thread.Sleep(1);
                counter = temp;
            }
            WriteLine($"Thread {name} exiting {nameof(RunDefault)}.");
        }
    }
}
