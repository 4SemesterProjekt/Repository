using System;
using System.Collections.Generic;
using System.Text;
using ApiFormat;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ClientLibForAPI
{
    public class SerializerPantry
    {
        const string URL = "https://splitlistwebapi.azurewebsites.net/api/Pantries/";

        //Return list of PantryDTO based on GroupId
        public static async Task<PantryDTO> GetPantryByGroupId(int GroupId)
        {
            var PantriesByIDString = await MSerializer.Client.GetStringAsync($"{URL}group/{GroupId}");
            var PantriesByGroupID = JsonConvert.DeserializeObject<PantryDTO>(PantriesByIDString);
            return PantriesByGroupID;

        }
        

        // POST: api/Pantries
        // Updates/Creates Pantry from parameter.
        // if ID == 0(default) create new PantryDTO
        // if ID exist database edit entity with that specific ID.
        public static async Task<PantryDTO> PostPantry(PantryDTO pantry)
        {
            var PantryContext = JsonConvert.SerializeObject(pantry, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Post, URL))
            {

                var httpContext = new StringContent(PantryContext, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                var response = await MSerializer.Client.SendAsync(request);
                var UpdatedPantry = JsonConvert.DeserializeObject<PantryDTO>(response.Content.ReadAsStringAsync().Result);
                return UpdatedPantry;
            }
        }

        // DELETE: Delete element
        // If ID exist in DB remove entity with that ID
        // if ID doesnt exist, do notning. 
        public static async Task<HttpResponseMessage> DeletePantry(PantryDTO pantry)
        {
            var PantryContext = JsonConvert.SerializeObject(pantry, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Delete, URL))
            {
                var httpContext = new StringContent(PantryContext, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                return await MSerializer.Client.SendAsync(request);


            }

        }
    }
}
