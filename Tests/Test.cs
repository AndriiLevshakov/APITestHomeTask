using Models.RequestModels;
using Models.ResponseModels;
using WebService;
using static Core.Logger.LoggerManager;

[assembly: Parallelizable(ParallelScope.All)]
[assembly: LevelOfParallelism(2)]

namespace Tests
{
    public class Test
    {
        [SetUp]
        public void Setup()
        {
            Core.ConfigurationManager _configManager = new Core.ConfigurationManager();

            Logger.Info($"Starting {TestContext.CurrentContext.Test.MethodName}");
        }

        [Test]
        [Category("API")]
        public void Test1_ListOfUsersCanBeReceivedSuccessfully()
        {
            var users = RequestFactory.GetModel<List<UserModel>>();

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

            Logger.Info("Test successfully finished");
        }

        [Test]
        [Category("API")]
        public void Test2_ValidateResponseHeaderForListOfUsers()
        {
            var ContentTypeHeader = RequestFactory.GetContentTypeHeader();

            Assert.That(ContentTypeHeader != null);
            Logger.Info("Checked that Content-Type header is present");

            Assert.That(ContentTypeHeader.Value, Is.EqualTo("application/json; charset=utf-8"), "Incorrect Content-Type header value");
            Logger.Info("Checked that the value of content-type header is correct");

            Logger.Info("Test successfully finished");
        }

        [Test]
        [Category("API")]
        public void Test3_ValidateResponseForListOfUsers()
        {
            var users = RequestFactory.GetModel<List<UserModel>>();

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

            Logger.Info("Test successfully finished");
        }

        [Test]
        [Category("API")]
        public void Test4_UserCanBeCreatedSuccessfully()
        {
            var id = RequestFactory.PostModel<UserRequestModel>().Id;

            Assert.That(id, Is.Not.Null, "Response body does not contain 'Id' property");
            Logger.Info("Checked that response body contains property 'id'");

            Logger.Info("Test successfully finished");
        }

        [Test]
        [Category("API")]
        public void Test5_UserIsNotifiedIfResourceDoesNotExist()
        {
            var request = RequestFactory.GetStatusCodeFromInvalidEndpoint<UserModel>();

            Assert.That(request == System.Net.HttpStatusCode.NotFound);
            Logger.Info("Checked that Status Code is 'Not Found'");

            Logger.Info("Test successfully finished");
        }
    }
}