using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.JiraApi.Request;
using RestSharpAutomation.JiraApi.Response;
using System;

namespace RestSharpAutomation.JiraApi
{
    [TestClass]
    public class TestJiraApi
    {
        private const string LoginEndPoint = "http://localhost:9191/rest/auth/1/session";

        [TestMethod]
        public void TestJirAApi()
        {
            JiraLogin jiraLogin = new JiraLogin()
            {
                username = "asimbera01",
                password = "Arthrex123"
            };

            IRestClient restClient = new RestClient() 
            {
                BaseUrl = new Uri("http://localhost:9191/")
            };

            IRestRequest restRequest = new RestRequest()
            {
                Resource = LoginEndPoint
            };

            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddJsonBody(jiraLogin);
            restRequest.AddHeader("Content-Type", "application/json");

            IRestResponse<LoginResponse> restResponse = restClient.Post<LoginResponse>(restRequest);
            Console.WriteLine(restResponse.Data);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
        }


    }
}
