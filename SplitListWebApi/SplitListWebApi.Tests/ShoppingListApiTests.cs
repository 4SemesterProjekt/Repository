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

        [SetUp]
        public void Setup()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            options = new DbContextOptionsBuilder<SplitListContext>()
                .UseSqlite(connection)
                .Options;
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
    }
}