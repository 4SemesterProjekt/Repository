﻿using System;
using System.Collections.Generic;
using System.Text;
using ApiFormat;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ClientLibForAPI
{
    
    public class SerializerShoppingList
    {
        
         const string URL = "https://splitlistwebapi.azurewebsites.net/api/ShoppingLists/";

        //Return list of shoppinglistDTO based on GroupId
        public static async Task<List<ShoppingListDTO>> GetShoppingListByGroupId(int GroupId)
        {
            var ShoppinglistsByIDString = await MSerializer.Client.GetStringAsync($"{URL}group/{GroupId}");
            var ShoppinglistsByGroupID = JsonConvert.DeserializeObject<List<ShoppingListDTO>>(ShoppinglistsByIDString);
            return ShoppinglistsByGroupID;

        }
        //Return a shoppinglistDTO based on ShoppinglistId
        public static async Task<ShoppingListDTO> GetShoppingListByShoppinglistId(int ShoppinglistId)
        {
            var ShoppinglistsByIdString = await MSerializer.Client.GetStringAsync($"{URL}{ ShoppinglistId}");
            var ShoppinglistsByID = JsonConvert.DeserializeObject<ShoppingListDTO>(ShoppinglistsByIdString);
            return ShoppinglistsByID;


        }

        // POST: api/ShoppingLists
        // Updates/Creates shoppinglist from parameter.
        // if ID == 0(default) create new ShoppingListDTO
        // if ID exist database edit entity with that specific ID.
        public static async Task<ShoppingListDTO> PostShoppingList(ShoppingListDTO shoppingList)
        {
            var ShoppinglistContext =JsonConvert.SerializeObject(shoppingList, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Post, URL))
            {
                
                var httpContext = new StringContent(ShoppinglistContext, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                var response = await MSerializer.Client.SendAsync(request);
                var createdShoppinglist = JsonConvert.DeserializeObject<ShoppingListDTO>(response.Content.ReadAsStringAsync().Result);
                return createdShoppinglist;
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

                return await MSerializer.Client.SendAsync(request);
                

            }
        
        }



    }
}
