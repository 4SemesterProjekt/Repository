using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using ApiFormat.User;

namespace ClientLibAPI
{
    public static class SerializerUser
    {
        const string URL = "https://splitlistwebapi.azurewebsites.net/api/Users/";

        // GET: api/Users/5
        // Return a UserDTO based on UserId
        public static async Task<UserDTO> GetUserById(string userId) 
        {
            var response = await MSerializer.Client.GetStringAsync($"{URL}{userId}");
            var user = JsonConvert.DeserializeObject<UserDTO>(response);
            return user;
        }

        // PUT: api/Users
        // Updates User from parameter
        public static async Task<UserDTO> UpdateUser(UserDTO user)
        {
            var userContext = JsonConvert.SerializeObject(user, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Put, URL))
            {
                var httpContext = new StringContent(userContext, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                var response = await MSerializer.Client.SendAsync(request);
                var updatedUser = JsonConvert.DeserializeObject<UserDTO>(await response.Content.ReadAsStringAsync());
                return updatedUser;
            }
        }

        // DELETE: api/Users
        // If Id exists in DB remove entity with that Id
        // if Id does not exist, do nothing
        public static async Task<HttpResponseMessage> DeleteUser(UserDTO user)
        {
            var userContext = JsonConvert.SerializeObject(user, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Delete, URL))
            {
                var httpContext = new StringContent(userContext, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                return await MSerializer.Client.SendAsync(request);
            }
        }
    }
}
