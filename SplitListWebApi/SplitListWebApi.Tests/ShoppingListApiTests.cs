using System.Linq;
using System.Threading.Tasks;
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
        public async Task DeleteShoppingList()
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
                await repo.DeleteShoppingList(list);
                Assert.AreEqual(0, context.ShoppingLists.Count());
            }
        }
        
        
        
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(7)]
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
        public async Task LoadToModelIdentical()
        {
            ShoppingListFormat formatList = new ShoppingListFormat()
            {
                Items = null,
                shoppingListGroupID = 4,
                shoppingListGroupName = "JordkimsGruppe",
                shoppingListID = 1,
                shoppingListName = "JørgensListe"
            };
            
            using (var context = new SplitListContext(options))
            {
                ShoppingListRepository repo = new ShoppingListRepository(context);
                ShoppingList modelList = await repo.LoadToModel(formatList);
                Assert.AreEqual(modelList.Name, formatList.shoppingListName);
                Assert.AreEqual(modelList.GroupID, formatList.shoppingListGroupID);
                Assert.AreEqual(modelList.ShoppingListID, formatList.shoppingListID);
            }
        }

        [TestCase(2)]
        [TestCase(7)]
        [TestCase(3)]
        public void UpdateShoppingListAddsWhenNoListFoundToUpdate(int amount) 
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new SplitListContext(options))
            {
                context.Groups.Add(new Group()
                {
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
                    repo.UpdateShoppingList(new ShoppingListFormat()
                    {
                        shoppingListID = i + 1,
                        shoppingListName = $"ShoppingList{i + 1}",
                        shoppingListGroupID = 1,
                        shoppingListGroupName = "Group1"
                    });
                }
            }
        }

        [Test]
        public void UpdateShoppingListUpdatesShoppingListWhenFound()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new SplitListContext(options))
            {
                context.Groups.Add(new Group()
                {
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
                shoppingListName = "ShoppingList1",
                shoppingListGroupID = 1,
                shoppingListGroupName = "Group1"
            };

            using (var context = new SplitListContext(options))
            {
                ShoppingListRepository repo = new ShoppingListRepository(context);
                repo.UpdateShoppingList(list);
                Assert.AreEqual("ShoppingList1", context.ShoppingLists.FirstOrDefault().Name);
                Assert.AreEqual(1, context.ShoppingLists.Count());

                repo.UpdateShoppingList(new ShoppingListFormat()
                {
                    shoppingListGroupID = 1,
                    shoppingListName = "ShoppingList7",
                    shoppingListGroupName = "Group1",
                });

                Assert.AreEqual(2, context.ShoppingLists.Count());
            }



        }

        [Test]
        public void UpdateShoppinglistItemsWhenFound()
        {

        }

    }
}