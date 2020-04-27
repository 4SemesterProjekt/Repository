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

                context.Items.AddRange(mapper.Map<List<ItemModel>>(items));
                context.SaveChanges();

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

                context.Items.AddRange(mapper.Map<List<ItemModel>>(items));
                context.SaveChanges();

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

                context.Items.Add(mapper.Map<ItemModel>(extraItem));
                context.SaveChanges();

                slFromDb.Items.Add(extraItem);

                slFromDb = slService.Update(slFromDb);

                Assert.AreEqual(3, context.ShoppingListItems.Count());
                Assert.AreEqual(3, slFromDb.Items.Count);
                Assert.AreEqual(3, context.Items.Count());
            }
        }
    }
}

