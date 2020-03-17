using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClientAPI
{
    internal static class Program
    {
        private static readonly HttpClient client = new HttpClient();
        
        private static async Task Main(string[] args)
        {
            var repositories = await ProcessRepositories();

            foreach (var user in repositories)
            {
                Console.WriteLine(user.Name);
            }
        }
        
        private static async Task<List<User>> ProcessRepositories()
        {
            var streamTask = client.GetStreamAsync("https://splitlistwebapi.azurewebsites.net/api/Users");
            var repositories = await JsonSerializer.DeserializeAsync<List<User>>(await streamTask);

            User user = new User(){Name = "Testing"};
            string output = JsonConvert.SerializeObject(user);
            
            return repositories;
        }
    }
}