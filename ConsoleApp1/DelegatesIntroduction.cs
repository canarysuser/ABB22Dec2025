using System;
using System.Collections.Generic;
using System.Text;
using static System.Console; 

namespace ConsoleApp1
{
    //Step1: Declaration 
    public delegate int ArithmeticDelegate(int num1, int num2);
    public class DelegatesIntroduction
    {
        public int Add(int a, int b) => a + b;
        public static int Subtract(int a, int b) { return a - b; }

        public static void Test()
        {
            DelegatesIntroduction di = new DelegatesIntroduction();
            //Step 2; 
            ArithmeticDelegate ad = new ArithmeticDelegate(di.Add);
            int x = 10, y = 20;
            int result = ad(x, y);
            WriteLine($" Result of ad({x}, {y}) is {result}");
            x += result; 
            y += result;
            result = ad.Invoke(x, y);
            WriteLine($"Result of ad.Invoke({x},{y}) => {result}");
            //Multicast 

            ad = ad + new ArithmeticDelegate(Subtract);
            // List<Delegate> has 2 items [ ad.Add, ad.Subtract ]
            // List<Delegate> has 2 items [ null, ad.Subtract ]

            x = 100; y = 150;
            result = ad.Invoke(y, x);
            WriteLine($"Result of ad.Invoke({y},{x}) => {result}");
            WriteLine("===============================================");
            
            //Anonymous Methods - unnamed methods 
            //avoid method name pollution 
            //access local variables 
            //inner functions 
            ad += delegate (int a, int b)
            {
                int z = x + y;
                return a + b;
            };
            //Lambda or Lambda expression 
            ad += (a, b) => (b > 0) ? a / b : 0;
            /*
             * Expression Lambdas -> single line of code - usually the return statement 
             * Statement Lambdas -> multiplie lines of code enclosed with {...} 
             * --> argument passing 
             * 1. Zero arguments => use () => .... 
             * 2. One argument => use (a) => ... OR a=>a+b;...
             * 3. Two or more arguments => use (a,b,....) => {... } 
             */
            ad += (a, b) =>
            {
                int z = a + b;
                return z;
            };

            ManualInvocation(ad);
            /* Built-In Delegates
             * 1. Action or Action<T1..T16>         -> invoke functions which do not return values 
             * 2. Func<TResult> or Func<T1..T16, TResult> -> invoke functions which returns value 
             * 3. Predicate<TInput>         -> takes an input and returns a boolean. 
             */
            Action a1 = () => WriteLine("Action delegte invoked.");
            Action<int, int> a2 = (a, b) => WriteLine($"a:{a}, b:{b}");
            a1.Invoke();
            a2(10, 20);
            Func<string> f1 = () => "Hello World";
            Func<int, int, bool> f2 = (a, b) => a > b; 
            WriteLine($"f1() => {f1()}");
            WriteLine($"f2(65,75) => {f2(65, 75)}");
            Predicate<string> p1 = (input) => input.Contains("a");
            WriteLine($"p1('Bengaluru') contains 'a' => {p1("Bengaluru")}");

            var numbers = Enumerable.Range(5, 35).ToList();
            ListMatcher(numbers, (a) => a %2==0);
            //ListMatcher(numbers, (a) => a > 10 && a < 20);


        }
        static void ListMatcher(List<int> numbers, Predicate<int> predicate)
        {
            foreach (int i in numbers)
            {
                WriteLine($"{i} > 10: {predicate(i)}");
            }
        }

        static void ManualInvocation(ArithmeticDelegate ad)
        {
            WriteLine($"\n Manual Invocation started...");
            int x = 999, y = 111;
            foreach (Delegate del in ad.GetInvocationList())
            {
                int result = (int) del.DynamicInvoke(x, y)!;
                WriteLine($"{del.Method.Name} returned {result}"); 
            }
    

        }
    }
}
