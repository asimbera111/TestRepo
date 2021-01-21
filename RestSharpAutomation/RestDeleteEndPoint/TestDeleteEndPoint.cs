using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.Helpers.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpAutomation.RestDeleteEndPoint
{
    [TestClass]
    public class TestDeleteEndPoint
    {
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private string deleteUrl = "http://localhost:8080/laptop-bag/webapi/api/delete/";
        private Random random = new Random();

        [TestMethod]
        public void TestDeleteEndPointusingRestSharp()
        {
            int id = random.Next(1000);
            string jsonData = "{" +
                            "\"BrandName\": \"Alienware\"," +
                            "\"Features\": {" +
                            "\"Feature\": [" +
                            "\"8th Generation Intel® Core™ i5-8300H\"," +
                            "\"Windows 10 Home 64-bit English\"," +
                            "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                            "\"8GB, 2x4GB, DDR4, 2666MHz\"" +
                            "]" +
                            "}," +
                            "\"Id\": " + id + "," +
                            "\"LaptopName\": \"Alienware M17\"" +
                            "}";

            Dictionary<string, string> httpHeader = new Dictionary<string, string>() 
            {
                { "Content-Type","Application/json"},
                { "Accept", "Application/json"}
            };
            RestClientHelper restClienthelper = new RestClientHelper();
            IRestResponse restResponse =  restClienthelper.PerformPostRequest(postUrl, httpHeader, jsonData, DataFormat.Json);
            Assert.AreEqual(200, (int)restResponse.StatusCode);

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(deleteUrl + id);
            restRequest.AddHeader("Accept", "*/*");
            IRestResponse restResponse1 = restClient.Delete(restRequest);
            Assert.AreEqual(200, (int)restResponse1.StatusCode);

            restResponse1 = restClient.Delete(restRequest);
            Assert.AreEqual(404, (int)restResponse1.StatusCode);
        }

        [TestMethod]
        public void TestDeleteEndPointusingRestSharp_HelperClass()
        {
            int id = random.Next(1000);
            string jsonData = "{" +
                            "\"BrandName\": \"Alienware\"," +
                            "\"Features\": {" +
                            "\"Feature\": [" +
                            "\"8th Generation Intel® Core™ i5-8300H\"," +
                            "\"Windows 10 Home 64-bit English\"," +
                            "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                            "\"8GB, 2x4GB, DDR4, 2666MHz\"" +
                            "]" +
                            "}," +
                            "\"Id\": " + id + "," +
                            "\"LaptopName\": \"Alienware M17\"" +
                            "}";

            Dictionary<string, string> httpHeader = new Dictionary<string, string>()
            {
                { "Content-Type","Application/json"},
                { "Accept", "Application/json"}
            };
            RestClientHelper restClienthelper = new RestClientHelper();
            IRestResponse restResponse = restClienthelper.PerformPostRequest(postUrl, httpHeader, jsonData, DataFormat.Json);
            Assert.AreEqual(200, (int)restResponse.StatusCode);

            httpHeader = new Dictionary<string, string>()
            {
                { "Accept","*/*"}                
            };
            
            IRestResponse restResponse1 = restClienthelper.PerformDeleteRequest(deleteUrl+id, httpHeader);
            Assert.AreEqual(200, (int)restResponse1.StatusCode);

            restResponse1 = restClienthelper.PerformDeleteRequest(deleteUrl + id, httpHeader);
            Assert.AreEqual(404, (int)restResponse1.StatusCode);
        }
    }

}
