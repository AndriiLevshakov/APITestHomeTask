using Models.ResponseModels;
using Newtonsoft.Json;
using RestSharp;

namespace Core.Utilities
{
    public static class ApiResponseHandler
    {
        public static List<T> GetUsers<T>(RestResponse response)
        {
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<List<T>>(response.Content);
        }
    }
}
