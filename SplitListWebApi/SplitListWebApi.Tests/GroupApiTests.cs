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
    public class GroupApiTests
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
        public void AddGroupAddsToDatabase()
        {
            using (var context = new SplitListContext(options))
            {
                IGroupRepository groupRepo = new GroupRepository(context);

                context.Database.EnsureCreated();

                GroupDTO group = new GroupDTO()
                {
                    Name = "Group1",
                    OwnerID = 1
                };
                groupRepo.AddGroup(group);

                Assert.AreEqual(1, context.Groups.Count());
                Assert.AreEqual("Group1", context.Groups.FirstOrDefault().Name);
                Assert.AreEqual(1, context.Groups.FirstOrDefault().OwnerID);
                Assert.AreEqual(1, context.Groups.FirstOrDefault().GroupID);
            }
        }

        [Test]
        public void DeleteGroupDeletesGroupFromDatabase()
        {
            using (var context = new SplitListContext(options))
            {
                IGroupRepository groupRepo = new GroupRepository(context);

                context.Database.EnsureCreated();

                GroupDTO group = new GroupDTO()
                {
                    Name = "Group1",
                    OwnerID = 1,
                    GroupID = 1
                };
                groupRepo.AddGroup(group);
                groupRepo.DeleteGroup(group);

                Assert.AreEqual(0, context.Groups.Count());
            }
        }

        [Test]
        public void GetUsersInGroupReturnsUsers()
        {
            using (var context = new SplitListContext(options))
            {
                IGroupRepository groupRepo = new GroupRepository(context);

                context.Database.EnsureCreated();

                GroupDTO group = new GroupDTO()
                {
                    Name = "Group1",
                    OwnerID = 1,
                    GroupID = 1
                };
                groupRepo.AddGroup(group);

                User user1 = new User()
                {
                    Name = "User1",
                    UserID = 1
                };

                context.Users.Add(user1);
                context.SaveChanges();

                User user2 = new User()
                {
                    Name = "User2",
                    UserID = 2
                };

                context.Users.Add(user2);
                context.SaveChanges();

                UserGroup userGroup1 = new UserGroup()
                {
                    GroupID = 1,
                    UserID = 1
                };

                UserGroup userGroup2 = new UserGroup()
                {
                    GroupID = 1,
                    UserID = 2
                };

                List<User> users = new List<User>()
                {
                    user1, user2
                };

                context.UserGroups.Add(userGroup1);
                context.UserGroups.Add(userGroup2);
                context.SaveChanges();

                List<UserDTO> usersDTO = groupRepo.GetUsersInGroup(group);
                Assert.AreEqual(2, users.Count);

                for (int i = 0; i < 2; i++)
                {
                    Assert.AreEqual(users[i].Name, usersDTO[i].Name);
                    Assert.AreEqual(users[i].UserID, usersDTO[i].UserID);
                }
            }
        }

        [Test]
        public void UpdateGroupUpdatesGroupOnly()
        {
            using (var context = new SplitListContext(options))
            {
                IGroupRepository groupRepo = new GroupRepository(context);

                context.Database.EnsureCreated();

                GroupDTO group = new GroupDTO()
                {
                    Name = "Group1",
                    OwnerID = 1,
                    GroupID = 1
                };
                groupRepo.AddGroup(group);

                group.Name = "GroupUpdated";
                group.OwnerID = 2;
                groupRepo.UpdateGroup(group);

                Assert.AreEqual("GroupUpdated", context.Groups.FirstOrDefault().Name);
                Assert.AreEqual(2, context.Groups.FirstOrDefault().OwnerID);
            }
        }

        [Test]
        public void UpdateGroupAddsUsersToGroup()
        {
            using (var context = new SplitListContext(options))
            {
                IGroupRepository groupRepo = new GroupRepository(context);

                context.Database.EnsureCreated();

                GroupDTO group = new GroupDTO()
                {
                    Name = "Group1",
                    OwnerID = 1,
                    GroupID = 1
                };
                groupRepo.AddGroup(group);

                group.Users = new List<UserDTO>()
                {
                    new UserDTO() { Name = "User1", UserID = 1}
                };

                groupRepo.UpdateGroup(group);
                Assert.AreEqual("User1", context.UserGroups.FirstOrDefault().User.Name);
                Assert.AreEqual("Group1", context.UserGroups.FirstOrDefault().Group.Name);

                group.Users.Add(new UserDTO()
                {
                    Name = "User2", UserID = 2
                });

                groupRepo.UpdateGroup(group);

                List<UserGroup> userGroups = context.UserGroups.ToList();

                for (int i = 0; i < 2; i++)
                {
                    Assert.AreEqual(group.Users[i].Name, userGroups[i].User.Name);
                    Assert.AreEqual("Group1", userGroups[i].Group.Name);
                }
            }
        }

        [Test]
        public void UpdateGroupRemovesUsersToGroup()
        {
            using (var context = new SplitListContext(options))
            {
                IGroupRepository groupRepo = new GroupRepository(context);

                context.Database.EnsureCreated();

                GroupDTO group = new GroupDTO()
                {
                    Name = "Group1",
                    OwnerID = 1,
                    GroupID = 1
                };
                groupRepo.AddGroup(group);

                group.Users = new List<UserDTO>()
                {
                    new UserDTO() { Name = "User1", UserID = 1},
                    new UserDTO() { Name = "User2", UserID = 2}
                };

                groupRepo.UpdateGroup(group);

                Assert.AreEqual(2, context.UserGroups.Count());

                group.Users.RemoveAt(1);
                groupRepo.UpdateGroup(group);

                Assert.AreEqual(1, context.UserGroups.Count());
                Assert.AreEqual(2, context.Users.Count());
            }
        }

        [Test]
        public void GetOwnerOfGroupReturnsOwner()
        {
            using (var context = new SplitListContext(options))
            {
                IGroupRepository groupRepo = new GroupRepository(context);

                context.Database.EnsureCreated();

                GroupDTO group = new GroupDTO()
                {
                    Name = "Group1",
                    OwnerID = 1,
                    GroupID = 1
                };
                groupRepo.AddGroup(group);

                group.Users = new List<UserDTO>()
                {
                    new UserDTO() {Name = "User1", UserID = 1},
                    new UserDTO() {Name = "User2", UserID = 2}
                };

                groupRepo.UpdateGroup(group);

                UserDTO owner = groupRepo.GetOwnerOfGroup(group);

                Assert.AreEqual("User1", owner.Name);
                Assert.AreEqual(1, owner.UserID);
            }
        }

        [Test]
        public void GetGroupByIDReturnsGroup()
        {
            using (var context = new SplitListContext(options))
            {
                IGroupRepository groupRepo = new GroupRepository(context);

                context.Database.EnsureCreated();

                GroupDTO group = new GroupDTO()
                {
                    Name = "Group1",
                    OwnerID = 1,
                    GroupID = 1
                };
                groupRepo.AddGroup(group);

                group.Users = new List<UserDTO>()
                {
                    new UserDTO() {Name = "User1", UserID = 1},
                    new UserDTO() {Name = "User2", UserID = 2}
                };

                groupRepo.UpdateGroup(group);

                GroupDTO correctGroup = groupRepo.GetGroupByGroupID(1);

                Assert.AreEqual("Group1", correctGroup.Name);
                Assert.AreEqual(1, correctGroup.GroupID);
                Assert.AreEqual(1, correctGroup.OwnerID);
                Assert.AreEqual(2, correctGroup.Users.Count);
            }
        }
    }
}