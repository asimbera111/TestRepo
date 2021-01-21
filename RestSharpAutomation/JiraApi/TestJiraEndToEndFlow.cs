using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.JiraApi.Request;
using RestSharpAutomation.JiraApi.Response;
using System;

namespace RestSharpAutomation.JiraApi
{
    [TestClass]
    public class TestJiraEndToEndFlow
    {
        private const string LoginEndPoint = "/rest/auth/1/session"; //requires GET 
        private const string LogoutEndPoint = "/rest/auth/1/session"; // requires DELETE
        private const string CreateProjectEndPoint = "/rest/api/2/project"; //requires POST
        private static IRestClient restClient;
        private static IRestResponse<LoginResponse> loginResponse;

        /**
         * 1. Login to JIRA -- Class initialize method
         * 2. Create a project --
         * 3. Logout -- Class CleanUp method
         **/

        [ClassInitialize]
        public static void Login(TestContext context)
        {
            restClient = new RestClient()
            {
                BaseUrl = new Uri("http://localhost:9191/")
            };

            IRestRequest restRequest = new RestRequest()
            {
                Resource = LoginEndPoint
            };

            JiraLogin jiraLogin = new JiraLogin()
            {
                username = "asimbera01",
                password = "Arthrex123"
            };
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddJsonBody(jiraLogin);
            loginResponse = restClient.Post<LoginResponse>(restRequest);
            Assert.AreEqual(200, (int)loginResponse.StatusCode);
        }

        [ClassCleanup]
        public static void Logout()
        {
            IRestRequest restRequest = new RestRequest()
            {
                Resource = LogoutEndPoint
            };

            restRequest.RequestFormat = DataFormat.Json;           
            restRequest.AddCookie(loginResponse.Data.session.name, loginResponse.Data.session.value);
            var response = restClient.Delete(restRequest);
            Assert.AreEqual(204, (int)response.StatusCode);
        }

        [TestMethod]
        public void CreateProject()
        {
            CreateProjectPayload createProjectPayload = new CreateProjectPayload();            
            IRestRequest restRequest = new RestRequest()
            {
                Resource = CreateProjectEndPoint
            };
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddBody(createProjectPayload);
            restRequest.AddCookie(loginResponse.Data.session.name, loginResponse.Data.session.value);

            var createProjectResponse = restClient.Post<CreateProjectResponse>(restRequest);
            Assert.AreEqual(201, (int)createProjectResponse.StatusCode);
        }
        
    }
}
