using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using ApiFormat;

namespace ClientLibAPI
{
    class SerializerUser
    {
        const string URL = "https://splitlistwebapi.azurewebsites.net/api/Users/";

        //Gets a User with groups based on User Id
        public static async Task<UserDTO> GetUsersGroupByUserId(int UserId) 
        {
            var response = await MSerializer.Client.GetStringAsync($"{URL}{UserId}");
            var User = JsonConvert.DeserializeObject<UserDTO>(response);
            return User;
        }

        public static async Task<UserDTO> PostUserByUserDTO(UserDTO user)
        {
            var Context = JsonConvert.SerializeObject(user, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Post, URL))
            {

                var httpContext = new StringContent(Context, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                var response = await MSerializer.Client.SendAsync(request);
                var User = JsonConvert.DeserializeObject<UserDTO>(response.Content.ReadAsStringAsync().Result);
                return User;
            }
        }


        public static async Task<HttpResponseMessage> DeleteUser(UserDTO user)
        {
            var Context = JsonConvert.SerializeObject(user, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Delete, URL))
            {
                var httpContext = new StringContent(Context, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                return await MSerializer.Client.SendAsync(request);


            }

        }

    }
}
