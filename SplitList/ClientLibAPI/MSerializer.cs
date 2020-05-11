using System.Net.Http;

namespace ClientLibAPI
{
    public static class MSerializer
    {
        private static readonly HttpClient client = new HttpClient();
        public static HttpClient Client => client;
    }
}
