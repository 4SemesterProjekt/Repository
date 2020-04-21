﻿using System.Collections.Generic;
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

                GenericRepository<GroupDTO, GroupModel> groupRepo = new GenericRepository<GroupDTO, GroupModel>(context, mapper);
                context.Database.EnsureCreated();

                GroupDTO group = new GroupDTO()
                {
                    Name = "TestGroup",
                    OwnerID = 45
                };

                group = group.Add(groupRepo);

                GroupDTO groupDTO = groupRepo.GetById(group.ModelId);

                Assert.AreEqual(groupDTO.Name, group.Name);
                Assert.AreEqual(groupDTO.OwnerID, group.OwnerID);
            }
        }

        [Test]
        public void UpdateGroupUpdatesProperties()
        {
            using (var context = new SplitListContext(options))
            {

                GenericRepository<GroupDTO, GroupModel> groupRepo = new GenericRepository<GroupDTO, GroupModel>(context, mapper);
                context.Database.EnsureCreated();

                GroupDTO group = new GroupDTO()
                {
                    Name = "TestGroup",
                    OwnerID = 45
                };

                group = group.Add(groupRepo);

                GroupDTO groupDTO = groupRepo.GetById(group.ModelId);
                groupDTO.Name = "GroupTest";
                groupDTO.OwnerID = 54;

                groupDTO.Save(groupRepo);
                GroupDTO dbGroup = groupRepo.GetById(group.ModelId);

                Assert.AreEqual(groupDTO.Name, dbGroup.Name);
                Assert.AreEqual(groupDTO.OwnerID, dbGroup.OwnerID);
            }

        }

        [Test]
        public void UpdateGroupUsers()
        {
            using (var context = new SplitListContext(options))
            {

                GenericRepository<GroupDTO, GroupModel> groupRepo = new GenericRepository<GroupDTO, GroupModel>(context, mapper);
                GenericRepository<UserDTO, UserModel> userRepo = new GenericRepository<UserDTO, UserModel>(context, mapper);
                context.Database.EnsureCreated();

                GroupDTO group = new GroupDTO()
                {
                    Name = "TestGroup",
                    OwnerID = 45,
                    Users = new List<UserDTO>()
                };

                group = group.Add(groupRepo);

                GroupDTO groupDTO = groupRepo.GetById(group.ModelId);

                var karl = new UserDTO()
                {
                    Groups = new List<GroupDTO>() { group },
                    Id = "abcetellerandetlort",
                    Name = "Karl"
                };

                karl = karl.Add(userRepo);

                groupDTO.Users.Add(karl);

                groupDTO.Save(groupRepo);
                GroupDTO dbGroup = groupRepo.GetById(groupDTO.ModelId);

                Assert.AreEqual(groupDTO.Name, dbGroup.Name);
                Assert.AreEqual(groupDTO.OwnerID, dbGroup.OwnerID);
                Assert.AreEqual(1, dbGroup.Users.Count);
            }
        }
    }
}

