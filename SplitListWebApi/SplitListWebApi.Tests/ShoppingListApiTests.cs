using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiFormat;
using ApiFormat.Group;
using ApiFormat.Item;
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

namespace SplitListWebApi.Tests
{
    [TestFixture]
    public class ShoppingListApiTests
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
                ShoppingListService slService = new ShoppingListService(context, mapper);
                GroupService groupService = new GroupService(context, mapper);

                GroupDTO groupDto = new GroupDTO()
                {
                    Name = "Group1",
                    OwnerID = "1"
                };

                groupDto = groupService.Create(groupDto);

                ShoppingListDTO slDto = new ShoppingListDTO()
                {
                    Name = "ShoppingList1",
                    Group = groupDto
                };

                var slFromDb = slService.Create(slDto);

                Assert.AreEqual(1, context.ShoppingLists.Count());
                Assert.AreEqual(slFromDb.Name, slDto.Name);
                Assert.AreEqual(slFromDb.GroupID, groupDto.ModelId);
            }
        }

        [Test]
        public void SLWithItemsCreatesShoppingListItems()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                ShoppingListService slService = new ShoppingListService(context, mapper);
                GroupService groupService = new GroupService(context, mapper);

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

                ShoppingListDTO slDto = new ShoppingListDTO()
                {
                    Name = "ShoppingList1",
                    Group = groupDto,
                    Items = items
                };

                var slFromDb = slService.Create(slDto);

                Assert.AreEqual(2, context.ShoppingListItems.Count());
                Assert.AreEqual(slDto.Items.Count, slFromDb.Items.Count);
                Assert.AreEqual(2, context.Items.Count());
            }
        }

        [Test]
        public void UpdateSLUpdatesItems()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                ShoppingListService slService = new ShoppingListService(context, mapper);
                GroupService groupService = new GroupService(context, mapper);

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

                ShoppingListDTO slDto = new ShoppingListDTO()
                {
                    Name = "ShoppingList1",
                    Group = groupDto,
                    Items = items
                };

                var slFromDb = slService.Create(slDto);

                ItemDTO extraItem = new ItemDTO()
                {
                    Name = "Pear",
                    Type = "Fruit",
                    Amount = 10
                };

                slFromDb.Items.Add(extraItem);

                slFromDb = slService.Update(slFromDb);

                Assert.AreEqual(3, context.ShoppingListItems.Count());
                Assert.AreEqual(3, slFromDb.Items.Count);
                Assert.AreEqual(3, context.Items.Count());
            }
        }

        [Test]
        public void UpdateSLUpdatesItemProperties()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                IService<ShoppingListDTO, ShoppingListModel> slService = new ShoppingListService(context, mapper);
                IService<GroupDTO, GroupModel> groupService = new GroupService(context, mapper);
                IService<ItemDTO, ItemModel> itemService = new ItemService(context, mapper);

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

                ShoppingListDTO slDto = new ShoppingListDTO()
                {
                    Name = "ShoppingList1",
                    Group = groupDto,
                    Items = items
                };

                var slFromDb = slService.Create(slDto);

                ItemDTO cashew = new ItemDTO
                {
                    Name = "Cashew",
                    Type = "Nut",
                    Amount = 67
                };
                slFromDb.Items[0] = cashew;
                slFromDb = slService.Update(slFromDb);
                ItemDTO cashewFromDb = slFromDb.Items[0];
                
                Assert.AreEqual(2, context.ShoppingListItems.Count());
                Assert.AreEqual(2, slFromDb.Items.Count);
                Assert.AreEqual(3, context.Items.Count());
                Assert.AreEqual(cashew.Name, cashewFromDb.Name);
                Assert.AreEqual(cashew.Amount, cashewFromDb.Amount);
                Assert.AreEqual(cashew.Type, cashewFromDb.Type);
            }
        }

        [Test]
        public void UpdateSLUpdatesPorperties()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                ShoppingListService slService = new ShoppingListService(context, mapper);
                GroupService groupService = new GroupService(context, mapper);

                GroupDTO groupDto = new GroupDTO()
                {
                    Name = "Group1",
                    OwnerID = "1"
                };

                groupDto = groupService.Create(groupDto);

                ShoppingListDTO dto = new ShoppingListDTO()
                {
                    Name = "TestList",
                    Group = groupDto
                };

                ShoppingListDTO dbList = slService.Create(dto);

                dbList.Name = "ListTest";
                dbList = slService.Update(dbList);

                Assert.AreNotEqual(context.ShoppingLists.FirstOrDefault().Name, dto.Name);
                Assert.AreEqual(context.ShoppingLists.FirstOrDefault().Name, dbList.Name);
            }
        }

        [Test]
        public void DeleteItemInSLRemovesItem()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                ShoppingListService slService = new ShoppingListService(context, mapper);
                GroupService groupService = new GroupService(context, mapper);

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

                ShoppingListDTO slDTO = new ShoppingListDTO()
                {
                    Name = "ShoppingListTest",
                    Group = groupDto,
                    Items = items
                };

                var slFromDb = slService.Create(slDTO);

                slFromDb.Items.RemoveAt(0);
                slFromDb = slService.Update(slFromDb);
                ItemDTO newItem0 = slFromDb.Items[0];

                Assert.AreEqual(1, context.ShoppingListItems.Count());
                Assert.AreEqual(1, slFromDb.Items.Count);
                Assert.AreEqual(2, context.Items.Count());
                Assert.AreEqual(newItem0.Name, "Apple");
            }
        }

        [Test]
        public void AddExistingItemToSLUsesItemInDb()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                GroupService groupService = new GroupService(context, mapper);
                ShoppingListService slService= new ShoppingListService(context, mapper);
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

                ShoppingListDTO slDTO = new ShoppingListDTO()
                {
                    Name = "slTest",
                    Group = groupDto,
                    Items = items
                };

                var slFromDb = slService.Create(slDTO);

                slFromDb.Items.Add(appleItem);
                slFromDb = slService.Update(slFromDb);
                ItemDTO appleItemFromDb = slFromDb.Items[1];

                Assert.AreEqual(2, context.ShoppingListItems.Count());
                Assert.AreEqual(2, slFromDb.Items.Count);
                Assert.AreEqual(2, context.Items.Count());
                Assert.AreEqual(appleItemFromDb.ModelId, appleItem.ModelId);
            }
        }
    }
}

