using Models.ResponseModels;
using Newtonsoft.Json;
using RestSharp;
using static Core.ConfigurationManager;

namespace WebService
{
    public class RestClientBuilder
    {
        public RestClient Build()
        {
            return new RestClient(BaseUrl);
        }

        public List<UserModel> GetUsers(RestResponse response)
        {
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<List<UserModel>>(response.Content);
        }
    }
}
