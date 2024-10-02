using Models.RequestModels;
using RestSharp;
using static Core.Logger.LoggerManager;

namespace WebService
{
    public class ApiClient
    {
        private RestClient restClient;

        public ApiClient(string url)
        {
            restClient = new RestClient(url);
        }

        private const string UsersResource = "/users";
        private const string InvalidResource = "/invalidendpoint";

        public RestResponse GetUsers()
        {
            var request = new RequestBuilder()
                .Get(UsersResource)
                .Build();
              
            Logger.Info($"Executing request {UsersResource}");

            var response = restClient.Execute(request);
            Logger.Info($"Received response {response.Content}");

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Logger.Error($"Error fetching users: {response.StatusDescription}");
            }

            return response;
        }

        public RestResponse PostUser(UserRequestModel user)
        {
            var request = new RequestBuilder()
                .Post(UsersResource)
                .WithBody(user)
                .Build();

            Logger.Info("Executing request to create a user");
            var response = restClient.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                Logger.Error($"Error creating user: {response.StatusDescription}");
            }

            return response;
        }

        public RestResponse GetInvalidResource()
        {
            var request = new RequestBuilder()
                .Get(InvalidResource)
                .Build();

            Logger.Info("Executing request to an invalid endpoint");
            var response = restClient.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                Logger.Error($"Unexpected status code: {response.StatusDescription}");
            }

            return response;
        }
    }
}
