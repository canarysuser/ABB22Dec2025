using System;
using System.Collections;
using System.Threading.Channels;

namespace ConsoleApp1
{
    public class Program
    {
        public record Person(string FirstName, string LastName)
        {
            public string Address { get; set; } = "";
            public string City { get; init; } = "Bengaluru";
        }; 

        async static Task Main()
        {
            var p1 = new Person("ABB", "Bengaluru");
            var p2 = new Person("ABB", "Bengaluru");
            p1.Address = "New Address";
            //p2.Address = "New Address";
            // p.City = "Chennai";
            var (FirstName, City) = p1;

            Console.WriteLine($"p1==p2?? {p1 == p2}");

            var p3 = p1 with { FirstName = "New ABB" };
            Console.WriteLine(p3);


            Console.WriteLine(p1);
            Console.WriteLine($"{FirstName}, {City}");

            Product prd = new() { ProductName = "ABC", ProductId = 191 };
            var (productName, productId) = prd;
            Console.WriteLine($"{productId}, {productName}");

            Console.WriteLine("==============================================================");
            //ThreadExample1.Test(); 
            //ThreadSync.Test();
            // ResetEventSynch.Test();
            //TaskBasedProgramming.Test();
            //await ParallelProgramming.Test();
            //ReflectionExample.Test();
            //            ReflectionExample.TestDynamicAssembly();
            MEFClient.Test();
            /*string email = "someone@example.com";
            bool status = AppExtensions.IsValid(email);
            if (status)
            {
                Console.WriteLine($"{email} is valid.");
            } else { Console.WriteLine($"{email} is not valid."); }
            status = AppExtensions.CanSend(email);
            int result = Power(2, 10);
            Console.WriteLine($"Result of Power(2,10)=> {result}");

            //foreach(var n in Powers(2, 10))
            //{
            //    Console.WriteLine($"Main: {n}");
            //    if (n > 100) break;
            //}
            IEnumerator ie = Powers(2, 10).GetEnumerator();
            while(ie.MoveNext())
            {
                Console.WriteLine(ie.Current);
            }*/
            // DelegatesIntroduction.Test();
            //LINQOperators.Test();
            //Console.WriteLine(GetNumType(-10));
        }
        static string GetNumType(int n) => n switch
        {
            < 0 => "Negative",
            0 => "Zero",
            > 0 =>  "Positive"
        };
        static IEnumerable<int> Powers( int num, int multiplier)
        {
            /*int result = 1; 
            for(int i=0; i < multiplier; i++)
            {
                result *= num;
                Console.WriteLine($"FROM Powers: {result}");
                yield return result;
            }
            */
            yield return 1;
            yield return 2;
            yield return 3;
            yield return 4;
            yield return 5;
        }

        static int Power(int number, int multiplier)
        {
            int result = 1; 
            for(int i = 0; i < multiplier; i++)
            {
                result *= number;
                Console.WriteLine($"i=>{i}, result=>{result}");
            }
            return result;
        }
    }

    public static class AppExtensions
    {
        public static bool IsValid(this string input) { return input.Contains("@"); }
        public static bool CanSend(string input) {  return input.Contains("@"); }
    }
    public static class AppExtensions2
    {
        public static string IsValid(this string input2) { return input2; }
        public static bool CanSend(string input) { return input.Contains("@"); }
    }


}