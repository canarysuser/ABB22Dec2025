using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using static System.Console;

namespace ConsoleApp1
{
    internal class ParallelProgramming
    {

        static string GetData()
        {
            Thread.Sleep(1000);
            return "Hello World";
        }
        static Task<string> GetDataAsync()
        {
            Task<string> t1 = Task.Factory.StartNew<string>(() =>
            {
                WriteLine("GetdataAsync");
                return GetData();
            });
            return t1;
        }
        
        async internal static Task Test()
        {
           var result= await GetDataAsync();
            WriteLine(result);
            ProductDbContext db = new ProductDbContext();
            var products = db.Products.Where(c=>c.CategoryId==1).ToListAsync().Result;
            products.ForEach(c => WriteLine(c));
            

            WriteLine("Press a key to start. ");
            ReadKey();

            /* Stopwatch watch = Stopwatch.StartNew();
             DrawCircle();
             DrawRectangle();
             DrawSquare();
             DrawTriangle();
             watch.Stop();
             ForegroundColor = ConsoleColor.Green;
             WriteLine($"Normal execution takes: {watch.ElapsedMilliseconds}");
             ResetColor();

             watch = Stopwatch.StartNew();
             Parallel.Invoke(
                 ()=>DrawCircle(),
                 () => DrawRectangle(),
                 () => DrawSquare(),
                 () => DrawTriangle()
                 );
             watch.Stop();
             ForegroundColor = ConsoleColor.DarkCyan;
             WriteLine($"Parallel execution takes: {watch.ElapsedMilliseconds}");
             ResetColor();*/

            SequentialKeyGeneration();
            ParallelKeyGeneration();

            WriteLine("Press a key to terminate the application. ");
            ReadKey();
        }
        static int MaxSize=1_000_000;

        static Func<byte[], string> HexToString = (a) =>
        {
            StringBuilder sb = new();
            for(int i=0; i<a.Length; i++)
            {
                sb.AppendFormat("{0:X}", a[i]);
            }
            return sb.ToString();
        };

        static void SequentialKeyGeneration()
        {
            WriteLine($"{nameof(SequentialKeyGeneration)} started.");
            Stopwatch watch = Stopwatch.StartNew();
            var aes = Aes.Create();
            List<string> keyString = new();
            for (int i=0; i < MaxSize; i++)
            {
                //var aes = Aes.Create();
                aes.GenerateIV();
                var key = aes.Key;
                //string _ = HexToString(key);
                keyString.Add(HexToString(key));

            }
            watch.Stop();
            WriteLine($"{nameof(SequentialKeyGeneration)} completed in {watch.ElapsedMilliseconds} ms.");
            WriteLine($"KeyStringList contains {keyString.Count()}");
        }
        static void ParallelKeyGeneration()
        {
            WriteLine($"{nameof(ParallelKeyGeneration)} started.");
            Stopwatch watch = Stopwatch.StartNew();
            var aes = Aes.Create();
            ConcurrentBag<string> keyString = new();
            Parallel.For(
                fromInclusive: 1,
                toExclusive: MaxSize+1, 
                body: (i) =>
            {
                aes.GenerateIV();
                var key = aes.Key;
                //string _ = HexToString(key);
                keyString.Add(HexToString(key));
            });
            watch.Stop();
            WriteLine($"{nameof(ParallelKeyGeneration)} completed in {watch.ElapsedMilliseconds} ms.");
            WriteLine($"KeyStringList contains {keyString.Count()}");
        }
        static void DrawCircle()
        {
            ShowThreadInfo("Circle");
            for (int i = 0; i < MaxSize; i++)
            {
                double k = i * i / Math.Sqrt(Math.PI * i);
            }
            Thread.Sleep(500);
            WriteLine($"{nameof(DrawCircle)}() called.");
        }
        static void DrawRectangle()
        {
            ShowThreadInfo("Rectangle");
            for (int i = 0; i < MaxSize; i++)
            {
                double k = i * i / Math.Sqrt(Math.PI * i);
            }
            Thread.Sleep(200);
            WriteLine($"{nameof(DrawRectangle)}() called.");
        }
        static void DrawSquare()
        {
            ShowThreadInfo("Square");
            for (int i = 0; i < MaxSize; i++)
            {
                double k = i * i / Math.Sqrt(Math.PI * i);
            }
            Thread.Sleep(900);
            WriteLine($"{nameof(DrawSquare)}() called.");
        }
        static void DrawTriangle()
        {
            ShowThreadInfo("Triangle");
            for (int i = 0; i < MaxSize; i++)
            {
                double k = i * i / Math.Sqrt(Math.PI * i);
            }
            Thread.Sleep(700);
            WriteLine($"{nameof(DrawTriangle)}() called.");
        }
        static void ShowThreadInfo(string taskName)
        {
            WriteLine($"{taskName} is running on \n\tthread id: {Thread.CurrentThread.ManagedThreadId} \n\tProcessor Id: {Thread.GetCurrentProcessorId()}\n\tIs Background: {Thread.CurrentThread.IsBackground}");
        }

    }
}
