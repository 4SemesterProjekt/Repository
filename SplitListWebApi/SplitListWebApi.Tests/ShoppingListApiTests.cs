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
        IMapper mapper;

        [SetUp]
        public void Setup()
        {
            options = new DbContextOptions<SplitListContext>(opt => opt.)
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
        public void RepoAddsSLToDb()
        {
            using (var context = new SplitListContext(options))
            {
                context.Database.EnsureCreated();
                GenericRepository<GroupDTO, GroupModel> groupRepo = new GenericRepository<GroupDTO, GroupModel>(context, mapper);
                GenericRepository<ShoppingListDTO, ShoppingListModel> slRepo = new GenericRepository<ShoppingListDTO, ShoppingListModel>(context, mapper);
                GroupDTO groupDTO = new GroupDTO()
                {
                    Name = "TestGroup",
                    OwnerID = 1
                };

                groupDTO = groupDTO.Add(groupRepo);

                ShoppingListDTO slDTO = new ShoppingListDTO()
                {
                    Name = "TestList",
                    GroupID = groupDTO.ModelId,
                    Group = groupDTO
                };

                slDTO = slDTO.Add(slRepo);
                ShoppingListDTO dbList = slRepo.GetById(slDTO.ModelId);

                Assert.AreEqual(dbList.Name, slDTO.Name);
                Assert.AreEqual(dbList.Group.Name, slDTO.Group.Name);
            }
        }
    }
}

