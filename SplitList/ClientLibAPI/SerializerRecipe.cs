using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ApiFormat.Recipe;

namespace ClientLibAPI
{
    public static class SerializerRecipe
    {
        const string URL = "https://splitlistwebapi.azurewebsites.net/api/Recipes/";

        // GET: api/Recipes/5, api/Recipes/6, api/Recipes/7
        // Returns a list of RecipeDTOs based on a number of RecipeIds
        public static async Task<List<RecipeDTO>> GetRecipesByIds(params int[] recipeIds)
        {
            var recipeContext = JsonConvert.SerializeObject(recipeIds, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Get, URL))
            {
                var httpContext = new StringContent(recipeContext, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                var response = await MSerializer.Client.SendAsync(request);
                var recipes = JsonConvert.DeserializeObject<List<RecipeDTO>>(await response.Content.ReadAsStringAsync());
                return recipes;
            }
        }

        // POST: api/Recipes
        // Creates Recipe from parameter
        public static async Task<RecipeDTO> CreateRecipe(RecipeDTO recipe)
        {
            var recipeContext = JsonConvert.SerializeObject(recipe, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Post, URL))
            {

                var httpContext = new StringContent(recipeContext, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                var response = await MSerializer.Client.SendAsync(request);
                var createdRecipe = JsonConvert.DeserializeObject<RecipeDTO>(await response.Content.ReadAsStringAsync());
                return createdRecipe;
            }
        }

        // POST: api/Recipes
        // Updates Recipe from parameter
        public static async Task<RecipeDTO> UpdateRecipe(RecipeDTO recipe)
        {
            var recipeContext = JsonConvert.SerializeObject(recipe, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Put, URL))
            {
                var httpContext = new StringContent(recipeContext, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                var response = await MSerializer.Client.SendAsync(request);
                var updatedRecipe = JsonConvert.DeserializeObject<RecipeDTO>(await response.Content.ReadAsStringAsync());
                return updatedRecipe;
            }
        }

        // DELETE: api/Recipes
        // If Id exists in DB remove entity with that Id
        // If Id does not exist, do nothing
        public static async Task<HttpResponseMessage> DeleteRecipe(RecipeDTO recipe)
        {
            var recipeContext = JsonConvert.SerializeObject(recipe, Formatting.None);
            using (var request = new HttpRequestMessage(HttpMethod.Delete, URL))
            {
                var httpContext = new StringContent(recipeContext, Encoding.UTF8, "application/json");
                request.Content = httpContext;

                return await MSerializer.Client.SendAsync(request);
            }
        }
    }
}
