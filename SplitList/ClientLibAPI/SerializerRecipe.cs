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

        // GET: api/Recipes
        // Returns all recipes in the database as a list of RecipeDTOs
        public static async Task<List<RecipeDTO>> GetRecipes()
        {
            var recipeString = await MSerializer.Client.GetStringAsync($"{URL}");
            var recipe = JsonConvert.DeserializeObject<List<RecipeDTO>>(recipeString);
            return recipe;
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
