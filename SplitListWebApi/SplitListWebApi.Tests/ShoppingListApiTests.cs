using System.Collections.Generic;
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
        public void DeleteShoppingList()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new SplitListContext(options))
            {

                Group jørgensGruppe = new Group()
                {
                    GroupID = 1,
                    Name = "JordkimsGruppe",
                    OwnerID = 1,
                    Pantries = null,
                    ShoppingLists = null,
                    UserGroups = null
                };
                context.Groups.Add(jørgensGruppe);

                ShoppingListDTO list = new ShoppingListDTO()
                {
                    shoppingListGroupID = jørgensGruppe.GroupID,
                    shoppingListGroupName = "JordkimsGruppe",
                    shoppingListName = "JørgensListe",
                    shoppingListID = 1
                };

                context.SaveChanges();
                
                ShoppingListRepository repo = new ShoppingListRepository(context);
                repo.LoadToModel(list);

                Assert.AreEqual(1, context.ShoppingLists.Count());

                repo.DeleteShoppingList(list);

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
                    repo.LoadToModel(new ShoppingListDTO()
                    {
                        shoppingListName = $"ShoppingList #{i}",
                        shoppingListGroupID = 1,
                        shoppingListGroupName = "Group1",
                    });
                }


                Assert.AreEqual(amount, context.ShoppingLists.Count());
                Assert.AreEqual(amount, repo.GetShoppingLists().Count);

            }
        }



        [Test]
        public void GetShoppingListsFromGroupID() { }

        [Test]
        public void GetShoppingListByID() 
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new SplitListContext(options))
            {

                Group jørgensGruppe = new Group()
                {
                    GroupID = 1,
                    Name = "JordkimsGruppe",
                    OwnerID = 1,
                    Pantries = null,
                    ShoppingLists = null,
                    UserGroups = null
                };
                context.Groups.Add(jørgensGruppe);

                ShoppingListDTO list = new ShoppingListDTO()
                {
                    shoppingListGroupID = jørgensGruppe.GroupID,
                    shoppingListGroupName = "JordkimsGruppe",
                    shoppingListName = "JørgensListe",
                    shoppingListID = 1,
                    Items = new List<ItemDTO> {
                        new ItemDTO()
                        {
                            Name = "Banan",
                            Type = "Fruit",
                            Amount = 2,
                            ItemID = 1
                        }
                    }
                };

                context.SaveChanges();

                ShoppingListRepository repo = new ShoppingListRepository(context);
                repo.LoadToModel(list);

                ShoppingListDTO dtoList = repo.GetShoppingListByID(1);
                Assert.AreEqual(dtoList.shoppingListName, list.shoppingListName);
                Assert.AreEqual(dtoList.shoppingListGroupName, list.shoppingListGroupName);
                Assert.AreEqual(dtoList.shoppingListGroupID, list.shoppingListGroupID);
                Assert.AreEqual(dtoList.shoppingListID, list.shoppingListID);

                Assert.AreEqual(dtoList.Items.Count, list.Items.Count);

                for (var i = 0; i < dtoList.Items.Count; ++i)
                {
                    Assert.AreEqual(dtoList.Items[i].Name, list.Items[i].Name);
                    Assert.AreEqual(dtoList.Items[i].Amount, list.Items[i].Amount);
                    Assert.AreEqual(dtoList.Items[i].Type, list.Items[i].Type);
                    Assert.AreEqual(dtoList.Items[i].ItemID, list.Items[i].ItemID);
                }
            }
        }

        [Test]
        public void LoadToModelAddsToDB()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();

                Group jørgensGruppe = new Group()
                {
                    GroupID = 1,
                    Name = "JordkimsGruppe",
                    OwnerID = 1,
                    Pantries = null,
                    ShoppingLists = null,
                    UserGroups = null
                };
            
                context.Groups.Add(jørgensGruppe);
                context.SaveChanges(); 

                ShoppingListDTO formatList = new ShoppingListDTO()
                {
                     shoppingListGroupID = jørgensGruppe.GroupID,
                     shoppingListGroupName = "JordkimsGruppe",
                     shoppingListName = "JørgensListe"
                };


                ShoppingListRepository repo = new ShoppingListRepository(context);
                ShoppingList modelList = repo.LoadToModel(formatList);
                ShoppingList actualList = context.ShoppingLists.FirstOrDefault();
                Assert.AreEqual(modelList.Name, actualList.Name);
                Assert.AreEqual(modelList.GroupID, actualList.GroupID);
                Assert.AreEqual(modelList.ShoppingListID, actualList.ShoppingListID);
            }
        }

        [Test]
        public void LoadToModelFindsListFromDB()
        {

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
                    repo.UpdateShoppingList(new ShoppingListDTO()
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

            ShoppingListDTO list = new ShoppingListDTO()
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

                repo.UpdateShoppingList(new ShoppingListDTO()
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