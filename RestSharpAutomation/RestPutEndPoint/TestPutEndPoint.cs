using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.Helpers.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model.Json_Model;
using WebServiceAutomation.Model.Xml_Model;

namespace RestSharpAutomation.RestPutEndPoint
{
    [TestClass]
    public class TestPutEndPoint
    {
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/find/";
        private string putUrl = "http://localhost:8080/laptop-bag/webapi/api/update";
        private Random random = new Random();

        [TestMethod]
        public void TestPutWithJsonData()
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
                { "Content-Type", "application/json"},
                { "Accept", "application/xml"}
            };
            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse restResponse = restClientHelper.PerformPostRequest(postUrl, httpHeader, jsonData, DataFormat.Json);
            Assert.AreEqual(200, (int)restResponse.StatusCode);


            jsonData = "{" +
                            "\"BrandName\": \"Alienware\"," +
                            "\"Features\": {" +
                            "\"Feature\": [" +
                            "\"8th Generation Intel® Core™ i5-8300H\"," +
                            "\"Windows 10 Home 64-bit English\"," +
                            "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                            "\"8GB, 2x4GB, DDR4, 2666MHz\"," +
                            "\"1TB Of RAM\"" +
                            "]" +
                            "}," +
                            "\"Id\": " + id + "," +
                            "\"LaptopName\": \"Alienware M17\"" +
                            "}";

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest() 
            {
                Resource = putUrl
            };

            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddJsonBody(jsonData);

            IRestResponse<Root> putRestResponse = restClient.Put<Root>(restRequest);
            Assert.IsTrue(putRestResponse.Data.Features.Feature.Contains("1TB Of RAM"));

            Dictionary<string, string> getHttpHeadr = new Dictionary<string, string>() 
            {
                { "Accept","application/json"}
            };
            IRestResponse<Root> getRestResponse = restClientHelper.PerformGetRequest<Root>(getUrl + id, getHttpHeadr);
            Assert.AreEqual(200, (int)getRestResponse.StatusCode);
            Assert.IsTrue(getRestResponse.Data.Features.Feature.Contains("1TB Of RAM"));
        }

        [TestMethod]
        public void TestPutWithXmlData()
        {
            int id = random.Next(1000);
            string xmlData = "<Laptop>" +
                                    "<BrandName>Alienware</BrandName>" +
                                    "<Features>" +
                                        "<Feature>8th Generation IntelÂ® Coreâ„¢ i5-8300H</Feature>" +
                                        "<Feature>Windows 10 Home 64-bit English</Feature>" +
                                        "<Feature>NVIDIAÂ® GeForceÂ® GTX 1660 Ti 6GB GDDR6</Feature>" +
                                        "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                                    "</Features>" +
                                "<Id>" + id + "</Id>" +
                                "<LaptopName>Alienware M17</LaptopName>" +
                             "</Laptop>";

            Dictionary<string, string> httpHeader = new Dictionary<string, string>()
            {
                { "Content-Type", "application/xml"},
                { "Accept", "application/xml"}
            };
            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse restResponse = restClientHelper.PerformPostRequest(postUrl, httpHeader, xmlData, DataFormat.Xml);
            Assert.AreEqual(200, (int)restResponse.StatusCode);

            xmlData = "<Laptop>" +
                                    "<BrandName>Alienware</BrandName>" +
                                    "<Features>" +
                                        "<Feature>8th Generation IntelÂ® Coreâ„¢ i5-8300H</Feature>" +
                                        "<Feature>Windows 10 Home 64-bit English</Feature>" +
                                        "<Feature>NVIDIAÂ® GeForceÂ® GTX 1660 Ti 6GB GDDR6</Feature>" +
                                        "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                                        "<Feature>1TB of RAM</Feature>" +
                                    "</Features>" +
                                "<Id>" + id + "</Id>" +
                                "<LaptopName>Alienware M17</LaptopName>" +
                             "</Laptop>";

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Resource = putUrl
            };

            restRequest.AddHeader("Content-Type", "application/xml");
            restRequest.AddHeader("Accept", "application/xml");
            restRequest.RequestFormat = DataFormat.Xml;
            restRequest.AddParameter("xmlData",xmlData,ParameterType.RequestBody);

            IRestResponse putRestResponse = restClient.Put(restRequest);
            var deserializer = new RestSharp.Deserializers.DotNetXmlDeserializer();
            Laptop laptop =  deserializer.Deserialize<Laptop>(restResponse);
            Assert.AreEqual(laptop.Id, id);

            Dictionary<string, string> getHttpHeadr = new Dictionary<string, string>()
            {
                { "Accept","application/xml"}
            };
            IRestResponse<Laptop> getRestResponse = restClientHelper.PerformGetRequest<Laptop>(getUrl + id, getHttpHeadr);
            Assert.AreEqual(200, (int)getRestResponse.StatusCode);
            Assert.IsTrue(getRestResponse.Data.Features.Feature.Contains("1TB of RAM"));
        }

        [TestMethod]
        public void TestPutWithJsonData_HelperClass()
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
                { "Content-Type", "application/json"},
                { "Accept", "application/xml"}
            };
            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse restResponse = restClientHelper.PerformPostRequest(postUrl, httpHeader, jsonData, DataFormat.Json);
            Assert.AreEqual(200, (int)restResponse.StatusCode);


            jsonData = "{" +
                            "\"BrandName\": \"Alienware\"," +
                            "\"Features\": {" +
                            "\"Feature\": [" +
                            "\"8th Generation Intel® Core™ i5-8300H\"," +
                            "\"Windows 10 Home 64-bit English\"," +
                            "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                            "\"8GB, 2x4GB, DDR4, 2666MHz\"," +
                            "\"1TB Of RAM\"" +
                            "]" +
                            "}," +
                            "\"Id\": " + id + "," +
                            "\"LaptopName\": \"Alienware M17\"" +
                            "}";
            
            
            IRestResponse<Laptop> restResponse1 = restClientHelper.PerformPutRequest<Laptop>(putUrl, httpHeader, jsonData, DataFormat.Json);
            Assert.AreEqual(200, (int)restResponse.StatusCode);            
            Assert.IsTrue(restResponse1.Data.Features.Feature.Contains("1TB Of RAM"));

            Dictionary<string, string> getHttpHeadr = new Dictionary<string, string>()
            {
                { "Accept","application/json"}
            };
            IRestResponse<Root> getRestResponse = restClientHelper.PerformGetRequest<Root>(getUrl + id, getHttpHeadr);
            Assert.AreEqual(200, (int)getRestResponse.StatusCode);
            Assert.IsTrue(getRestResponse.Data.Features.Feature.Contains("1TB Of RAM"));
        }
    }
}
