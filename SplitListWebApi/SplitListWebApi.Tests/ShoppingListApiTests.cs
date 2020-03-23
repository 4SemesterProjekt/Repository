using System.Linq;
using ApiFormat;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SplitListWebApi.Models;
using SplitListWebApi.Repository;

namespace SplitListWebApi.Tests
{
    [TestFixture]
    public class ShoppingListApiTests
    {
        private DbContextOptions<SplitListContext> options;
        private SqliteConnection connection;

        [SetUp]
        public void Setup()
        {
           connection = new SqliteConnection("DataSource=:memory:");
           connection.Open();

            options = new DbContextOptionsBuilder<SplitListContext>()
                .UseSqlite(connection)
                .Options;
        }

        [TearDown]
        public void TearDown()
        {
            connection.Close();
        }

        [Test]
        public void AddShoppingList()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new SplitListContext(options))
            {
                context.Groups.Add(new Group()
                {
                    GroupID = 1,
                    Name = "Group1",
                    OwnerID = 1,
                    Pantries = null,
                    ShoppingLists = null,
                    UserGroups = null
                });
                context.SaveChanges();
            }

            ShoppingListFormat list = new ShoppingListFormat()
            {
                shoppingListID = 1,
                shoppingListName = "ShoppingList1",
                shoppingListGroupID = 1,
                shoppingListGroupName = "Group1"
            };
            using (var context = new SplitListContext(options))
            {
                ShoppingListRepository repo = new ShoppingListRepository(context);
                repo.AddShoppingList(list);
                Assert.AreEqual(1, context.ShoppingLists.Count());
                Assert.AreEqual("ShoppingList1", context.ShoppingLists.First().Name);

            }
        }

        [Test]
        public void DeleteShoppingList()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new SplitListContext(options))
            {
                context.Groups.Add(new Group()
                {
                    GroupID = 1,
                    Name = "Group1",
                    OwnerID = 1,
                    Pantries = null,
                    ShoppingLists = null,
                    UserGroups = null
                });
            }
            ShoppingListFormat list = new ShoppingListFormat()
            {
                shoppingListID = 2,
                shoppingListName = "ShoppingList1",
                shoppingListGroupID = 1,
                shoppingListGroupName = "Group1"
            };
            using (var context = new SplitListContext(options))
            {
                ShoppingListRepository repo = new ShoppingListRepository(context);
                repo.AddShoppingList(list);
                Assert.AreEqual(1, context.ShoppingLists.Count());
                repo.DeleteShoppingList(list);
                Assert.AreEqual(0, context.ShoppingLists.Count());
            }
        }

        [TestCase(2)]
        public void GetAllShoppingLists(int amount)
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                Assert.AreEqual(0, context.ShoppingLists.Count());

                context.Groups.Add(new Group()
                {
                    GroupID = 1,
                    Name = "Group1",
                    OwnerID = 1,
                    Pantries = null,
                    ShoppingLists = null,
                    UserGroups = null
                });
                context.SaveChanges();
            }

            using (var context = new SplitListContext(options))
            {
                ShoppingListRepository repo = new ShoppingListRepository(context);
                for (var i = 0; i < amount; ++i)
                {
                    repo.AddShoppingList(new ShoppingListFormat()
                    {
                        shoppingListID = i+1,
                        shoppingListName = $"ShoppingList{i+1}",
                        shoppingListGroupID = 1,
                        shoppingListGroupName = "Group1"
                    });
                }
                

                Assert.AreEqual(amount, context.ShoppingLists.Count());
                Assert.AreEqual(amount, repo.GetShoppingLists().Result.Count);

            }


        }
            


        [Test]
        public void GetShoppingListsFromGroupID() { }

        [Test]
        public void GetShoppingListByID() { }

        [Test]
        public void LoadToModelIdentical() { }

        [Test]
        public void UpdateShoppingList() { }

    }
}