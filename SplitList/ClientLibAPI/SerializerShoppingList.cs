using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiFormat;
using Newtonsoft.Json;

namespace ClientLibAPI
{
    public static class SerializerShoppingList
    {
        static readonly HttpClient client = new HttpClient();
        const string URL = "https://splitlistwebapi.azurewebsites.net/api/ShoppingLists/";

        //Return list of shoppinglistDTO based on GroupId
        public static List<ShoppingListDTO> GetShoppingListByGroupId(int GroupId)
        {
            HttpResponseMessage response = client.GetAsync($"{URL}group/{GroupId}").Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            List<ShoppingListDTO> ShoppingListsByGroupID = JsonConvert.DeserializeObject<List<ShoppingListDTO>>(responseBody);
            return ShoppingListsByGroupID;
        }
        //Return a shoppinglistDTO based on ShoppinglistId
        public static ShoppingListDTO GetShoppingListByShoppingListId(int ShoppinglistId)
        {
            HttpResponseMessage response = client.GetAsync($"{URL}{ShoppinglistId}").Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                ShoppingListDTO ShoppingListItemsByID = JsonConvert.DeserializeObject<ShoppingListDTO>(responseBody);
                return ShoppingListItemsByID;
        }

        // POST: api/ShoppingLists
        // Updates/Creates shoppinglist from parameter.
        // if ID == 0(default) create new ShoppingListDTO
        // if ID exist database edit entity with that specific ID.
        public static HttpResponseMessage PostShoppingList(ShoppingListDTO shoppingList)
        {
            var ShoppinglistContext = JsonConvert.SerializeObject(shoppingList, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Post, URL))
            {
                var httpContext = new StringContent(ShoppinglistContext, Encoding.UTF8, "application/json");
                request.Content = httpContext;
                return client.SendAsync(request).Result;
            }
        }

        // DELETE: Delete element
        // If ID exist in DB remove entity with that ID
        // if ID doesnt exist, do notning. 
        public static async Task<HttpResponseMessage> DeleteShoppingList(ShoppingListDTO shoppingList)
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