﻿//using System.Collections.Generic;
//using System.Linq;
//using ApiFormat;
//using Microsoft.Data.Sqlite;
//using Microsoft.EntityFrameworkCore;
//using NUnit.Framework;
//using SplitListWebApi.Areas.Identity.Data;
//using SplitListWebApi.Repository;

//namespace SplitListWebApi.Tests
//{
//    [TestFixture]
//    public class GroupApiTests
//    {
//        private DbContextOptions<SplitListContext> options;
//        private SqliteConnection connection;

//        [SetUp]
//        public void Setup()
//        {
//            connection = new SqliteConnection("DataSource=:memory:");
//            connection.Open();

//            options = new DbContextOptionsBuilder<SplitListContext>()
//                .UseSqlite(connection)
//                .Options;
//        }

//        [TearDown]
//        public void TearDown()
//        {
//            connection.Close();
//        }



//        [Test]
//        public void UpdateGroupAddsToDatabase()
//        {
//            using (var context = new SplitListContext(options))
//            {
//                IGroupRepository groupRepo = new GenericRepository(context);

//                context.Database.EnsureCreated();

//                IGroupDTO group = new IGroupDTO()
//                {
//                    Name = "Group1",
//                    OwnerID = "1"
//                };
//                groupRepo.UpdateGroup(group);

//                Assert.AreEqual(1, context.Groups.Count());
//                Assert.AreEqual("Group1", context.Groups.FirstOrDefault().Name);
//                Assert.AreEqual("1", context.Groups.FirstOrDefault().OwnerID);
//                Assert.AreEqual(1, context.Groups.FirstOrDefault().GroupModelId);
//            }
//        }

//        [Test]
//        public void DeleteGroupDeletesGroupFromDatabase()
//        {
//            using (var context = new SplitListContext(options))
//            {
//                IGroupRepository groupRepo = new GenericRepository(context);

//                context.Database.EnsureCreated();

//                IGroupDTO group = new IGroupDTO()
//                {
//                    Name = "Group1",
//                    OwnerID = "1"
//                };
//                groupRepo.UpdateGroup(group);
//                groupRepo.DeleteGroup(group);

//                Assert.AreEqual(0, context.Groups.Count());
//            }
//        }

//        [Test]
//        public void GetUsersInGroupReturnsUsers()
//        {
//            using (var context = new SplitListContext(options))
//            {
//                IGroupRepository groupRepo = new GenericRepository(context);

//                context.Database.EnsureCreated();

//                IGroupDTO group = new IGroupDTO()
//                {
//                    Name = "Group1",
//                    OwnerID = "1"
//                };
//                groupRepo.UpdateGroup(group);

//                User user1 = new User()
//                {
//                    Name = "User1",
//                    Id = "1"
//                };

//                context.Users.Add(user1);
//                context.SaveChanges();

//                User user2 = new User()
//                {
//                    Name = "User2",
//                    Id = "2"
//                };

//                context.Users.Add(user2);
//                context.SaveChanges();

//                UserGroup userGroup1 = new UserGroup()
//                {
//                    GroupModelId = 1,
//                    Id = "1"
//                };

//                UserGroup userGroup2 = new UserGroup()
//                {
//                    GroupModelId = 1,
//                    Id = "2"
//                };

//                List<User> users = new List<User>()
//                {
//                    user1, user2
//                };

//                context.UserGroups.Add(userGroup1);
//                context.UserGroups.Add(userGroup2);
//                context.SaveChanges();

//                IGroupDTO GroupFromDb = groupRepo.GetGroupByGroupID(group.GroupModelId);
//                Assert.AreEqual(2, users.Count);

//                for (int i = 0; i < 2; i++)
//                {
//                    Assert.AreEqual(users[i].Name, GroupFromDb.Users[i].Name);
//                    Assert.AreEqual(users[i].Id, GroupFromDb.Users[i].Id);
//                }
//            }
//        }

//        [Test]
//        public void UpdateGroupUpdatesGroupOnly()
//        {
//            using (var context = new SplitListContext(options))
//            {
//                IGroupRepository groupRepo = new GenericRepository(context);

//                context.Database.EnsureCreated();

//                IGroupDTO group = new IGroupDTO()
//                {
//                    Name = "Group1",
//                    OwnerID = "1"
//                };
//                groupRepo.UpdateGroup(group);

//                group.Name = "GroupUpdated";
//                group.OwnerID = "2";
//                groupRepo.UpdateGroup(group);

//                Assert.AreEqual("GroupUpdated", context.Groups.FirstOrDefault().Name);
//                Assert.AreEqual("2", context.Groups.FirstOrDefault().OwnerID);
//            }
//        }

//        [Test]
//        public void UpdateGroupAddsUsersToGroup()
//        {
//            using (var context = new SplitListContext(options))
//            {
//                IGroupRepository groupRepo = new GenericRepository(context);

//                context.Database.EnsureCreated();

//                IGroupDTO group = new IGroupDTO()
//                {
//                    Name = "Group1",
//                    OwnerID = "1"
//                };
//                groupRepo.UpdateGroup(group);

//                group.Users = new List<UserDTO>()
//                {
//                    new UserDTO() { Name = "User1", Id = "1"}
//                };

//                groupRepo.UpdateGroup(group);
//                Assert.AreEqual("User1", context.UserGroups.FirstOrDefault().User.Name);
//                Assert.AreEqual("Group1", context.UserGroups.FirstOrDefault().Group.Name);

//                group.Users.Add(new UserDTO()
//                {
//                    Name = "User2", Id = "2"
//                });

//                groupRepo.UpdateGroup(group);

//                List<UserGroup> userGroups = context.UserGroups.ToList();

//                for (int i = 0; i < 2; i++)
//                {
//                    Assert.AreEqual(group.Users[i].Name, userGroups[i].User.Name);
//                    Assert.AreEqual("Group1", userGroups[i].Group.Name);
//                }
//            }
//        }

//        [Test]
//        public void UpdateGroupRemovesUsersFromGroup()
//        {
//            using (var context = new SplitListContext(options))
//            {
//                IGroupRepository groupRepo = new GenericRepository(context);

//                context.Database.EnsureCreated();

//                IGroupDTO group = new IGroupDTO()
//                {
//                    Name = "Group1",
//                    OwnerID = "1"
//                };
//                groupRepo.UpdateGroup(group);

//                group.Users = new List<UserDTO>()
//                {
//                    new UserDTO() { Name = "User1", Id = "1"},
//                    new UserDTO() { Name = "User2", Id = "2"}
//                };

//                groupRepo.UpdateGroup(group);

//                Assert.AreEqual(2, context.UserGroups.Count());

//                group.Users.RemoveAt(1);
//                groupRepo.UpdateGroup(group);

//                Assert.AreEqual(1, context.UserGroups.Count());
//                Assert.AreEqual(2, context.Users.Count());
//            }
//        }

//        [Test]
//        public void GetOwnerOfGroupReturnsOwner()
//        {
//            using (var context = new SplitListContext(options))
//            {
//                IGroupRepository groupRepo = new GenericRepository(context);

//                context.Database.EnsureCreated();

//                IGroupDTO group = new IGroupDTO()
//                {
//                    Name = "Group1",
//                    OwnerID = "1"
//                };
//                groupRepo.UpdateGroup(group);

//                group.Users = new List<UserDTO>()
//                {
//                    new UserDTO() {Name = "User1", Id = "1"},
//                    new UserDTO() {Name = "User2", Id = "2"}
//                };

//                groupRepo.UpdateGroup(group);

//                UserDTO owner = groupRepo.GetOwnerOfGroup(group);

//                Assert.AreEqual("User1", owner.Name);
//                Assert.AreEqual("1", owner.Id);
//            }
//        }

//        [Test]
//        public void GetGroupByIDReturnsGroup()
//        {
//            using (var context = new SplitListContext(options))
//            {
//                IGroupRepository groupRepo = new GenericRepository(context);

//                context.Database.EnsureCreated();

//                IGroupDTO group = new IGroupDTO()
//                {
//                    Name = "Group1",
//                    OwnerID = "1"
//                };
//                groupRepo.UpdateGroup(group);

//                group.Users = new List<UserDTO>()
//                {
//                    new UserDTO() {Name = "User1", Id = "1"},
//                    new UserDTO() {Name = "User2", Id = "2"}
//                };

//                groupRepo.UpdateGroup(group);

//                IGroupDTO correctGroup = groupRepo.GetGroupByGroupID(1);

//                Assert.AreEqual("Group1", correctGroup.Name);
//                Assert.AreEqual(1, correctGroup.GroupModelId);
//                Assert.AreEqual("1", correctGroup.OwnerID);
//                Assert.AreEqual(2, correctGroup.Users.Count);
//            }

//        }
//    }
//}