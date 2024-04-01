using RestSharp;

namespace WebService
{
    public class RequestFactory
    {
        public static RestRequest CreateGetRequest(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Get);
            return request;
        }

        public static RestRequest CreatePostRequest(string endpoint, object body)
        {
            var request = new RestRequest(endpoint, Method.Post);
            request.AddJsonBody(body);
            return request;
        }


    }
}
