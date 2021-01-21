
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.Helpers.Request;
using System;
using System.Collections.Generic;
using System.Net;
using WebServiceAutomation.Model.Xml_Model;

namespace RestSharpAutomation.RestPostEndPoint
{
    [TestClass]
    public class TestPostEndPoint
    {
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private Random random = new Random();

        [TestMethod]
        public void TestPostUsingRestSharp()
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

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(postUrl);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", "application/xml");
            restRequest.AddJsonBody(jsonData);

            IRestResponse restResponse = restClient.Post(restRequest);
            HttpStatusCode statusCode = restResponse.StatusCode;
            Console.WriteLine("Staus code is :" + (int)statusCode); 
        }

        private Laptop GetLaptopObject()
        {
            Laptop laptop = new Laptop();
            laptop.BrandName = "Sample Brand name";
            laptop.LaptopName = "Sample laptop name";

            Features features = new Features();
            List<string> featureList = new List<string>()
            {
                ("Sample Feature")
            };

            features.Feature = featureList;
            laptop.Id = random.Next(1000);
            laptop.Features = features;
            return laptop;
        }

        [TestMethod]
        public void TestPostWithModelObject()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(postUrl);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", "application/xml");

            //When an object is passed as a body, restsharp is unable to convert it to JSON format.Hence need explicit deserializer
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddBody(GetLaptopObject());

            IRestResponse restResponse = restClient.Post(restRequest);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
        }

        [TestMethod]
        public void TestPostWithModelObject_HelperClass()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>() 
            {
                { "Content-Type","application/json"},
                { "Accept","application/xml"}
            };
            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse<Laptop> restResponse = restClientHelper.PerformPostRequest<Laptop>(postUrl, httpHeader, GetLaptopObject(), DataFormat.Json);            
            Assert.AreEqual(200, (int)restResponse.StatusCode);
        }

        [TestMethod]
        public void TestPostWithXmlUsingRestSharp()
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

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(postUrl);
            restRequest.AddHeader("Content-Type", "application/xml");
            restRequest.AddHeader("Accept", "application/xml");
            restRequest.AddParameter("xmlBody", xmlData, ParameterType.RequestBody);

            IRestResponse<Laptop> restResponse = restClient.Post<Laptop>(restRequest);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
        }

        [TestMethod]
        public void TestPostWithXmlUsingRestSharp_ModelObject()
        {           
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(postUrl);
            restRequest.AddHeader("Content-Type", "application/xml");
            restRequest.AddHeader("Accept", "application/xml");
            restRequest.RequestFormat = DataFormat.Xml;
            restRequest.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
            restRequest.AddParameter("xmlBody", restRequest.XmlSerializer.Serialize(GetLaptopObject()), ParameterType.RequestBody);

            IRestResponse<Laptop> restResponse = restClient.Post<Laptop>(restRequest);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
        }

        [TestMethod]
        public void TestPostWithXmlUsingRestSharp_HelperClass()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>()
            {
                {"Content-Type", "application/xml"},
                {"Accept", "application/xml"}
            };
            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse<Laptop> restResponse = restClientHelper.PerformPostRequest<Laptop>(postUrl, httpHeader, GetLaptopObject(), DataFormat.Xml);        
            Assert.AreEqual(200, (int)restResponse.StatusCode);
        }
    }
}
