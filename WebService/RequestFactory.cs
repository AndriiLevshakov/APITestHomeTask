using static Core.Logger.LoggerManager;
using Newtonsoft.Json;
using RestSharp;
// please do not use static usings like this, as where BaseUrl comes from is omitted in the class itself
using static Core.ConfigurationManager;
using System.Net;

namespace WebService
{
    public class RequestFactory
    {
        // you should redo requests in general
        // you should have a request builder class
        // which would allow you to build requests the following way
        // RestRequest request = new RequestBuilder()
        //         .Post(endpoint)
        //         .WithBody(body);
        //
        // Then you also need to have an apiClient
        // which would have a method with descriptive name that would send build the request and send it to the endpoint
        //
        // private const string UsersResource = "/users"; 
        //
        // public RestResponse GetUsers()
        // {
        //      
        //      var request = new RequestBuilder()
        //                               .Get(UsersResource)
        //      Logger.Info("Executing request")
        //      var response = client.Execute(request);
        //      return response;
        // }
        //
        // then in the test itself you should be verifying the response status code
        // you can write an extension method to simplify deserilization of body from the response object

        public static T GetModel<T>()
            {
            // formatting
                var client = new RestClient(BaseUrl);
                var request = new RestRequest(BaseUrl, Method.Get);
                var response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                Logger.Info("Checked that Status Code is OK");

                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
                else
                {
                    return default;
                }
            }

        public static RestSharp.HeaderParameter GetContentTypeHeader()
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest(BaseUrl, Method.Get);
            var response = client.Execute(request);

            return response.ContentHeaders.Where(h => h.Name == "Content-Type").First();
        }

        // formatting
            public static T PostModel<T>()
            {
                var client = new RestClient(BaseUrl);
                var request = new RestRequest(BaseUrl, Method.Post);
            // request body should not come from configuration
            // you should create it in the test
                request.AddJsonBody(RequestBody);
                var response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                Logger.Info("Checked that Status Code is 'Created'");

                return JsonConvert.DeserializeObject<T>(response.Content);
                }
                else
                {
                    return default;
                }
            }

        public static HttpStatusCode GetStatusCodeFromInvalidEndpoint<T>()
        {
            var client = new RestClient(InvalidEndpoint);
            var request = new RestRequest(InvalidEndpoint, Method.Get);
            var response = client.Execute(request);
            
            return response.StatusCode;
        }        
    }
}
