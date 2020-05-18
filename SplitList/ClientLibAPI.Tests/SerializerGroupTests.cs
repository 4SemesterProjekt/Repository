using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using ClientLibAPI;
using ApiFormat.Group;

namespace ClientLibAPI.Tests
{
    [TestFixture]
    public class SerializerGroupTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void GetGroupByIdReturnsGroupFromId()
        {
            var group = SerializerGroup.CreateGroup(new GroupDTO()
            {
                Name = "Group Test",
                OwnerID = "123"
            }).Result;

            var groupGet = SerializerGroup.GetGroupById(group.ModelId).Result;
            var result = SerializerGroup.DeleteGroup(group).Result;
            Assert.AreEqual(group.ModelId, groupGet.ModelId);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void CreateGroupReturnsCreatedGroup()
        {
            var group = new GroupDTO()
            {
                Name = "Group Test",
                OwnerID = "123"
            };

            var groupCreated = SerializerGroup.CreateGroup(group).Result;
            Assert.AreEqual(group.Name, groupCreated.Name);
            Assert.AreEqual(group.OwnerID, groupCreated.OwnerID);
            var result = SerializerGroup.DeleteGroup(group).Result;
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        }

        [Test]
        public void UpdateGroupReturnsUpdatedGroup()
        {
            var group = SerializerGroup.CreateGroup(new GroupDTO()
            {
                Name = "Group Test",
                OwnerID = "123"
            }).Result;
            group.Name = "Group Test123";
            var groupUpdated = SerializerGroup.UpdateGroup(group).Result;
            Assert.AreEqual(group.Name, groupUpdated.Name);
            Assert.AreEqual(group.OwnerID, groupUpdated.OwnerID);
            Assert.AreEqual(group.ModelId, groupUpdated.ModelId);
            var result = SerializerGroup.DeleteGroup(group).Result;
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        }

        [Test]
        public void DeleteGroupReturnsHttpResponseOk()
        {
            var group = SerializerGroup.CreateGroup(new GroupDTO()
            {
                Name = "Group Test",
                OwnerID = "123"
            }).Result;

            var result = SerializerGroup.DeleteGroup(group).Result;
   
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        }
    }
}
