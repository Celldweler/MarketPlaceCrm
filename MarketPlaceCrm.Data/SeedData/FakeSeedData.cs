using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bogus;
using MarketPlaceCrm.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlaceCrm.Data.SeedData
{
    public static class FakeSeedData
    {
        public static List<Product> _fakeProducts { get; set; } = new List<Product>();
        public static List<Stock> _fakeStocks { get; set; } = new List<Stock>();
        public static List<Order> _fakeOrders { get; set; } = new List<Order>();
        public static List<Shipper> _fakeShippers { get; set; } = new List<Shipper>();
        public static List<Customer> _fakeCustomers { get; set; } = new List<Customer>();
        public static List<OrderDetail> _fakeOrderDetails { get; set; } = new List<OrderDetail>();

        private static int StockIdInc = 1;
        private static int OrderIdInc = SeedDataConstants.Order.SeedOrderId;

        public static void Seed(this ModelBuilder builder)
        {
            initState(builder);
            // SeedProducts(builder.Entity<Product>());
            // SeedCustomres(builder.Entity<Customer>());
            // SeedOrders(builder.Entity<Order>());
            // SeedOrderDetails(builder.Entity<OrderDetail>());
            // SeedStocks(builder.Entity<Stock>());
            // SeedShippers(builder.Entity<Shipper>());
            
            // SeedCategories(builder.Entity<Category>());
        }

        private static void SeedCategories(EntityTypeBuilder<Category> builder)
        {
           
        }

        private static List<Stock> GenTestsStocks(int productId)
        {
            var fakeStocks = new List<Stock>();
            fakeStocks.AddRange(new List<Stock>()
            {
                new Stock()
                {
                    Id = StockIdInc++, Created = DateTime.Now, Description = "S", Qty = 100,
                    ProductId = productId
                },
                new Stock()
                {
                    Id = StockIdInc++, Created = DateTime.Now, Description = "M", Qty = 100,
                    ProductId = productId
                },
                new Stock()
                {
                    Id = StockIdInc++, Created = DateTime.Now, Description = "L", Qty = 100,
                    ProductId = productId
                },
                new Stock()
                {
                    Id = StockIdInc++, Created = DateTime.Now, Description = "XL", Qty = 100,
                    ProductId = productId
                },
            });

            return fakeStocks;
        }

        private static void initState(ModelBuilder builder)
        {
            if (_fakeProducts != null && _fakeProducts.Count == 0)
            {
                var fakeId = SeedDataConstants.Product.SeedID;
                var rnd = new Random();
                var fakeProducts = new Faker<Product>()
                    .RuleFor(o => o.Id, x => fakeId++)
                    .RuleFor(o => o.Name, x => x.Lorem.Sentence(5))
                    .RuleFor(o => o.Description, x => x.Lorem.Sentence(10, 15))
                    .RuleFor(o => o.Cost, GetRoundPrice)
                    .RuleFor(o => o.Created, GenRandomDate)
                    // .RuleFor(o => o.Stocks, x => GenTestsStocks(fakeId - 1))
                    .Generate(SeedDataConstants.Product.Count);

                for (int i = 0; i < SeedDataConstants.Product.Count; i++)
                {
                    _fakeStocks.AddRange(GenTestsStocks(SeedDataConstants.Product.SeedID + i));
                }

                builder.Entity<Product>()
                    .HasData(fakeProducts);

                builder.Entity<Stock>()
                    .HasData(_fakeStocks);
            }

            if (_fakeCustomers != null && _fakeCustomers.Count == 0)
            {
                var custid = SeedDataConstants.Customers.SeedID;
                var rnd = new Random();

                var cutomers = new Faker<Customer>()
                    .RuleFor(o => o.Id, x => custid++)
                    .RuleFor(o => o.Name, x => x.Person.FirstName)
                    .RuleFor(o => o.Email, x => x.Person.Email)
                    .RuleFor(o => o.PhoneNumber, x => x.Person.Phone)
                    .RuleFor(o => o.LastName, x => x.Person.LastName)
                    .RuleFor(o => o.Address, x => x.Person.Address.Street)
                    .RuleFor(o => o.City, x => x.Person.Address.City)
                    .RuleFor(o => o.ZipCode, x => x.Person.Address.ZipCode)
                    // .RuleFor(o => o.Orders, GenOrdersForCustomerWithId(custid - 1, rnd.Next(0, 5)))
                    .Generate(SeedDataConstants.Customers.Count);

                custid = SeedDataConstants.Customers.SeedID;
                for (int i = 0; i < SeedDataConstants.Customers.Count; i++)
                {
                    var orders = GenOrdersForCustomerWithId(custid + i, rnd.Next(0, 5));
                    if(orders.Count > 0)
                        _fakeOrders.AddRange(orders);
                }

                builder.Entity<Customer>()
                    .HasData(cutomers);

                builder.Entity<Order>()
                    .HasData(_fakeOrders);

                builder.Entity<OrderDetail>()
                    .HasData(_fakeOrderDetails);
            }
        }

        private static List<Order> GenOrdersForCustomerWithId(int custId, int count)
        {
            if (count == 0) return new List<Order>();

            var rnd = new Random();
            var orders = new Faker<Order>()
                .RuleFor(o => o.Id, x => OrderIdInc++)
                .RuleFor(o => o.Created, GenRandomDate)
                .RuleFor(o => o.CustomerID, custId)
                // .RuleFor(o => o.OrderDetails, GenOrderDetails(rnd.Next(1, 3), OrderIdInc-1))
                .Generate(count);

            for (int i = 0; i < count; i++)
            {
                var details = GenOrderDetails(rnd.Next(1, 3), SeedDataConstants.Order.SeedOrderId + i);
                _fakeOrderDetails.AddRange(details);
            }

            return orders;
        }

        private static void SeedOrderDetails(EntityTypeBuilder<OrderDetail> builder)
        {
            var id = SeedDataConstants.OrderDetail.SeedID;
            var minOrderID = SeedDataConstants.Order.SeedOrderId;
            var maxOrderID = SeedDataConstants.Order.SeedOrderId + SeedDataConstants.OrderDetail.Count - 1;
            var minProductId = SeedDataConstants.Product.SeedID;
            var maxProductId = SeedDataConstants.Product.Count;

            var details = new Faker<OrderDetail>()
                .RuleFor(o => o.Id, x => id++)
                .RuleFor(o => o.Qty, x => x.Random.Int(1, 5))
                .RuleFor(o => o.OrderID, x => x.Random.Int(minOrderID, maxOrderID))
                .RuleFor(o => o.ProductID, x => x.Random.Int(minProductId, maxProductId))
                .Generate(20);

            builder.HasData(details);
        }

        private static void SeedShippers(EntityTypeBuilder<Shipper> builder)
        {
        }

        private static void SeedCustomres(EntityTypeBuilder<Customer> builder)
        {
            var custid = SeedDataConstants.Customers.SeedID;
            var cutomers = new Faker<Customer>()
                .RuleFor(o => o.Id, x => custid++)
                .RuleFor(o => o.Name, x => x.Person.FirstName)
                .RuleFor(o => o.Email, x => x.Person.Email)
                .RuleFor(o => o.PhoneNumber, x => x.Person.Phone)
                .RuleFor(o => o.LastName, x => x.Person.LastName)
                .RuleFor(o => o.Address, x => x.Person.Address.Street)
                .RuleFor(o => o.City, x => x.Person.Address.City)
                .RuleFor(o => o.ZipCode, x => x.Person.Address.ZipCode)
                .Generate(SeedDataConstants.Customers.Count);

            builder.HasData(cutomers);
        }

        #region seed orders

        private static int id = SeedDataConstants.OrderDetail.SeedID;
        private static List<OrderDetail> GenOrderDetails(int count, int orderId)
        {
            var minProductId = SeedDataConstants.Product.SeedID;
            var maxProductId = SeedDataConstants.Product.Count;

            var details = new Faker<OrderDetail>()
                .RuleFor(o => o.Id, x => id++)
                .RuleFor(o => o.Qty, x => x.Random.Int(1, 5))
                .RuleFor(o => o.OrderID, orderId)
                .RuleFor(o => o.ProductID, x => x.Random.Int(minProductId, maxProductId))
                .Generate(count);

            return details;
        }

        private static void SeedOrders(EntityTypeBuilder<Order> builder)
        {
            var orderId = SeedDataConstants.Order.SeedOrderId;
            var minCustId = SeedDataConstants.Customers.SeedID;
            var maxCustId = SeedDataConstants.Customers.Count;
            var rnd = new Random();
            var orders = new Faker<Order>()
                .RuleFor(o => o.Id, x => orderId++)
                .RuleFor(o => o.Created, GenRandomDate)
                .RuleFor(o => o.CustomerID, x => x.Random.Int(minCustId, maxCustId))
                .RuleFor(o => o.OrderDetails, GenOrderDetails(rnd.Next(1, 3), orderId - 1))
                .Generate(SeedDataConstants.Order.Count);

            builder.HasData(orders);
        }

        #endregion

        #region Seed Stocks

        private static void SeedStocks(EntityTypeBuilder<Stock> builder)
        {
            var countProducts = SeedDataConstants.Product.Count;
            var fakeStockIdCounter = SeedDataConstants.Stock.SeedID;

            #region LINQ altenative

            var stocks = Enumerable.Range(1, countProducts)
                .Select(x => new List<Stock>
                {
                    new Stock()
                    {
                        Id = fakeStockIdCounter++, Created = DateTime.Now, Description = "S", Qty = 100, ProductId = x
                    },
                    new Stock()
                    {
                        Id = fakeStockIdCounter++, Created = DateTime.Now, Description = "M", Qty = 100, ProductId = x
                    },
                    new Stock()
                    {
                        Id = fakeStockIdCounter++, Created = DateTime.Now, Description = "L", Qty = 100, ProductId = x
                    },
                    new Stock()
                    {
                        Id = fakeStockIdCounter++, Created = DateTime.Now, Description = "XL", Qty = 100, ProductId = x
                    },
                })
                .Aggregate((x, y) => x.Concat(y).ToList())
                .ToList();

            #endregion

            var fakeStocks = new List<Stock>();
            for (int i = 1; i <= countProducts; i++)
            {
                fakeStocks.AddRange(new List<Stock>()
                {
                    new Stock()
                    {
                        Id = fakeStockIdCounter++, Created = DateTime.Now, Description = "S", Qty = 100, ProductId = i
                    },
                    new Stock()
                    {
                        Id = fakeStockIdCounter++, Created = DateTime.Now, Description = "M", Qty = 100, ProductId = i
                    },
                    new Stock()
                    {
                        Id = fakeStockIdCounter++, Created = DateTime.Now, Description = "L", Qty = 100, ProductId = i
                    },
                    new Stock()
                    {
                        Id = fakeStockIdCounter++, Created = DateTime.Now, Description = "XL", Qty = 100, ProductId = i
                    },
                });
            }

            builder.HasData(fakeStocks);
        }

        #endregion

        #region Seed Products

        private static void SeedProducts(EntityTypeBuilder<Product> entityTypeBuilder)
        {
            if (_fakeProducts != null && _fakeProducts.Count > 0)
                entityTypeBuilder.HasData(_fakeProducts);
        }

        #endregion

        #region Utils

        private static DateTime GenRandomDate(Faker faker)
            => faker.Date.Between(new DateTime(2022, 1, 1), new DateTime(2023, 2, 7));

        private static decimal GetRoundPrice(Faker value) =>
            decimal.Round(value.Random.Decimal(10.00m, 100.00m), 2, MidpointRounding.AwayFromZero);

        #endregion
    }
}