using System;
using System.Collections.Generic;
using System.Text;
using ApiFormat;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
namespace ClientLibAPI
{
    public class SerializerGroups
    {
        const string URL = "https://splitlistwebapi.azurewebsites.net/api/groups/";


        //Get a group based on GroupId
        public static async Task<GroupDTO> GetGroupByGroupId(int GroupId)
        {
            var GroupByIDString = await MSerializer.Client.GetStringAsync($"{URL}{GroupId}");
            var GroupByGroupID = JsonConvert.DeserializeObject<GroupDTO>(GroupByIDString);
            return GroupByGroupID;
        }

        //Get a owner based on GroupId
        public static async Task<UserDTO> GetGroupOwnerByGroupId(int GroupId)
        {
            var GroupOwnerByGroupIdString = await MSerializer.Client.GetStringAsync($"{URL}{GroupId}/owner");
            var GroupOwner = JsonConvert.DeserializeObject<UserDTO>(GroupOwnerByGroupIdString);
            return GroupOwner;
        }

        //Post a group,
        //will update a group if it already exist
        //will create a group if it doesn't  exist
        //based on ID.
        public static async Task<GroupDTO> PostGroup(GroupDTO group)
        {
            var context = JsonConvert.SerializeObject(group, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Post, URL))
            {

                var httpContext = new StringContent(context, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                var response = await MSerializer.Client.SendAsync(request);
                var UpdatedGroup = JsonConvert.DeserializeObject<GroupDTO>(response.Content.ReadAsStringAsync().Result);
                return UpdatedGroup;
            }
        }

        //will Delete a group based on ID.
        public static async Task<HttpResponseMessage> DeleteGroup(GroupDTO group)
        {
            var Context = JsonConvert.SerializeObject(group, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Delete, URL))
            {
                var httpContext = new StringContent(Context, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                return await MSerializer.Client.SendAsync(request);


            }

        }
    }
}
