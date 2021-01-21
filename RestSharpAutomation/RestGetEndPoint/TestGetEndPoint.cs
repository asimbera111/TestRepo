using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharp.Authenticators;
using RestSharpAutomation.Helpers.Request;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WebServiceAutomation.Model.Json_Model;
using WebServiceAutomation.Model.Xml_Model;

namespace RestSharpAutomation.RestGetEndPoint
{
    [TestClass]
    public class TestGetEndPoint
    {
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/all";
        private string secureGetUrl = "http://localhost:8080/laptop-bag/webapi/secure/all";

        [TestMethod]
        public void TestGetUsingRestSharp()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getUrl);
            IRestResponse restResponse =  restClient.Get(restRequest);
            HttpStatusCode statusCode = restResponse.StatusCode;
            Console.WriteLine("Staus code is :" + (int)statusCode);

            if (restResponse.IsSuccessful)
            {
                Console.WriteLine("Response content: " + restResponse.Content);
            }           

        }

        [TestMethod]
        public void TestGetInXmlFormat()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getUrl);
            restRequest.AddHeader("accept", "application/xml");
            IRestResponse restResponse = restClient.Get(restRequest);

            if (restResponse.IsSuccessful)
            {
                Console.WriteLine("Response code: " + (int)restResponse.StatusCode);
                Console.WriteLine("Response content: " + restResponse.Content);
            }
        }

        [TestMethod]
        public void TestGetInJsonFormat()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getUrl);
            restRequest.AddHeader("accept", "application/json");
            IRestResponse restResponse = restClient.Get(restRequest);

            if (restResponse.IsSuccessful)
            {
                Console.WriteLine("Response code: " + (int)restResponse.StatusCode);
                Console.WriteLine("Response content: " + restResponse.Content);
            }
        }

        [TestMethod]
        public void TestGetWithJson_Deserialize()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getUrl);
            restRequest.AddHeader("accept", "application/json");
            IRestResponse<List<Root>> restResponse = restClient.Get<List<Root>>(restRequest);

            if (restResponse.IsSuccessful)
            {
                Console.WriteLine("Response code: " + (int)restResponse.StatusCode);
                Assert.AreEqual(200, (int)restResponse.StatusCode);
                Console.WriteLine(restResponse.Data.Count);
                List<Root> data = restResponse.Data;

                Root obj = data.Find((x) =>
                {
                    return x.Id == 200;
                });

                Assert.AreEqual("Alienware M17", obj.LaptopName);
                Assert.IsTrue(obj.Features.Feature.Contains("Windows 10 Home 64-bit English"),"The feaure do not contain the value");
            }
            else 
            {
                Console.WriteLine("Error Msg : " + restResponse.ErrorMessage);
                Console.WriteLine("Stack trace : " + restResponse.ErrorException);
            }
        }

        [TestMethod]
        public void TestGetWithXml_Deserialize()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getUrl);
            restRequest.AddHeader("accept", "application/xml");
            //Deserialize an XML response
            var dotNetXmlDeserializer = new RestSharp.Deserializers.DotNetXmlDeserializer();
            IRestResponse restResponse = restClient.Get(restRequest);

            if (restResponse.IsSuccessful)
            {
                Console.WriteLine("Response code: " + (int)restResponse.StatusCode);
                Assert.AreEqual(200, (int)restResponse.StatusCode);                
                LaptopDetailss data = dotNetXmlDeserializer.Deserialize<LaptopDetailss>(restResponse);
                Assert.AreEqual("Alienware", data.Laptop.BrandName);
            }
            else
            {
                Console.WriteLine("Error Msg : " + restResponse.ErrorMessage);
                Console.WriteLine("Stack trace : " + restResponse.ErrorException);
            }
        }

        [TestMethod]
        public void TestGetWithExecute()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Method = Method.GET,
                Resource = getUrl
            };
            restRequest.AddHeader("accept", "application/json");
            IRestResponse<List<Laptop>> restResponse =  restClient.Execute<List<Laptop>>(restRequest);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Data, "Response is not null");
        }

        [TestMethod]
        public void TestGetWithXmlUsingHelperClass()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>()
            {
                { "Accept","application/xml"}
            };

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse restResponse = restClientHelper.PerformGetRequest(getUrl, httpHeader);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Content, "Content is null");

            IRestResponse<LaptopDetailss> restResponse1 = restClientHelper.PerformGetRequest<LaptopDetailss>(getUrl, httpHeader);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse1.Data, "Content is null");
        }

        [TestMethod]
        public void TestGetWithJsonUsingHelperClass()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>()
            {
                { "Accept","application/json"}
            };

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse restResponse = restClientHelper.PerformGetRequest(getUrl, httpHeader);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Content, "Content is null");

            IRestResponse<List<Laptop>> restResponse1 = restClientHelper.PerformGetRequest<List<Laptop>>(getUrl, httpHeader);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse1.Data, "Content is null");
        }

        [TestMethod]
        public void TestSecureGet()
        {
            IRestClient restClient = new RestClient();
            restClient.Authenticator = new HttpBasicAuthenticator("admin", "welcome");
            IRestRequest restRequest = new RestRequest()
            {
                Resource = secureGetUrl
            };

            IRestResponse restResponse = restClient.Get(restRequest);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
        }

    }
}
