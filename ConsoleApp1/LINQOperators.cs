using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using System.Text;
using static System.Console;

namespace ConsoleApp1
{
    public class LINQOperators
    {
        static List<string> cities = ["Bengaluru", "Chennai", "Mumbai", "Hyderabad", "Thiruvananthapura",
        "Kolkata", "Jaipur", "Lucknow", "Leh", "Shimla", "Gandhinagar", "Ranchi"];

        static string line = "".PadLeft(45, '=');
        static int counter = 1;
        static void PrintList(IEnumerable<string> list, string header)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(line)
              .AppendLine($"             {header}")
              .AppendLine(line);
            foreach (string c in list)
            {
                sb.Append($"{c}, ");
            }
            sb.AppendLine($"\n{line}");
            WriteLine(sb.ToString());
        }
        internal static void Test()
        {
            BasicQuery();
            ProjectionOperators();
            RestrictionOperators();
            SortingOperators();
            AggregationOperators();
            GroupingOperators();
            PartitionOperators();
            ElementOperators();
            DbQuery();

            void DbQuery()
            {
                ProductDbContext db = new ProductDbContext();
                var q1 = from item in db.Products.AsNoTracking()
                         where item.ProductName.Contains("es")
                         orderby item.UnitsInStock
                         select item;
                foreach (var item in q1)
                    WriteLine(item);
                var q2 = db.Products.AsNoTracking()
                    .ToList()
                    .Where(c => c.ProductName.Contains("es", StringComparison.OrdinalIgnoreCase))
                    .Select(c => c);
                foreach (var item in q2)
                    WriteLine(item);
            }
        }
        static void ElementOperators()
        {
            var first = cities.First();
            var last = cities.Last();
            var firstMatching = cities.FirstOrDefault(c => c.Length > 20);
            var lastMatching = cities.Last(c => c.Length > 10);
            WriteLine($"First: {first}, Last:{last}");
            WriteLine($"First Matching: {(firstMatching == null ? "NA" : firstMatching)}, Last Matching:{lastMatching}");
        }
        static void PartitionOperators()
        {
            var q1 = (from c in cities select c).Take(5);
            PrintList(q1, $"{counter++}. Take 5");
            var q2 = cities.Skip(5);
            PrintList(q2, $"{counter++}. Skip 5");

            var q3 = cities.Where(c => c.Length > 5)
                .OrderBy(c => c[0])
                .Skip(2)
                .Take(1);
            PrintList(q3, $"{counter++}. Multiple Operators");

        }
        static void GroupingOperators()
        {
            var q1 = from c in cities
                     orderby c
                     group c by new { Letter = c[0], Length = c.Length } into g
                     select g;
            foreach (var g in q1)
            {
                WriteLine($"Group Key: {g.Key}");
                PrintList(g.ToList(), $"{counter++}. Items in Key {g.Key}");
            }

        }
        static void AggregationOperators()
        {
            var count = cities.Count();
            var totalLength = cities.Sum(c => c.Length);
            var minLength = cities.Min(c => c.Length);
            var maxLength = cities.Max(c => c.Length);
            var avgLength = cities.Average(c => c.Length);
            WriteLine($"Count:{count}, Total:{totalLength},Min:{minLength}, Max:{maxLength},Avg:{avgLength}");
        }
        static void SortingOperators()
        {
            var q1 = from c in cities
                     orderby c[0] descending, c[1]
                     select c;
            PrintList(q1, $"{counter++}. Ordered Query");
            var q2 = cities.OrderBy(c => c[0]).ThenByDescending(c => c[1]);
            PrintList(q2, $"{counter++}. Ordered Query");
        }
        static void RestrictionOperators()
        {
            //WHERE 
            var q1 = from c in cities
                     where c.Length > 10
                     select c;
            PrintList(q1, "Restriction criteria, Length > 10");
            var q2 = cities.Where(c => c.Length < 10).Select(c => c);
            PrintList(q2, "Restriction criteria, Length < 10");
            var q3 = from c in cities
                     where c.StartsWith("B") || c.StartsWith("C")
                     select c;
            PrintList(q3, "Restriction criteria, Startwith 'C' or 'B'");

        }
        static void ProjectionOperators()
        {
            //var q1 = from c in cities
            //         select new
            //         {
            //             Name = c,
            //             InitialLetter = c[0],
            //             c.Length
            //         };
            var q1 = cities.Select(c => new
            {
                Name = c,
                InitialLetter = c[0],
                c.Length
            });
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(line)
              .AppendLine($"             Projection Operators")
              .AppendLine(line);
            foreach (var c in q1)
            {
                sb.AppendLine($"{c.InitialLetter} | {c.Length:00} | {c.Name}");
            }
            sb.AppendLine($"\n{line}");
            WriteLine(sb.ToString());
        }
        static void BasicQuery()
        {
            //1. Query Syntax 
            var q1 = from c in cities
                     select c;
            PrintList(q1, $"{counter++}. Basic Query Syntax");
            //2. Method Syntax
            var q2 = cities.Select(c => c);
            PrintList(q2, $"{counter++}. Basic Method Syntax");

        }

    }
}
