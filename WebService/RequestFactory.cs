using static Core.Logger.LoggerManager;
using Newtonsoft.Json;
using RestSharp;
using static Core.ConfigurationManager;
using System.Net;

namespace WebService
{
    public class RequestFactory
    {
            public static T GetModel<T>()
            {
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

            public static T PostModel<T>()
            {
                var client = new RestClient(BaseUrl);
                var request = new RestRequest(BaseUrl, Method.Post);
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
