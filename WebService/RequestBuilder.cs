using RestSharp;

namespace WebService
{
    public class RequestBuilder
    {
        private readonly RestRequest _request;

        public RequestBuilder()
        {
            _request = new RestRequest();
        }

        public RequestBuilder Get(string resource)
        {
            _request.Resource = resource;
            return this;
        }

        public RequestBuilder Post(string resource)
        {
            _request.Resource = resource;
            _request.Method = Method.Post;
            return this;
        }

        public RequestBuilder WithMethod(Method method)
        {
            _request.Method = method;
            return this;
        }

        public RequestBuilder WithBody(object body)
        {
            _request.AddJsonBody(body);
            return this;
        }

        public RestRequest Build()
        {
            return _request;
        }
    }
}
