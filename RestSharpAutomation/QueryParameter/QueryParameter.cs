using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using WebServiceAutomation.Model.Xml_Model;

namespace RestSharpAutomation.QueryParameter
{
    [TestClass]
    public class QueryParameter
    {
        private string searchUrl = "http://localhost:8080/laptop-bag/webapi/api/query";

        [TestMethod]
        public void TestQueryParameter()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest() 
            {
                Resource = searchUrl
            };

            restRequest.AddHeader("Accept", "Application/json");
            //restRequest.AddParameter("id", "1", ParameterType.QueryString);
            restRequest.AddQueryParameter("id", "1");
            restRequest.AddQueryParameter("LaptopName", "Alienware M17");

            IRestResponse<Laptop> restResponse = restClient.Get<Laptop>(restRequest);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
        }

    }
}
