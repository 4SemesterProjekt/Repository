using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiFormat;
using ApiFormat.Group;
using ApiFormat.ShoppingList;
using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SplitListWebApi.Areas.AutoMapper;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Implementation;
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
        public void RepoAddsSLToDb()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                
            }
        }
    }
}

