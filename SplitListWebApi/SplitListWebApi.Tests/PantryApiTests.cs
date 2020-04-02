using System.Collections.Generic;
using System.Linq;
using ApiFormat;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
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
        void UdatePantryAddsNewPantry()
        {

        }

        [Test]
        void UpdatePantryUpdatesItems()
        {

        }

        [Test]
        void DeletePantryRemovesPantry()
        {

        }

        [Test]
        void DeletePantryRemovesFromPantryItem()
        {

        }

        [Test]
        void GetPantryFromGroupIDGetsPantry() 
        {
        
        }

    }
}
