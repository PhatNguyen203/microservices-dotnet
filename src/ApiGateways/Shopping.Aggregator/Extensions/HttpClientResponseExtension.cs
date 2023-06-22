using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Extensions
{
    public static class HttpClientResponseExtension
    {
        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage responseMsg) where T : class
        {
            if (!responseMsg.IsSuccessStatusCode)
                throw new ApplicationException($"Something went wrong calling the API: { responseMsg.ReasonPhrase }");
            var data = await responseMsg.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(data, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
