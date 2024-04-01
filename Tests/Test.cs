using Models.ResponseModels;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using WebService;
using static Core.ConfigurationManager;
using static Core.Logger.LoggerManager;
using static Core.Utilities.ApiResponseHandler;

[assembly: Parallelizable(ParallelScope.All)]
[assembly: LevelOfParallelism(2)]

namespace Tests
{
    public class Test
    {
        private RestClient _restClient;

        [SetUp]
        public void Setup()
        {
            Core.ConfigurationManager _configManager = new Core.ConfigurationManager();

            RestClientBuilder clientBuilder = new RestClientBuilder();

            _restClient = clientBuilder.Build();

            Logger.Info($"Starting {TestContext.CurrentContext.Test.MethodName}");
        }

        [Test]
        [Category("API")]
        public void Test1_ListOfUsersCanBeReceivedSuccessfully()
        {
            var request = RequestFactory.CreateGetRequest(BaseUrl);

            var response = _restClient.Execute(request);
            Logger.Info("Sent request to API");

            Assert.That(response, Is.Not.Null, "Response is null");
            Logger.Info("Response is received");

            List<UserModel> users = GetUsers(response);
            Logger.Info("Deserializing Response");

            Assert.That(users, Is.Not.Null.And.Not.Empty, "List of users is null or empty");
            Logger.Info("Checked if the list of users is not null or empty");

            foreach (var user in users)
            {
                Assert.That(user.Id, Is.Not.Null, "User ID is null or empty");
                Assert.That(user.Name, Is.Not.Null.And.Not.Empty, "User name is null or empty");
                Assert.That(user.Username, Is.Not.Null.And.Not.Empty, "Username is null or empty");
                Assert.That(user.Email, Is.Not.Null.And.Not.Empty, "Email is null or empty");
                Assert.That(user.Address, Is.Not.Null, "Address is null");
                Assert.That(user.Phone, Is.Not.Null.And.Not.Empty, "Phone is null or empty");
                Assert.That(user.Website, Is.Not.Null.And.Not.Empty, "Website is null or empty");
                Assert.That(user.Company, Is.Not.Null, "Company is null");
            }
            Logger.Info("Checked that the List of users contains data: Id,  name, username, email, address, phone, website, company");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");

            Assert.That(!response.Content.Contains("error"), Is.True, "Response contains error message");
            Logger.Info("Checked that response does not contain errors");

            Logger.Info("Test successfully finished");
        }

        [Test]
        [Category("API")]
        public void Test2_ValidateResponseHeaderForListOfUsers()
        {
            var request = RequestFactory.CreateGetRequest(BaseUrl);

            var response = _restClient.Execute(request);
            Logger.Info("Sent request to API");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
            Logger.Info("Checked that response status code is OK");

            var contentTypeHeaders = response.ContentHeaders.Where(h => h.Name == "Content-Type").ToList();
            Assert.That(contentTypeHeaders, Is.Not.Empty, "Content-Type header is missing");
            Logger.Info("Checked that Content-Type header is present");

            var contentTypeHeader = contentTypeHeaders.FirstOrDefault();
            Assert.That(contentTypeHeader.Value, Is.EqualTo("application/json; charset=utf-8"), "Incorrect Content-Type header value");
            Logger.Info("Checked that the value of content-type header is correct");

            Assert.That(!response.Content.Contains("error"), Is.True, "Response contains error message");
            Logger.Info("Checked that response does not contain errors");

            Logger.Info("Test successfully finished");
        }

        [Test]
        [Category("API")]
        public void Test3_ValidateResponseForListOfUsers()
        {
            var request = RequestFactory.CreateGetRequest(BaseUrl);

            var response = _restClient.Execute(request);
            Logger.Info("Sent request to API");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
            Logger.Info("Checked that response status code is OK");

            List<UserModel> users = GetUsers(response);
            Logger.Info("Deserializing Response");

            Assert.That(users, Is.Not.Null, "List of users is null");
            Assert.That(users.Count, Is.EqualTo(10), "Expected 10 users in the list");
            Logger.Info("Checked that the list of users contains exactly 10 users");

            HashSet<int?> userIds = new HashSet<int?>();
            foreach (var user in users)
            {
                Assert.That(userIds.Add(user.Id), Is.True, $"Duplicate user ID found: {user.Id}");
            }
            Logger.Info("Checked that each user has a unique ID");

            foreach (var user in users)
            {
                Assert.That(user.Name, Is.Not.Null.And.Not.Empty, "User name is null or empty");
                Assert.That(user.Username, Is.Not.Null.And.Not.Empty, "Username is null or empty");
            }
            Logger.Info("Checked that each user has non-empty Name and Username");

            foreach (var user in users)
            {
                Assert.That(user.Company, Is.Not.Null, "Company is null");
                Assert.That(user.Company.Name, Is.Not.Null.And.Not.Empty, "Company name is null or empty");
            }
            Logger.Info("Checked that each user contains a Company with non-empty Name");

            Assert.That(!response.Content.Contains("error"), Is.True, "Response contains error message");
            Logger.Info("Checked that response does not contain errors");

            Logger.Info("Test successfully finished");
        }

        [Test]
        [Category("API")]
        public void Test4_UserCanBeCreatedSuccessfully()
        {
            var request = RequestFactory.CreatePostRequest(BaseUrl, RequestBody);

            var response = _restClient.Execute(request);
            Logger.Info("Sent request to API");

            Assert.That(response, Is.Not.Null, "Response is null");
            Logger.Info("Checked that response was received");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "Unexpected status code");
            Logger.Info("Checked that status code is 'created'");

            var responseBody = JObject.Parse(response.Content);

            Assert.That(responseBody["id"], Is.Not.Null, "Response body does not contain 'Id' property");
            Logger.Info("Checked that response body contains property 'id'");

            Assert.That(!response.Content.Contains("error"), Is.True, "Response contains error message");
            Logger.Info("Checked that response does not contain errors");

            Logger.Info("Test successfully finished");
        }

        [Test]
        [Category("API")]
        public void Test5_UserIsNotifiedIfResourceDoesNotExist()
        {
            var request = RequestFactory.CreateGetRequest(InvalidEndpoint);

            var response = _restClient.Execute(request);
            Logger.Info("Sent request to API");

            Assert.That(response, Is.Not.Null, "Response is null");
            Logger.Info("Checked that response is not null");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), "Unexpected status code");
            Logger.Info("Checked that status code is 'Not Found'");

            Assert.That(!response.Content.Contains("error"), Is.True, "Response contains error message");
            Logger.Info("Checked that response does not contain errors");

            Logger.Info("Test successfully finished");
        }
    }
}