using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiFormat.Group;
using ApiFormat.Item;
using ApiFormat.Recipe;
using ApiFormat.ShoppingList;
using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SplitListWebApi.Areas.AutoMapper;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Services;

namespace SplitListWebApi.Tests
{
    [TestFixture]
    class RecipeApiTests
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
                cfg.AddProfile<RecipeProfile>();
            });
            mapper = config.CreateMapper();
        }

        [TearDown]
        public void TearDown()
        {
            connection.Close();
        }

        [Test]
        public void GetFromDatabaseGetsRecipe()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                RecipeService recipeService = new RecipeService(context, mapper);

                RecipeDTO recipeDto = new RecipeDTO()
                {
                    Name = "The Best Recipe",
                    Introduction = "It's the best",
                    Method = "Get Gut"
                };

                var recipeFromDb = recipeService.Create(recipeDto);

                Assert.AreEqual(1, context.Recipes.Count());
                Assert.AreEqual(recipeDto.Name, recipeFromDb.Name);
                Assert.AreEqual(recipeDto.Method, recipeFromDb.Method);
            }
        }

        [Test]
        public void UpdateOnRecipeUpdatesProperties()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                RecipeService recipeService = new RecipeService(context, mapper);

                RecipeDTO recipeDto = new RecipeDTO()
                {
                    Name = "The Best Recipe",
                    Introduction = "It's the best",
                    Method = "Get Gut"
                };

                var recipeFromDb = recipeService.Create(recipeDto);

                recipeFromDb.Name = "PSYKE, Now it's the best.";
                recipeFromDb.Method = "Get really gut";

                var updatedRecipeFromDb = recipeService.Update(recipeFromDb);

                Assert.AreEqual(recipeFromDb.Name, updatedRecipeFromDb.Name);
                Assert.AreEqual(recipeFromDb.Method, updatedRecipeFromDb.Method);
            }
        }

        [Test]
        public void CreateOnDatabaseCreatesItems()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                RecipeService recipeService = new RecipeService(context, mapper);
                ItemService itemService = new ItemService(context, mapper);

                ItemDTO item1 = new ItemDTO()
                {
                    Amount = 3,
                    Name = "Banana",
                    Type = "Fruit"
                };

                itemService.Create(item1);

                ItemDTO item2 = new ItemDTO()
                {
                    Amount = 10000,
                    Name = "Orange",
                    Type = "Fruit"
                };

                itemService.Create(item2);

                ItemDTO newItem = new ItemDTO()
                {
                    Amount = -3,
                    Name = "Ryebread",
                    Type = "Bread"
                };

                //itemService.Create() not run, to check shadowtable repository.

                RecipeDTO recipeDto = new RecipeDTO()
                {
                    Name = "The Best Recipe",
                    Introduction = "It's the best",
                    Method = "Get Gut",
                    Items = new List<ItemDTO>()
                    {
                        item1,
                        item2,
                        newItem
                    }
                };

                var recipeFromDb = recipeService.Create(recipeDto);

                Assert.AreEqual(3, context.Items.Count());
                Assert.AreEqual(context.RecipeItems.Count(), 3);
                Assert.AreEqual(recipeFromDb.Items[0].Name, item1.Name);
            }
        }

        [Test]
        public void DeleteOnRecipeDeletesRecipeAndRecipeItems()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                RecipeService recipeService = new RecipeService(context, mapper);
                ItemService itemService = new ItemService(context, mapper);

                ItemDTO item1 = new ItemDTO()
                {
                    Amount = 3,
                    Name = "Banana",
                    Type = "Fruit"
                };

                itemService.Create(item1);

                ItemDTO item2 = new ItemDTO()
                {
                    Amount = 10000,
                    Name = "Orange",
                    Type = "Fruit"
                };

                itemService.Create(item2);

                ItemDTO newItem = new ItemDTO()
                {
                    Amount = -3,
                    Name = "Ryebread",
                    Type = "Bread"
                };

                //itemService.Create() not run, to check shadowtable repository.

                RecipeDTO recipeDto = new RecipeDTO()
                {
                    Name = "The Best Recipe",
                    Introduction = "It's the best",
                    Method = "Get Gut",
                    Items = new List<ItemDTO>()
                    {
                        item1,
                        item2,
                        newItem
                    }
                };

                var recipeFromDb = recipeService.Create(recipeDto);

                recipeService.Delete(recipeFromDb);

                Assert.AreEqual(context.Recipes.Count(), 0);
                Assert.AreEqual(context.Items.Count(), 3);
                Assert.AreEqual(context.RecipeItems.Count(), 0);
            }
        }

        [Test]
        public void GetByIdsOnServiceGetsRecipes()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                RecipeService recipeService = new RecipeService(context, mapper);
                ItemService itemService = new ItemService(context, mapper);

                ItemDTO item1 = new ItemDTO()
                {
                    Amount = 3,
                    Name = "Banana",
                    Type = "Fruit"
                };

                itemService.Create(item1);

                ItemDTO item2 = new ItemDTO()
                {
                    Amount = 10000,
                    Name = "Orange",
                    Type = "Fruit"
                };

                itemService.Create(item2);

                ItemDTO newItem = new ItemDTO()
                {
                    Amount = -3,
                    Name = "Ryebread",
                    Type = "Bread"
                };

                RecipeDTO recipeDto = new RecipeDTO()
                {
                    Name = "The Best Recipe",
                    Introduction = "It's the best",
                    Method = "Get Gut",
                    Items = new List<ItemDTO>()
                    {
                        item1,
                        item2,
                        newItem
                    }
                };

                var recipe1FromDb = recipeService.Create(recipeDto);

                RecipeDTO recipe2Dto = new RecipeDTO()
                {
                    Name = "The Best Recipe",
                    Introduction = "No this is the best",
                    Method = "Get even Gutter",
                    Items = new List<ItemDTO>()
                    {
                        item1,
                        item2
                    }
                };

                var recipe2FromDb = recipeService.Create(recipe2Dto);

                var recipes = recipeService.GetByIds(new[] {recipe1FromDb.ModelId, recipe2FromDb.ModelId});

                Assert.AreEqual(recipes[0].Introduction, recipe1FromDb.Introduction);
                Assert.AreEqual(recipes[1].Introduction, recipe2FromDb.Introduction);
            }
        }

        [Test]
        public void DeleteNonExistingRecipeThrowsNullReferenceException()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                RecipeService recipeService = new RecipeService(context, mapper);

                RecipeDTO recipeDto = new RecipeDTO()
                {
                    Name = "The Best Recipe",
                    Introduction = "It's the best",
                    Method = "Get Gut"
                };

                Assert.Throws(typeof(NullReferenceException), () => recipeService.Delete(recipeDto));
            }
        }
    }
}
