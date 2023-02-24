using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgileObjects.ReadableExpressions;
using Bogus;
using Bogus.Extensions;
using MarketPlaceCrm.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;

namespace Sandbox
{
    class Program
    {
        public static string RemoveAllWhiteSpace(string source)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrWhiteSpace(source))
                return source;

            var result = source.Where(x => !char.IsWhiteSpace(x))
                .Select(x => x)
                .ToArray();

            return new string(result);
        }

        public static bool CheckIsValid(string source)
        {
            if (string.IsNullOrEmpty(source))
                return false;
            
            return true;
        }
        public static bool Check(string s, string p1, string p2)
        {
            if (!CheckIsValid(s) || !CheckIsValid(p1) || !CheckIsValid(p2))
                return false;

            // trim all string
            var sWithoutWitheSpaces = RemoveAllWhiteSpace(s);
            var p1WithoutWitheSpaces = RemoveAllWhiteSpace(p1);
            var p2WithoutWitheSpaces = RemoveAllWhiteSpace(p2);

            if (sWithoutWitheSpaces.Length != p1WithoutWitheSpaces.Length + p2WithoutWitheSpaces.Length) return false;

            if (sWithoutWitheSpaces.Equals(p1WithoutWitheSpaces + p2WithoutWitheSpaces))
                return true;

            int counterP1 = 0, counterP2 = 0;
            for (int i = 0; i < sWithoutWitheSpaces.Length; i++)
            {
                //s = hello P1 = hlo P2 = el
                var sLetter = sWithoutWitheSpaces[i];

                char p1Letter = '0';
                if(counterP1 < p1WithoutWitheSpaces.Length)
                    p1Letter = p1WithoutWitheSpaces[counterP1];
                
                char p2Letter = '0';
                if(counterP2 < p2WithoutWitheSpaces.Length)
                    p2Letter = p2WithoutWitheSpaces[counterP2];
                
                if (p1Letter == sLetter && p1Letter  != '0')
                {
                    counterP1++;
                }

                else if (p2Letter == sLetter && p2Letter != '0')
                {
                    counterP2++;
                }
                
                else return false;
            }
            
            return true;
        }

        static void print()
        {
           
        }

        // Id = 1,
        // Id = 2,
        // Id = 3,
        static void Main(string[] args)
        {
            using (var ctx = new AppDbContextFactory().CreateDbContext(new[] { "dev" }))
            {
                ctx.Products.Add(new Product() { Id = 2, Name = "aiphone 11" });
                //
                // ctx.Categories.Add(new Category() {Name = "electronic" });
                // ctx.Categories.Add(new Category() {Name = "computer" });
                // ctx.Categories.Add(new Category() {Name = "notebook" });
                //
                // ctx.ProductCategories.Add(new ProductCategory() { ProductId = 1, CategoryId = 1 });

                var pc = ctx.ProductCategories.FirstOrDefault(x => x.ProductId == 1 && x.CategoryId == 1);
                pc.CategoryId = 2;
                ctx.SaveChanges();
            }
            
            return;
            // using (var ctx = new AppDbContextFactory().CreateDbContext(new[] { "dev" }))
            // {
            //     var customer = new Customer
            //     {
            //         UserName = "test",
            //         Orders = new List<Order>()
            //         {
            //             new Order { Item = "t-shirt", Quantity = 1, },
            //             new Order { Item = "hoodie", Quantity = 3, },
            //             new Order { Item = "pant", Quantity = 2, }
            //         }
            //     };
            //     ctx.Add(customer);
            //     ctx.SaveChanges();
            //
            //     ctx.Entry(customer).State = EntityState.Detached;
            //
            //     // foreach (var o in customer.Orders)
            //     // {
            //     //     ctx.Entry(o).State = EntityState.Detached;
            //     // }
            //
            //     var customerEdit = new Customer() { Id = 1, UserName = "update test 2" };
            //     var orders = new List<Order>
            //     {
            //         new Order
            //         {
            //             OrderId = 1, Item = "t-shirt", Quantity = 100,
            //             CustomerId = customerEdit.Id
            //         },
            //         new Order
            //         {
            //             OrderId = 2, Item = "hoodie", Quantity = 100,
            //             CustomerId = customerEdit.Id
            //         },
            //         new Order
            //         {
            //             OrderId = 3, Item = "pant", Quantity = 100,
            //             CustomerId = customerEdit.Id
            //         },
            //     };
            //
            //     ctx.Attach(customerEdit);
            //     ctx.Entry(customerEdit).State = EntityState.Modified;
            //
            //     ctx.SaveChanges();
            //
            //     var customers = ctx.Customers.AsNoTracking()
            //         .Include(x => x.Orders)
            //         .ToList();
            //
            //     foreach (var c in customers)
            //     {
            //         Console.WriteLine($"{c.Id} - {c.UserName}: ");
            //         foreach (var o in c.Orders)
            //         {
            //             Console.WriteLine($"  {o.OrderId} - {o.Item} - {o.Quantity} - {o.CustomerId}");
            //         }
            //     }
            // }
        }
    }


    //
    public static class ExtensionsForTesting
    {
        public static void Dump(this object obj)
        {
            Console.WriteLine(obj.DumpString());
        }

        public static string DumpString(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}