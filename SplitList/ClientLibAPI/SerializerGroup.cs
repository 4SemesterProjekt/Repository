using System;
using System.Collections.Generic;
using System.Text;
using ApiFormat;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ApiFormat.Group;

namespace ClientLibAPI
{
    public class SerializerGroup
    {
        const string URL = "https://splitlistwebapi.azurewebsites.net/api/Groups/";

        // GET: api/Groups/5
        // Return a GroupDTO based on GroupId
        public static async Task<GroupDTO> GetGroupById(int groupId)
        {
            var groupByIdString = await MSerializer.Client.GetStringAsync($"{URL}{groupId}");
            var groupByGroupId = JsonConvert.DeserializeObject<GroupDTO>(groupByIdString);
            return groupByGroupId;
        }

        // POST: api/Groups
        // Creates Group from parameter
        public static async Task<GroupDTO> CreateGroup(GroupDTO group)
        {
            var context = JsonConvert.SerializeObject(group, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Post, URL))
            {
                var httpContext = new StringContent(context, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                var response = await MSerializer.Client.SendAsync(request);
                var updatedGroup = JsonConvert.DeserializeObject<GroupDTO>(await response.Content.ReadAsStringAsync());
                return updatedGroup;
            }
        }

        // PUT: api/Groups
        // Updates Group from parameter
        public static async Task<GroupDTO> UpdateGroup(GroupDTO group)
        {
            var context = JsonConvert.SerializeObject(group, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Put, URL))
            {
                var httpContext = new StringContent(context, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                var response = await MSerializer.Client.SendAsync(request);
                var updatedGroup = JsonConvert.DeserializeObject<GroupDTO>(await response.Content.ReadAsStringAsync());
                return updatedGroup;
            }
        }

        // DELETE: api/Groups/5
        // If Id exists in DB remove entity with that Id
        // If Id does not exist, do nothing
        public static async Task<HttpResponseMessage> DeleteGroup(GroupDTO group)
        {
            var groupContext = JsonConvert.SerializeObject(group, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Delete, URL))
            {
                var httpContext = new StringContent(groupContext, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                return await MSerializer.Client.SendAsync(request);
            }
        }
    }
}
