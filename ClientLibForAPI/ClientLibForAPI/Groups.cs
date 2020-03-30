using System;
using DatabaseEntries;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace ClientLibForAPI
{
    public class Groups
    {
        private static readonly HttpClient client = new HttpClient();
        public List<Group> GetGroup() 
        {
            
            var grouplist = client.GetStringAsync("https://splitlistwebapi.azurewebsites.net/api/groups").Result;
            var test = JsonConvert.DeserializeObject<List<Group>>(grouplist);

            
            return test;
        }

    }
}
