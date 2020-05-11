using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiFormat;
using ApiFormat.Group;
using ApiFormat.Item;
using ApiFormat.Pantry;
using ApiFormat.ShoppingList;
using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SplitListWebApi.Areas.AutoMapper;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Implementation;
using SplitListWebApi.Services;
using SplitListWebApi.Services.Interfaces;
using SplitListWebApi.Utilities;
using ArgumentException = System.ArgumentException;

namespace SplitListWebApi.Tests
{
    [TestFixture]
    public class PantryApiTests
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

            var config = new MapperConfiguration(cfg => {
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
        public void GetFromDatabaseGetsSL()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                GroupService groupService = new GroupService(context, mapper);
                PantryService pantryService = new PantryService(context, mapper);

                GroupDTO groupDto = new GroupDTO()
                {
                    Name = "Group1",
                    OwnerID = "1"
                };

                groupDto = groupService.Create(groupDto);

                PantryDTO pantryDTO = new PantryDTO()
                {
                    Name = "PantryTest",
                    Group = groupDto
                };

                var pantryFromDb = pantryService.Create(pantryDTO);

                Assert.AreEqual(1, context.Pantries.Count());
                Assert.AreEqual(pantryDTO.Name, pantryFromDb.Name);
                Assert.AreEqual(pantryFromDb.GroupID, groupDto.ModelId);
            }
        }

        [Test]
        public void PantryWithItemsCreatesPantryItemsItems()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                GroupService groupService = new GroupService(context, mapper);
                PantryService pantryService = new PantryService(context, mapper);

                GroupDTO groupDto = new GroupDTO()
                {
                    Name = "Group1",
                    OwnerID = "1"
                };

                groupDto = groupService.Create(groupDto);

                List<ItemDTO> items = new List<ItemDTO>()
                {
                    new ItemDTO()
                    {
                        Name = "Banana",
                        Type = "Fruit",
                        Amount = 5
                    },
                    new ItemDTO()
                    {
                        Name = "Apple",
                        Type = "Fruit",
                        Amount = 2
                    }
                };

                PantryDTO pantryDTO = new PantryDTO()
                {
                    Name = "PantryTest",
                    Group = groupDto,
                    Items = items
                };

                var pantryFromDb = pantryService.Create(pantryDTO);

                Assert.AreEqual(2, context.PantryItems.Count());
                Assert.AreEqual(pantryDTO.Items.Count, pantryFromDb.Items.Count);
                Assert.AreEqual(2, context.Items.Count());
            }
        }

        [Test]
        public void UpdatePantryUpdatesItems()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                GroupService groupService = new GroupService(context, mapper);
                PantryService pantryService = new PantryService(context, mapper);

                GroupDTO groupDto = new GroupDTO()
                {
                    Name = "Group1",
                    OwnerID = "1"
                };

                groupDto = groupService.Create(groupDto);

                List<ItemDTO> items = new List<ItemDTO>()
                {
                    new ItemDTO()
                    {
                        Name = "Banana",
                        Type = "Fruit",
                        Amount = 5
                    },
                    new ItemDTO()
                    {
                        Name = "Apple",
                        Type = "Fruit",
                        Amount = 2
                    }
                };

                PantryDTO pantryDTO = new PantryDTO()
                {
                    Name = "PantryTest",
                    Group = groupDto,
                    Items = items
                };

                var pantryFromDb = pantryService.Create(pantryDTO);

                ItemDTO extraItem = new ItemDTO()
                {
                    Name = "Pear",
                    Type = "Fruit",
                    Amount = 10
                };

                pantryFromDb.Items.Add(extraItem);

                pantryFromDb = pantryService.Update(pantryFromDb);

                Assert.AreEqual(3, context.PantryItems.Count());
                Assert.AreEqual(3, pantryFromDb.Items.Count);
                Assert.AreEqual(3, context.Items.Count());
            }
        }

        [Test]
        public void UpdatePantryUpdatesItemProperties()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                GroupService groupService = new GroupService(context, mapper);
                PantryService pantryService = new PantryService(context, mapper);

                GroupDTO groupDto = new GroupDTO()
                {
                    Name = "Group1",
                    OwnerID = "1"
                };

                groupDto = groupService.Create(groupDto);

                List<ItemDTO> items = new List<ItemDTO>()
                {
                    new ItemDTO()
                    {
                        Name = "Banana",
                        Type = "Fruit",
                        Amount = 5
                    },
                    new ItemDTO()
                    {
                        Name = "Apple",
                        Type = "Fruit",
                        Amount = 2
                    }
                };

                PantryDTO pantryDTO = new PantryDTO()
                {
                    Name = "PantryTest",
                    Group = groupDto,
                    Items = items
                };

                var pantryFromDb = pantryService.Create(pantryDTO);

                ItemDTO cashew = new ItemDTO
                {
                    Name = "Cashew",
                    Type = "Nut",
                    Amount = 67
                };

                pantryFromDb.Items[0] = cashew;
                pantryFromDb= pantryService.Update(pantryFromDb);
                ItemDTO cashewFromDb = pantryFromDb.Items[0];

                Assert.AreEqual(2, context.PantryItems.Count());
                Assert.AreEqual(2, pantryFromDb.Items.Count);
                Assert.AreEqual(3, context.Items.Count());
                Assert.AreEqual(cashew.Name, cashewFromDb.Name);
                Assert.AreEqual(cashew.Amount, cashewFromDb.Amount);
                Assert.AreEqual(cashew.Type, cashewFromDb.Type);
            }
        }

        [Test]
        public void UpdatePantryUpdatesPorperties()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                GroupService groupService = new GroupService(context, mapper);
                PantryService pantryService = new PantryService(context, mapper);

                GroupDTO groupDto = new GroupDTO()
                {
                    Name = "Group1",
                    OwnerID = "1"
                };

                groupDto = groupService.Create(groupDto);

                PantryDTO pantryDTO = new PantryDTO()
                {
                    Name = "PantryTest",
                    Group = groupDto
                };

                PantryDTO pantryFromDb = pantryService.Create(pantryDTO);

                pantryFromDb.Name = "ListTest";
                pantryFromDb= pantryService.Update(pantryFromDb);

                Assert.AreNotEqual(context.Pantries.FirstOrDefault().Name, pantryDTO.Name);
                Assert.AreEqual(context.Pantries.FirstOrDefault().Name, pantryFromDb.Name);
            }
        }

        [Test]
        public void DeleteItemInPantryRemovesItem()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                GroupService groupService = new GroupService(context, mapper);
                PantryService pantryService = new PantryService(context, mapper);

                GroupDTO groupDto = new GroupDTO()
                {
                    Name = "Group1",
                    OwnerID = "1"
                };

                groupDto = groupService.Create(groupDto);

                List<ItemDTO> items = new List<ItemDTO>()
                {
                    new ItemDTO()
                    {
                        Name = "Banana",
                        Type = "Fruit",
                        Amount = 5
                    },
                    new ItemDTO()
                    {
                        Name = "Apple",
                        Type = "Fruit",
                        Amount = 2
                    }
                };

                PantryDTO pantryDTO = new PantryDTO()
                {
                    Name = "PantryTest",
                    Group = groupDto,
                    Items = items
                };

                var pantryFromDb = pantryService.Create(pantryDTO);

                pantryFromDb.Items.RemoveAt(0);
                pantryFromDb = pantryService.Update(pantryFromDb);
                ItemDTO newItem0 = pantryFromDb.Items[0];

                Assert.AreEqual(1, context.PantryItems.Count());
                Assert.AreEqual(1, pantryFromDb.Items.Count);
                Assert.AreEqual(2, context.Items.Count());
                Assert.AreEqual(newItem0.Name, "Apple");
            }
        }

        [Test]
        public void AddExistingItemToPantryUsesItemInDb()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                GroupService groupService = new GroupService(context, mapper);
                PantryService pantryService = new PantryService(context, mapper);
                ItemService itemService = new ItemService(context, mapper);

                GroupDTO groupDto = new GroupDTO()
                {
                    Name = "Group1",
                    OwnerID = "1"
                };

                groupDto = groupService.Create(groupDto);

                ItemDTO appleItem = itemService.Create(new ItemDTO()
                {
                    Name = "Apple",
                    Amount = 2,
                    Type = "Fruit"
                });


                List<ItemDTO> items = new List<ItemDTO>()
                {
                    new ItemDTO()
                    {
                        Name = "Banana",
                        Type = "Fruit",
                        Amount = 5
                    }
                };

                PantryDTO pantryDTO = new PantryDTO()
                {
                    Name = "PantryTest",
                    Group = groupDto,
                    Items = items
                };

                var pantryFromDb = pantryService.Create(pantryDTO);

                pantryFromDb.Items.Add(appleItem);
                pantryFromDb = pantryService.Update(pantryFromDb);
                ItemDTO appleItemFromDb = pantryFromDb.Items[1];

                Assert.AreEqual(2, context.PantryItems.Count());
                Assert.AreEqual(2, pantryFromDb.Items.Count);
                Assert.AreEqual(2, context.Items.Count());
                Assert.AreEqual(appleItemFromDb.ModelId, appleItem.ModelId);
            }
        }

        [Test]
        public void UpdateOnNonExistingPantryThrowsException()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                GroupService groupService = new GroupService(context, mapper);
                PantryService pantryService = new PantryService(context, mapper);

                GroupDTO groupDto = new GroupDTO()
                {
                    Name = "Group1",
                    OwnerID = "1"
                };

                groupDto = groupService.Create(groupDto);

                PantryDTO pantryDTO = new PantryDTO()
                {
                    Name = "PantryTest",
                    Group = groupDto
                };

                Assert.Throws(typeof(ArgumentException), () => pantryService.Update(pantryDTO));
            }
        }
    }
}

