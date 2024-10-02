using Core;
using Models.RequestModels;
using Models.ResponseModels;
using Newtonsoft.Json;
using WebService;

[assembly: Parallelizable(ParallelScope.All)]
[assembly: LevelOfParallelism(2)]

namespace Tests
{
    public class Test
    {
        private ApiClient _apiClient;

        [SetUp]
        public void Setup()
        {
            string baseUrl = ConfigurationManager.BaseUrl;
            _apiClient = new ApiClient(baseUrl);
        }

        [Test]
        [Category("API")]
        public void Test1_ListOfUsersCanBeReceivedSuccessfully()
        {
            var response = _apiClient.GetUsers();

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "Status code is not 200 OK");

            Assert.That(!response.Content.Contains("error"), Is.True, "Response contains error message");

            var users = JsonConvert.DeserializeObject<List<UserModel>>(response.Content);
            Assert.That(users, Is.Not.Null.And.Not.Empty, "List of users is null or empty");

            foreach (var user in users)
            {
                Assert.That(user.Id, Is.GreaterThan(0), "User ID is null or empty");
                Assert.That(user.Name, Is.Not.Null.And.Not.Empty, "User name is null or empty");
                Assert.That(user.Username, Is.Not.Null.And.Not.Empty, "Username is null or empty");
                Assert.That(user.Email, Is.Not.Null.And.Not.Empty, "Email is null or empty");
                Assert.That(user.Address, Is.Not.Null, "Address is null");
                Assert.That(user.Phone, Is.Not.Null.And.Not.Empty, "Phone is null or empty");
                Assert.That(user.Website, Is.Not.Null.And.Not.Empty, "Website is null or empty");
                Assert.That(user.Company, Is.Not.Null, "Company is null");
            }
        }

        [Test]
        [Category("API")]
        public void Test2_ValidateResponseHeaderForListOfUsers()
        {
            var response = _apiClient.GetUsers();

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "Status cose is not 200 OK");

            var contentTypeHeaders = response.ContentHeaders
                .Where(h => h.Name.Contains("Content-Type"))
                .ToList();

            Assert.That(contentTypeHeaders, Is.Not.Empty, "Content-Type header is missing");

            var contentTypeHeader = contentTypeHeaders.FirstOrDefault();
            Assert.That(contentTypeHeader.Value, Is.EqualTo("application/json; charset=utf-8"), "Incorrect Content-Type header value");

            Assert.That(!response.Content.Contains("error"), Is.True, "Response contains error message");
        }

        [Test]
        [Category("API")]
        public void Test3_ValidateResponseForListOfUsers()
        {
            var response = _apiClient.GetUsers();

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "Status code is not 200 OK");

            var users = JsonConvert.DeserializeObject<List<UserModel>>(response.Content);

            Assert.That(users, Is.Not.Null, "List of users is null");
            Assert.That(users.Count, Is.EqualTo(10), "Expected 10 users in the list");

            HashSet<int?> userIds = new HashSet<int?>();
            foreach (var user in users)
            {
                Assert.That(userIds.Add(user.Id), Is.True, $"Duplicate user ID found: {user.Id}");
            }

            foreach (var user in users)
            {
                Assert.That(user.Name, Is.Not.Null.And.Not.Empty, "User name is null or empty");
                Assert.That(user.Username, Is.Not.Null.And.Not.Empty, "Username is null or empty");
            }

            foreach (var user in users)
            {
                Assert.That(user.Company, Is.Not.Null, "Company is null");
                Assert.That(user.Company.Name, Is.Not.Null.And.Not.Empty, "Company name is null or empty");
            }
        }

        [Test]
        [Category("API")]
        public void Test4_UserCanBeCreatedSuccessfully()
        {
            UserRequestModel newUser = new UserRequestModel()
            {
                Name = "John Doe",
                Username = "johndoe",
               
            };


            var response = _apiClient.PostUser(newUser);

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created), "Status code is not 201 Created");

            var createdUser = JsonConvert.DeserializeObject<UserModel>(response.Content);
            Assert.That(createdUser, Is.Not.Null, "Response content is null");
            Assert.That(createdUser.Id, Is.GreaterThan(0), "Response body does not contain 'Id' property");

            Assert.That(!response.Content.Contains("error"), Is.True, "Response contains error message");
        }

        [Test]
        [Category("API")]
        public void Test5_UserIsNotifiedIfResourceDoesNotExist()
        {
            var response = _apiClient.GetInvalidResource();

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound), "Status code is not 404 Not Found");

            Assert.That(!response.Content.Contains("error"), Is.True, "Response contains error message");
        }
    }
}