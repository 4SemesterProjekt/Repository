using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SplitListWebApi.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiFormat.Group;
using ApiFormat.User;
using AutoMapper;
using SplitListWebApi.Areas.AutoMapper;
using SplitListWebApi.Repositories.Implementation;
using SplitListWebApi.Services;

namespace SplitListWebApi.Tests
{
    [TestFixture]
    public class UserApiTests
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
        public void CreateUserOnRepoCreatesUser()
        {
            using (var db = new SplitListContext(options))
            { 
                UserService _userService = new UserService(db, mapper);
                GenericRepository<UserModel> _userRepo = new GenericRepository<UserModel>(db);
                db.Database.EnsureCreated();
                UserDTO dto = new UserDTO()
                {
                    Name = "Karl Jørgen",
                    Id = "1"
                };
                UserModel model = mapper.Map<UserModel>(dto);
                _userRepo.Create(model);

                Assert.That(db.Users.Count(), Is.EqualTo(1));
            }
        }

        [Test]
        public void UpdateUserUpdatesProperties()
        {
            using (var db = new SplitListContext(options))
            {
                UserService _userService = new UserService(db, mapper);
                GenericRepository<UserModel> _userRepo = new GenericRepository<UserModel>(db);

                db.Database.EnsureCreated();
                UserDTO dto = new UserDTO()
                {
                    Name = "Karl Jørgen",
                    Id = "1"
                };
                UserModel model = mapper.Map<UserModel>(dto);
                model = _userRepo.Create(model);

                dto.Name = "Karl Jørgen v2";
                dto = _userService.Update(dto);

                Assert.That(dto.Name, Is.EqualTo("Karl Jørgen v2"));
            }
        }

        [Test]
        public void CreateGroupWithUserAddsGroupToUserDTO()
        {
            using (var db = new SplitListContext(options))
            {
                UserService _userService = new UserService(db, mapper);
                GroupService _groupService = new GroupService(db, mapper);
                GenericRepository<UserModel> _userRepo = new GenericRepository<UserModel>(db);
                db.Database.EnsureCreated();



                UserDTO dto = new UserDTO()
                {
                    Name = "Karl Jørgen",
                    Id = "1"
                };

                UserModel model = mapper.Map<UserModel>(dto);
                model = _userRepo.Create(model);

                GroupDTO groupDto = new GroupDTO()
                {
                    Name = "Karl Jørgens Gruppe",
                    OwnerID = model.Id,
                    Users = new List<UserDTO>() { mapper.Map<UserDTO>(model) }
                };

                _groupService.Create(groupDto);
                dto = _userService.GetById(model.Id);

                Assert.That(dto.Groups.FirstOrDefault().Name, Is.EqualTo(groupDto.Name));
                Assert.That(db.UserGroups.Count(), Is.EqualTo(1));
            }
        }

        [Test]
        public void DeleteUserDeletesUserAndUserGroups()
        {
            using (var db = new SplitListContext(options))
            {
                UserService _userService = new UserService(db, mapper);
                GroupService _groupService = new GroupService(db, mapper);
                GenericRepository<UserModel> _userRepo = new GenericRepository<UserModel>(db);
                db.Database.EnsureCreated();

                UserDTO dto = new UserDTO()
                {
                    Name = "Karl Jørgen",
                    Id = "1"
                };

                UserModel model = mapper.Map<UserModel>(dto);
                model = _userRepo.Create(model);

                GroupDTO groupDto = new GroupDTO()
                {
                    Name = "Karl Jørgens Gruppe",
                    OwnerID = model.Id,
                    Users = new List<UserDTO>() { mapper.Map<UserDTO>(model) }
                };

                _groupService.Create(groupDto);
                _userService.Delete(mapper.Map<UserDTO>(model));

                Assert.IsEmpty(db.Users);
                Assert.IsEmpty(db.UserGroups);
            }
        }

        [Test]
        public void GetUserByEmailGetsUser()
        {
            using (var db = new SplitListContext(options))
            {
                UserService _userService = new UserService(db, mapper);
                GenericRepository<UserModel> _userRepo = new GenericRepository<UserModel>(db);
                db.Database.EnsureCreated();
                UserDTO dto = new UserDTO()
                {
                    Name = "Karl Jørgen",
                    Id = "1",
                    Email = "karl@joergensen.com"
                };
                UserModel model = mapper.Map<UserModel>(dto);
                _userRepo.Create(model);

                var userFromDb =_userService.GetByEmail(dto.Email);

                Assert.That(userFromDb.Name, Is.EqualTo(dto.Name));
            }
        }
    }
}
