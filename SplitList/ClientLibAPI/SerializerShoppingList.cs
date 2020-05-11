using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiFormat.ShoppingList;
using Newtonsoft.Json;

namespace ClientLibAPI
{
    
    public static class SerializerShoppingList
    {
        
        const string URL = "https://splitlistwebapi.azurewebsites.net/api/ShoppingLists/";

        // GET: api/ShoppingLists/5
        // Return a ShoppingListDTO based on ShoppingListId
        public static async Task<ShoppingListDTO> GetShoppingListById(int shoppingListId)
        {
            var shoppingListsByIdString = await MSerializer.Client.GetStringAsync($"{URL}{shoppingListId}");
            var shoppingListsById = JsonConvert.DeserializeObject<ShoppingListDTO>(shoppingListsByIdString);
            return shoppingListsById;
        }

        // POST: api/ShoppingLists
        // Creates ShoppingList from parameter
        public static async Task<ShoppingListDTO> CreateShoppingList(ShoppingListDTO shoppingList)
        {
            var shoppingListContext = JsonConvert.SerializeObject(shoppingList, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Post, URL))
            {
                var httpContext = new StringContent(shoppingListContext, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                var response = await MSerializer.Client.SendAsync(request);
                var createdShoppingList = JsonConvert.DeserializeObject<ShoppingListDTO>(await response.Content.ReadAsStringAsync());
                return createdShoppingList;
            }
        }

        // PUT: api/ShoppingLists
        // Updates ShoppingList from parameter
        public static async Task<ShoppingListDTO> UpdateShoppingList(ShoppingListDTO shoppingList)
        {
            var shoppingListContext = JsonConvert.SerializeObject(shoppingList, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Put, URL))
            {
                var httpContext = new StringContent(shoppingListContext, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                var response = await MSerializer.Client.SendAsync(request);
                var updatedShoppingList = JsonConvert.DeserializeObject<ShoppingListDTO>(await response.Content.ReadAsStringAsync());
                return updatedShoppingList;
            }
        }

        // DELETE: api/ShoppingLists
        // If Id exists in DB remove entity with that Id
        // if Id does not exist, do nothing
        public static async Task<HttpResponseMessage> DeleteShoppingList(ShoppingListDTO shoppingList)
        {
            var shoppingListContext = JsonConvert.SerializeObject(shoppingList, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Delete, URL))
            {
                var httpContext = new StringContent(shoppingListContext, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                return await MSerializer.Client.SendAsync(request);
            }
        }
    }
}