using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Newtonsoft.Json;
using System.Threading.Tasks;
using DatabaseEntries;

namespace ClientAPI
{
    internal static class Program
    {
        private static readonly HttpClient client = new HttpClient();
        
        private static void Main(string[] args)
        {
            ClientAPI cAPI = new ClientAPI();
            var username = cAPI.Write();
            foreach(var name in username)
            {
                Console.WriteLine(name);
            }
        }
        
        public class ClientAPI
        {
            public List<string> Write()
            {
                var read_user = client.GetStringAsync("https://splitlistwebapi.azurewebsites.net/api/Users").Result;
                var users = JsonConvert.DeserializeObject<List<User>>(read_user);
                List<string> usernames = new List<string>();
                foreach(var user in users)
                {
                    usernames.Add(user.name);
                }
                return usernames;
            }
        }
    }
}