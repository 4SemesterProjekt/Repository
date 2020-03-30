using System;
using System.Collections.Generic;
using System.Text;
using ApiFormat;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ClientLibForAPI
{
    class Shoppinglist
    {
        private static readonly HttpClient client = new HttpClient();
        const string URL = "https://splitlistwebapi.azurewebsites.net/api/ShoppingLists/";


        //Return list of shoppinglistDTO based on GroupId
        public async Task<List<ShoppingListDTO>> GetShoppingListByGroupId(int GroupId)
        {
            var ShoppinglistsByIDString = await client.GetStringAsync($"{URL}group/{GroupId}");
            var ShoppinglistsByGroupID = JsonConvert.DeserializeObject<List<ShoppingListDTO>>(ShoppinglistsByIDString);
            return ShoppinglistsByGroupID;

        }
        //Return a shoppinglistDTO based on ShoppinglistId
        public async Task<ShoppingListDTO> GetShoppingListByShoppinglistId(int ShoppinglistId)
        {
            var ShoppinglistsByIdString = await client.GetStringAsync($"{URL}{ ShoppinglistId}");
            var ShoppinglistsByID = JsonConvert.DeserializeObject<ShoppingListDTO>(ShoppinglistsByIdString);
            return ShoppinglistsByID;


        }

        // POST: api/ShoppingLists
        // Updates/Creates shoppinglist from parameter.
        // if ID == 0(default) create new ShoppingListDTO
        // if ID exist database edit entity with that specific ID.
        public async Task<HttpResponseMessage> PostShoppingList(ShoppingListDTO shoppingList)
        {
            var ShoppinglistContext =JsonConvert.SerializeObject(shoppingList, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Post, URL))
            {
                
                var httpContext = new StringContent(ShoppinglistContext, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                return await client.SendAsync(request);
                
                
            }
        }

        // DELETE: Delete element
        // If ID exist in DB remove entity with that ID
        // if ID doesnt exist, do notning. 
        public async Task<HttpResponseMessage> DeleteShoppingList(ShoppingListDTO shoppingList) 
        {
            var ShoppinglistContext = JsonConvert.SerializeObject(shoppingList, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Delete, URL)) 
            {
                var httpContext = new StringContent(ShoppinglistContext, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                return await client.SendAsync(request);
               

            }
        
        }



    }
}
