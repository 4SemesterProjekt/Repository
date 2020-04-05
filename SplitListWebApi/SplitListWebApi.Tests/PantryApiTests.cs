using System.Collections.Generic;
using System.Linq;
using ApiFormat;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Areas.Identity.Data.Models;
using SplitListWebApi.Models;
using SplitListWebApi.Repository;

namespace SplitListWebApi.Tests
{
    [TestFixture]
    public class PantryApiTests
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
        public void UdatePantryAddsNewPantry()
        {
            using (var context = new SplitListContext(options))
            {
                PantryRepository repo = new PantryRepository(context);
                context.Database.EnsureCreated();

                Group group = new Group()
                {
                    GroupID = 1,
                    Name = "Group1",
                    OwnerID = "1",
                };
                context.Groups.Add(group);
                context.SaveChanges();

                repo.UpdatePantry(new PantryDTO()
                {
                    GroupID = 1,
                    GroupName = "Group1",
                    Name = "Pantry1"
                });

                Assert.AreEqual(1, context.Pantries.Count());
                Assert.AreEqual(1, context.Pantries.FirstOrDefault().PantryID);
                Assert.AreEqual("Group1", context.Pantries.FirstOrDefault().Group.Name);
            }
        }

        [Test]
        public void UpdatePantryAddsItems()
        {
            using (var context = new SplitListContext(options))
            {
                PantryRepository repo = new PantryRepository(context);
                context.Database.EnsureCreated();

                Group group = new Group()
                {
                    GroupID = 1,
                    Name = "Group1",
                    OwnerID = "1",
                };
                context.Groups.Add(group);
                context.SaveChanges();

                PantryDTO pantry = new PantryDTO()
                {
                    GroupID = 1,
                    GroupName = "Group1",
                    Name = "Pantry1",
                    Items = new List<ItemDTO>()
                    {
                        new ItemDTO()
                        {
                            Name = "Banana",
                            Amount = 5,
                            Type = "Fruit"
                        },
                        new ItemDTO()
                        {
                            Name = "Apple",
                            Amount = 2,
                            Type = "Fruit"
                        }
                    }
                };

                repo.UpdatePantry(pantry);
                
                Assert.AreEqual(2, context.Pantries.FirstOrDefault().PantryItems.Count);
            }
        }

        [Test]
        public void UpdatePantryRemovesItems()
        {
            using (var context = new SplitListContext(options))
            {
                PantryRepository repo = new PantryRepository(context);
                context.Database.EnsureCreated();

                Group group = new Group()
                {
                    GroupID = 1,
                    Name = "Group1",
                    OwnerID = "1",
                };
                context.Groups.Add(group);
                context.SaveChanges();

                PantryDTO pantry = new PantryDTO()
                {
                    GroupID = 1,
                    GroupName = "Group1",
                    Name = "Pantry1",
                    Items = new List<ItemDTO>()
                    {
                        new ItemDTO()
                        {
                            Name = "Banana",
                            Amount = 5,
                            Type = "Fruit"
                        },
                        new ItemDTO()
                        {
                            Name = "Apple",
                            Amount = 2,
                            Type = "Fruit"
                        }
                    }
                };

                repo.UpdatePantry(pantry);

                Assert.AreEqual(2, context.Pantries.FirstOrDefault().PantryItems.Count);

                pantry.Items.RemoveAt(1);

                repo.UpdatePantry(pantry);

                Assert.AreEqual(1, context.Pantries.FirstOrDefault().PantryItems.Count);
                Assert.AreEqual("Banana", context.Pantries.FirstOrDefault().PantryItems.First().Item.Name);
            }
        }

        [Test]
        public void DeletePantryRemovesPantry()
        {
            using (var context = new SplitListContext(options))
            {
                PantryRepository repo = new PantryRepository(context);
                context.Database.EnsureCreated();

                Group group = new Group()
                {
                    GroupID = 1,
                    Name = "Group1",
                    OwnerID = "1",
                };
                context.Groups.Add(group);
                context.SaveChanges();

                PantryDTO pantry = new PantryDTO()
                {
                    GroupID = 1,
                    GroupName = "Group1",
                    Name = "Pantry1"
                };

                repo.UpdatePantry(pantry);

                Assert.AreEqual(1, context.Pantries.Count());

                repo.DeletePantry(pantry);

                Assert.AreEqual(0, context.Pantries.Count());
            }
        }

        [Test]
        public void DeletePantryRemovesFromPantryItems()
        {
            using (var context = new SplitListContext(options))
            {
                PantryRepository repo = new PantryRepository(context);
                context.Database.EnsureCreated();

                Group group = new Group()
                {
                    GroupID = 1,
                    Name = "Group1",
                    OwnerID = "1",
                };
                context.Groups.Add(group);
                context.SaveChanges();

                PantryDTO pantry = new PantryDTO()
                {
                    GroupID = 1,
                    GroupName = "Group1",
                    Name = "Pantry1",
                    Items = new List<ItemDTO>()
                    {
                        new ItemDTO()
                        {
                            Name = "Banana",
                            Amount = 5,
                            Type = "Fruit"
                        }
                    }
                };

                repo.UpdatePantry(pantry);

                Assert.AreEqual(1, context.PantryItems.Count());

                repo.DeletePantry(pantry);

                Assert.AreEqual(0, context.PantryItems.Count());
            }
        }

        [Test]
        public void GetPantryFromGroupIDGetsPantry() 
        {
            using (var context = new SplitListContext(options))
            {
                PantryRepository repo = new PantryRepository(context);
                context.Database.EnsureCreated();

                Group group = new Group()
                {
                    GroupID = 1,
                    Name = "Group1",
                    OwnerID = "1",
                };
                context.Groups.Add(group);
                context.SaveChanges();

                PantryDTO pantry = new PantryDTO()
                {
                    GroupID = 1,
                    GroupName = "Group1",
                    Name = "Pantry1",
                    Items = new List<ItemDTO>()
                    {
                        new ItemDTO()
                        {
                            Name = "Banana",
                            Amount = 5,
                            Type = "Fruit"
                        },
                        new ItemDTO()
                        {
                            Name = "Apple",
                            Amount = 2,
                            Type = "Fruit"
                        }
                    }
                };

                repo.UpdatePantry(pantry);

                PantryDTO pantryDTO = repo.GetPantryFromGroupID(1);
                for (int i = 0; i < pantry.Items.Count; i++)
                {
                    Assert.AreEqual(pantry.Items[i].Name, pantryDTO.Items[i].Name);
                    Assert.AreEqual(pantry.Items[i].Type, pantryDTO.Items[i].Type);
                    Assert.AreEqual(pantry.Items[i].Amount, pantryDTO.Items[i].Amount);
                }
            }
        }
    }
}
