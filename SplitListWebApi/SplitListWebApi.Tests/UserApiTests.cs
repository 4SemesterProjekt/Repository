//using Microsoft.Data.Sqlite;
//using Microsoft.EntityFrameworkCore;
//using NUnit.Framework;
//using SplitListWebApi.Areas.Identity.Data;
//using SplitListWebApi.Repository;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace SplitListWebApi.Tests
//{
//    class UserApiTests
//    {
//        [TestFixture]
//        public class GroupApiTests
//        {
//            private DbContextOptions<SplitListContext> options;
//            private SqliteConnection connection;
//            private UserRepository repo;

//            [SetUp]
//            public void Setup()
//            {
//                connection = new SqliteConnection("DataSource=:memory:");
//                connection.Open();

//                options = new DbContextOptionsBuilder<SplitListContext>()
//                    .UseSqlite(connection)
//                    .Options;

//            }

//            [TearDown]
//            public void TearDown()
//            {
//                connection.Close();
//            }
//        }
//    }
//}
