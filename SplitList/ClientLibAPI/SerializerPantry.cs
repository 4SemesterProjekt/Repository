using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ApiFormat.Pantry;

namespace ClientLibAPI
{
    public static class SerializerPantry
    {
        const string URL = "https://splitlistwebapi.azurewebsites.net/api/Pantries/";

        // GET: api/Pantries/5
        // Return a PantryDTO based on PantryId
        public static async Task<PantryDTO> GetPantryById(int pantryId)
        {
            var pantryByIdString = await MSerializer.Client.GetStringAsync($"{URL}{pantryId}");
            var pantryById = JsonConvert.DeserializeObject<PantryDTO>(pantryByIdString);
            return pantryById;
        }

        // POST: api/Pantries
        // Creates Pantry from parameter
        public static async Task<PantryDTO> CreatePantry(PantryDTO pantry)
        {
            var pantryContext = JsonConvert.SerializeObject(pantry, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Post, URL))
            {

                var httpContext = new StringContent(pantryContext, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                var response = await MSerializer.Client.SendAsync(request);
                var createdPantry = JsonConvert.DeserializeObject<PantryDTO>(await response.Content.ReadAsStringAsync());
                return createdPantry;
            }
        }

        // POST: api/Pantries
        // Updates Pantry from parameter
        public static async Task<PantryDTO> UpdatePantry(PantryDTO pantry)
        {
            var pantryContext = JsonConvert.SerializeObject(pantry, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Put, URL))
            {
                var httpContext = new StringContent(pantryContext, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                var response = await MSerializer.Client.SendAsync(request);
                var updatedPantry = JsonConvert.DeserializeObject<PantryDTO>(await response.Content.ReadAsStringAsync());
                return updatedPantry;
            }
        }

        // DELETE: api/Pantries
        // If Id exists in DB remove entity with that Id
        // If Id does not exist, do nothing
        public static async Task<HttpResponseMessage> DeletePantry(PantryDTO pantry)
        {
            var pantryContext = JsonConvert.SerializeObject(pantry, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Delete, URL))
            {
                var httpContext = new StringContent(pantryContext, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                return await MSerializer.Client.SendAsync(request);
            }
        }
    }
}
