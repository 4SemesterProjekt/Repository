using System;
using System.Collections.Generic;
using System.Linq;
using ApiFormat;
using ApiFormat.Group;
using ApiFormat.Item;
using ApiFormat.User;
using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SplitListWebApi.Areas.AutoMapper;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Implementation;
using SplitListWebApi.Services;
using SplitListWebApi.Services.Interfaces;
using SplitListWebApi.Utilities;

namespace SplitListWebApi.Tests
{
    [TestFixture]
    public class ItemAPITests
    {
        private DbContextOptions<SplitListContext> options;
        private SqliteConnection connection;
        private IMapper mapper;

        [SetUp]
        public void Setup()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            options = new DbContextOptionsBuilder<SplitListContext>()
                .UseSqlite(connection)
                .Options;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GroupProfile>();
                cfg.AddProfile<PantryProfile>();
                cfg.AddProfile<UserProfile>();
                cfg.AddProfile<ShoppingListProfile>();
                cfg.AddProfile<ItemProfile>();
            });
            mapper = config.CreateMapper();
        }

        [TearDown]
        public void TearDown()
        {
            connection.Close();
        }

        [Test]
        public void CreateItemCreatesItemInDb()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                IService<ItemDTO, ItemModel> service = new ItemService(context, mapper);

                ItemDTO item = new ItemDTO()
                {
                    Name = "Banana",
                    Type = "Fruit"
                };

                ItemDTO itemFromDb = service.Create(item);

                Assert.AreEqual(item.Name, itemFromDb.Name);
                Assert.AreEqual(item.Type, itemFromDb.Type);
                Assert.AreNotEqual(item.ModelId, itemFromDb.ModelId);
            }
        }

        [Test]
        public void DeleteItemDeletesItemFromDb()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                IService<ItemDTO, ItemModel> service = new ItemService(context, mapper);

                List<ItemDTO> items = new List<ItemDTO>()
                {
                    new ItemDTO
                    {
                        Name = "Banana",
                        Type = "Fruit"
                    },
                    new ItemDTO
                    {
                        Name = "Apple",
                        Type = "Fruit"
                    },
                    new ItemDTO
                    {
                        Name = "Coconut",
                        Type = "Fruit"
                    }
                };

                List<ItemDTO> itemsFromDb = new List<ItemDTO>();

                foreach (ItemDTO item in items)
                {
                    itemsFromDb.Add(service.Create(item));
                }

                Assert.AreEqual(3, context.Items.Count());

                foreach (ItemDTO item in itemsFromDb)
                {
                    service.Delete(item);
                }

                Assert.AreEqual(0, context.Items.Count());
            }
        }
    }
}