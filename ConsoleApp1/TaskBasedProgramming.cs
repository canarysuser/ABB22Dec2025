using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static System.Console; 

namespace ConsoleApp1
{
    internal class TaskBasedProgramming
    {
        internal static void Test()
        {
            WriteLine("Press a key to start. ");
            ReadKey();

            //TasksIntroductionTest();
            //TestTasksWithChaining(); 
            TestTaskCancellations();
            WriteLine("Press a key to terminate the application. ");
            ReadKey();


        }
        static void ShowThreadInfo(string taskName)
        {
            WriteLine($"{taskName} is running on \n\tthread id: {Thread.CurrentThread.ManagedThreadId} \n\tProcessor Id: {Thread.GetCurrentProcessorId()}\n\tIs Background: {Thread.CurrentThread.IsBackground}");
        }

        static Task CreateTask(string taskName,CancellationToken ct)
        {
            return new Task(() =>
            {
                ct.ThrowIfCancellationRequested();
                ShowThreadInfo(taskName);
                Task.Delay(TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();
                if (ct.IsCancellationRequested)
                {
                    WriteLine($"{taskName} cancellation is requested.");
                }
                ct.ThrowIfCancellationRequested();
                WriteLine($"{taskName} executed.");
            }, cancellationToken: ct);
        }
        static void TestTaskCancellations()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            var ct = tokenSource.Token; 
            Task t1 = CreateTask("Task 1", ct);
            Task t2 = CreateTask("Second Task", ct);
            t1.Start();
            t2.Start();
            Write("Do you want to cancel? [Y/N]");
            string input = ReadLine().ToUpper();
            if (input == "Y") tokenSource.Cancel(); 
            try
            {
                Task.WaitAll(t1, t2);
                WriteLine($"Task 1 status: {t1.Status.ToString()}");
                WriteLine($"Task 2 status: {t2.Status.ToString()}");
            } catch (OperationCanceledException oce)
            {
                WriteLine($"Canceled: {oce.Message}");
            } catch (AggregateException ae)
            {
                WriteLine($"Aggregate Exception: {ae.Message}");
                ae.InnerExceptions.ToList().ForEach(e => WriteLine(e.Message));
            }
            if (t1.IsCanceled)
                WriteLine("t1 is canceled.");
            if (t2.IsCanceled)
                WriteLine("t2 is canceled.");
        }
        static void TestTasksWithChaining()
        {
            var chainedTasks = 
                Task.Factory.StartNew(() => { ShowThreadInfo("Task 1"); WriteLine("Task 1 runs"); return 10; })
                .ContinueWith((res1) => { ShowThreadInfo("Task 2"); WriteLine("Task 2 runs"); return res1.Result * 123; })
                .ContinueWith((res2) => { ShowThreadInfo("Task 3"); WriteLine("Task 3 runs"); return res2.Result / 5; })
                .ContinueWith((res3) => { ShowThreadInfo("Task 4"); WriteLine($"Response from previous = {res3.Result}"); })
                .ContinueWith((err) => { ShowThreadInfo("Task 1"); WriteLine($"Error: {err.Exception?.Message}"); });
            Task.WaitAll(chainedTasks); 
        }

        internal static void TasksIntroductionTest()
        {
            
            ShowThreadInfo("Main"); 
            Task t1 = new Task(() => 
            {
                ShowThreadInfo("Task 1");
                WriteLine("First Task");
            });
            t1.Start();
            Task t2 = Task.Factory.StartNew(() =>
            {
                ShowThreadInfo("TaskFactory 1");
                WriteLine("Second task using Task.Factory.StartNew()");
            });
            Task t3 = Task.Run(() =>
            {
                ShowThreadInfo("TaskRun 1");
                WriteLine("Third task using Task.Run()");
            });
            Task<int> t4 = Task.Factory.StartNew<int>(() =>
            {
                ShowThreadInfo("TaskWith Return 1");
                Task.Delay(1000).GetAwaiter().GetResult();
                ShowThreadInfo("TaskWith Return 2");
                WriteLine("Fourth Task with return type");
                return DateTime.Now.Millisecond;
            });

            Task.WaitAll(t1, t2, t3, t4);
            WriteLine($"Task4 returned : {t4.Result}");

        }
    }
}
