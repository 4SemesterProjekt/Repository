using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace ClientLibForAPI
{
    public static class MSerializer
    {
        private static readonly HttpClient client = new HttpClient();
        public static HttpClient Client { get => client; }
    }
}
