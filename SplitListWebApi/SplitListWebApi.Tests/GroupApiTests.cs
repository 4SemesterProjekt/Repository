using System.Collections.Generic;
using System.Linq;
using ApiFormat;
using ApiFormat.Group;
using ApiFormat.User;
using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SplitListWebApi.Areas.AutoMapper;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Implementation;
using SplitListWebApi.Services;
using SplitListWebApi.Utilities;

namespace SplitListWebApi.Tests
{
    [TestFixture]
    public class GroupApiTests
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
        public void GetFromDatabaseGetsGroup()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                GroupService service = new GroupService(context, mapper);

                GroupDTO group = new GroupDTO()
                {
                    Name = "TestGroup",
                    OwnerID = 45
                };

                GroupDTO dbGroupDto = service.Create(group);

                Assert.AreEqual(group.Name, dbGroupDto.Name);
            }
        }

        [Test]
        public void UpdateGroupUpdatesGroupProperties()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                GroupService service = new GroupService(context, mapper);

                GroupDTO group = new GroupDTO()
                {
                    Name = "TestGroup",
                    OwnerID = 45
                };

                GroupDTO dbGroupDto = service.Create(group);
                Assert.AreEqual(group.Name, dbGroupDto.Name);
                Assert.AreEqual(group.OwnerID, dbGroupDto.OwnerID);

                dbGroupDto.Name = "GroupTest";
                dbGroupDto.OwnerID = 54;
                GroupDTO updatedGroup = service.Update(dbGroupDto);

                Assert.AreEqual(updatedGroup.OwnerID, dbGroupDto.OwnerID);
                Assert.AreEqual(updatedGroup.Name, dbGroupDto.Name);
            }
        }


        [Test]
        public void GroupWithUsersCreatesUserGroups()
        {
            using (var context = new SplitListContext(options))
            {

                context.Database.EnsureCreated();
                GroupService service = new GroupService(context, mapper);
                UserService userService = new UserService(context, mapper);

                UserDTO jordkim = new UserDTO()
                {
                    Name = "Jordkim",
                    Id = "1234567890"
                };

                var entry = context.Users.Add(mapper.Map<UserModel>(jordkim));
                entry.State = EntityState.Detached;
                context.SaveChanges();
                

                UserDTO theBetterJordkim = new UserDTO()
                {
                    Name = "Jordkim The Master of EVERYTHING",
                    Id = "etellerandetlort-nikolaj2020"
                };

                entry = context.Users.Add(mapper.Map<UserModel>(theBetterJordkim));
                entry.State = EntityState.Detached;
                context.SaveChanges();
                

                GroupDTO group = new GroupDTO()
                {
                    Name = "TestGroup",
                    OwnerID = 45,
                    Users = new List<UserDTO>()
                    {
                        jordkim,
                        theBetterJordkim
                    }
                };

                GroupDTO dbGroupDto = service.Create(group);

                Assert.AreEqual(dbGroupDto.Users.Count, group.Users.Count);
            }

        }

        [Test]
        public void UpdateGroupUpdatesUsers()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                GroupService service = new GroupService(context, mapper);

                UserDTO jordkim = new UserDTO()
                {
                    Name = "Jordkim",
                    Id = "1234567890"
                };

                var entry = context.Users.Add(mapper.Map<UserModel>(jordkim));
                entry.State = EntityState.Detached;
                context.SaveChanges();

                //her er jordkim endnu ikke registreret i context.Users, selvom forventet?

                GroupDTO group = new GroupDTO()
                {
                    Name = "TestGroup",
                    OwnerID = 45,
                    Users = new List<UserDTO>()
                    {
                        jordkim
                    }
                };

                var groupFromDB = service.Create(group);

                Assert.AreEqual(1, groupFromDB.Users.Count);

                UserDTO theBetterJordkim = new UserDTO()
                {
                    Name = "Jordkim The Master of EVERYTHING",
                    Id = "etellerandetlort-nikolaj2020"
                };

                entry = context.Users.Add(mapper.Map<UserModel>(theBetterJordkim));
                entry.State = EntityState.Detached;
                context.SaveChanges();


                groupFromDB.Users.Add(theBetterJordkim);

                groupFromDB = service.Update(groupFromDB);

                Assert.AreEqual(2, groupFromDB.Users.Count);
                Assert.AreEqual("Jordkim The Master of EVERYTHING", groupFromDB.Users[1].Name);
            }
        }
    }
}


