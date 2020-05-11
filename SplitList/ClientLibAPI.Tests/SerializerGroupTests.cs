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

        }

        [Test]
        public void UpdateGroupReturnsUpdatedGroup()
        {

        }

        [Test]
        public void DeleteGroupReturnsHttpResponseOk()
        {

        }
    }
}
