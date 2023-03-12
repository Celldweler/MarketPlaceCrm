using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using MarketPlaceCrm.Data.Context;
using MarketPlaceCrm.Data.Entities;
using MarketPlaceCrm.Data.Entities.Moderation;
using MarketPlaceCrm.Data.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlaceCrm.Data.SeedData
{
    public static class MemoryFakeSeeder
    {
        #region Fields

        public static List<Product> _fakeProducts { get; set; } = new List<Product>();
        public static List<Stock> _fakeStocks { get; set; } = new List<Stock>();
        public static List<Order> _fakeOrders { get; set; } = new List<Order>();
        public static List<Shipper> _fakeShippers { get; set; } = new List<Shipper>();
        public static List<Customer> _fakeCustomers { get; set; } = new List<Customer>();
        public static List<OrderDetail> _fakeOrderDetails { get; set; } = new List<OrderDetail>();

        private static int StockIdInc = 1;
        private static int OrderIdInc = SeedDataConstants.Order.SeedOrderId;

        #endregion

        public static void SeedFakeDataInMemory(this AppDbContext ctx)
        {
            try
            {
                // SeedProducts(ctx);
                // SeedCustomres(ctx);
                // SeedOrders(ctx);
                // SeedOrderDetails(ctx);
                // SeedStocks(ctx);
                // SeedShippers(ctx);
                initState(ctx);
                SeedCategories(ctx);
                SeedShippingMethods(ctx);
                
                ctx.AddRange(new List<OrderHistoryDetail>
                {
                    new OrderHistoryDetail
                    {
                        CustomerId = 1,
                        OrderId = 10248,
                        StatusUpdated = new DateTime(2023,2,16, 1,0,0),
                        Status = OrderStatuses.Pending
                    },
                    new OrderHistoryDetail
                    {
                        CustomerId = 1,
                        OrderId = 10248,
                        StatusUpdated = new DateTime(2023,2,16, 1,30,0),
                        Status = OrderStatuses.Processing
                    },
                    new OrderHistoryDetail
                    {
                        CustomerId = 1,
                        OrderId = 10248,
                        StatusUpdated = new DateTime(2023,2,16, 2,30,0),
                        Status = OrderStatuses.Paid
                    },
                    new OrderHistoryDetail
                    {
                        CustomerId = 1,
                        OrderId = 10248,
                        StatusUpdated = new DateTime(2023,2,17, 8,30,0),
                        Status = OrderStatuses.Shipped
                    },
                    new OrderHistoryDetail
                    {
                        CustomerId = 1,
                        OrderId = 10248,
                        StatusUpdated = new DateTime(2023,2,20, 11,30,0),
                        Status = OrderStatuses.Delivered
                    },
                    new OrderHistoryDetail
                    {
                        CustomerId = 1,
                        OrderId = 10248,
                        StatusUpdated = new DateTime(2023,2,21, 18,30,0),
                        Status = OrderStatuses.Completed
                    },
                });
                ctx.Add(new Notification
                {
                    Id = 1,
                    IsRead = false,
                    JsonData = "comment passed moderation",
                    ReceiverId = 1,
                    SenderId = 666,
                });
                ctx.Add(new ApplicationUser
                {
                    Id = 666,
                    UserName = "RaimeAdmin",
                    Email = "adminRaime@email.com",
                    Role = UserRoles.Moderator,
                });
                ctx.Add(new ApplicationUser
                {
                    Id = 777,
                    UserName = "Maveldous",
                    Email = "artem_admin@email.com",
                    Role = UserRoles.Moderator,
                });
                ctx.Add(new Product
                {
                    Id = 10000,
                    Name = "zip-hoodie",
                    Description = "test",
                    Cost = 100,
                    IsPublished = false,
                    Deleted = false,
                });
                ctx.Add(new ModerationItem
                {
                    Type = ModerationTypes.NewAddedProduct,
                    UserId = 666,
                    CurrentId = 10000,
                    TargetId = 10000,
                });
                
                //
                ctx.Add(new Comment
                {
                    Text = "text cooment",
                    Created = DateTime.Now,
                    PassedModeration = true,
                    Deleted = false,
                    FromId = 1,
                    ProductId = 1,
                });
                ctx.Add(new Comment
                {
                    Text = "text cooment",
                    Created = DateTime.Now,
                    PassedModeration = false,
                    Deleted = false,
                    FromId = 2,
                    ProductId = 2,
                });
                ctx.SaveChanges();
            }
            catch (Exception e)
            {
                // ctx.Database.RollbackTransaction();
                Console.WriteLine(e);
                throw;
            }
        }

        private static void SeedShippingMethods(AppDbContext ctx)
        {
            ctx.AddRange(new List<ShippingMethod>
            {
                new ShippingMethod() {Id = 1, Name = "Самовывоз из точки выдачи", Description = "Бесплатно"},
                new ShippingMethod() {Id = 2, Name = "Самовывоз из отделений почтовых операторов", Description = "63₴ - 120₴"},
                new ShippingMethod() {Id = 3, Name = "Доставка курьером", Description = "Бесплатно"},
            });
        }

        private static void initState(AppDbContext ctx)
        {
            if (_fakeProducts != null && _fakeProducts.Count == 0)
            {
                var fakeId = SeedDataConstants.Product.SeedID;
                var rnd = new Random();
                _fakeProducts = new Faker<Product>()
                    .RuleFor(o => o.Id, x => fakeId++)
                    .RuleFor(o => o.Name, x => x.Lorem.Sentence(5))
                    .RuleFor(o => o.Description, x => x.Lorem.Sentence(10, 15))
                    .RuleFor(o => o.Cost, GetRoundPrice)
                    .RuleFor(o => o.Created, GenRandomDate)
                    // .RuleFor(o => o.Stocks, x => GenTestsStocks(fakeId - 1))
                    .Generate(SeedDataConstants.Product.Count);

                var images = new[] { "puffer-jacket.jpeg", "tee-sort.jfif", "WOF-FULL-ZIP-HOODIE.jpeg", "XCROSS-T-WASHED-HOODIE-DARK.jfif" };
                for (int i = 0; i < SeedDataConstants.Product.Count; i++)
                {
                    if(i < images.Length)
                        _fakeProducts[i].Image = images[i];
                    _fakeStocks.AddRange(GenTestsStocks(SeedDataConstants.Product.SeedID + i));
                }

                ctx.Add(new Product
                {
                    Id = 100,
                    Name = "Asus N551JK-XO076H Laptop",
                    Description =
                        "Laptop Asus N551JK Intel Core i7-4710HQ 2.5 GHz, RAM 16GB, HDD 1TB, Video NVidia GTX 850M 4GB, BluRay, 15.6, Full HD, Win 8.1",
                    Cost = 1500.00m,
                    Reviews = new List<Comment>
                    {
                        new Comment
                        {
                            Text = "mac top",
                            PassedModeration = true,
                            Likes = 5,
                            Dislikes = 3,
                            FromId = 1,
                        },
                        new Comment
                        {
                            Text = "test test comment from customer 2",
                            PassedModeration = true,
                            Likes = 5,
                            Dislikes = 3,
                            FromId = 2,
                            Replies = new List<Comment>()
                            {
                                new Comment
                                {
                                    FromId = 1,
                                    Text = "test reply to comment leave customer 2",
                                    PassedModeration = true,
                                    Likes = 1,
                                    Dislikes = 0,
                                }
                            }
                        },
                    },
                    Categories = new List<ProductCategoryRelationship>()
                    {
                        new ProductCategoryRelationship()
                        {
                            CategoryId = 710
                        }
                    }
                });
                ctx.AddRange(_fakeProducts);
                ctx.AddRange(_fakeStocks);
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
                    var orders = GenOrdersForCustomerWithId(custid + i, rnd.Next(1, 5));
                    if (orders.Count > 0)
                        _fakeOrders.AddRange(orders);
                }

                ctx.Add(new Customer
                {
                    Id = 1111,
                    Email = "test@email.com",
                    PhoneNumber = "+380 96 12 333 22",
                    UserName = "test",
                    Name = "Artem",
                    LastName = "Naumov",
                    Address = "fake address",
                    City = "Odessa",
                    Country = "Ukraine",
                    ZipCode = "65000"
                });
                ctx.AddRange(cutomers);
                ctx.AddRange(_fakeOrders);
                ctx.AddRange(_fakeOrderDetails);
            }
        }

        private static void SeedCategories(AppDbContext ctx)
        {
            var categoriesNames = new[] { "HOODIES", "PANTS", "TEES", "JKTS", "HATS", "Computers" };
            ctx.AddRange(new List<Category>()
            {
                new Category { Id = 1, Name = categoriesNames[0] },
                new Category { Id = 2, Name = categoriesNames[1] },
                new Category { Id = 3, Name = categoriesNames[2] },
                new Category { Id = 4, Name = categoriesNames[3] },
                new Category { Id = 5, Name = categoriesNames[4] },
                new Category { Id = 6, Name = categoriesNames[5] },
                new Category { Id = 7, Name = "Computers >> Desktops", ParentID = 6 },
                new Category { Id = 8, Name = "Computers >> Notebooks", ParentID = 6 },
                new Category { Id = 9, Name = "Computers >> Software", ParentID = 6 },
                new Category { Id = 710, Name = "Electronics", },
            });
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

        private static void SeedOrderDetails(AppDbContext ctx)
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

            ctx.AddRange(details);
        }

        private static void SeedShippers(AppDbContext ctx)
        {
        }

        private static void SeedCustomres(AppDbContext ctx)
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

            ctx.AddRange(cutomers);
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