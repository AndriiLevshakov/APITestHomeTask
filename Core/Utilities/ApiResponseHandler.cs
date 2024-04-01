using Models.ResponseModels;
using Newtonsoft.Json;
using RestSharp;

namespace Core.Utilities
{
    public static class ApiResponseHandler
    {
        public static List<UserModel> GetUsers(RestResponse response)
        {
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<List<UserModel>>(response.Content);
        }
    }
}
