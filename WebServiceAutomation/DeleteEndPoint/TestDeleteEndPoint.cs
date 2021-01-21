
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebServiceAutomation.Helpers.Authentication;
using WebServiceAutomation.Helpers.Request;
using WebServiceAutomation.Model;

namespace WebServiceAutomation.DeleteEndPoint
{
    [TestClass]
    public class TestDeleteEndPoint
    {
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/find/";
        private string deleteUrl = "http://localhost:8080/laptop-bag/webapi/api/delete/";
        private string secureGetUrl = "http://localhost:8080/laptop-bag/webapi/secure/find/";
        private string securePostUrl = "http://localhost:8080/laptop-bag/webapi/secure/add/";
        private string secureDeleteUrl = "http://localhost:8080/laptop-bag/webapi/secure/delete/";
        
        private RestResponse restResponse;
        private string jsonMediaType = "application/json";
        private string xmlMediaType = "application/xml";
        private Random random = new Random();

        [TestMethod]
        public void TestDeleteMethodUsingXml()
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

            Dictionary<string, string> httpHeaders = new Dictionary<string, string>()
            {
                { "Accept", xmlMediaType}
            };
            restResponse = HttpClientHelper.PerformPostRequest(postUrl, httpHeaders, xmlData, xmlMediaType);
            Assert.AreEqual(200, restResponse.StatusCode);

            using (HttpClient httpClient = new HttpClient())
            {
                Task<HttpResponseMessage> httpResponseMessage = httpClient.DeleteAsync(deleteUrl + id);
                restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode,
                                                httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(200, restResponse.StatusCode);
            }
            restResponse = HttpClientHelper.PerformGetRequest(getUrl + id, httpHeaders);
            Assert.AreNotEqual(200, restResponse.StatusCode);
        }

        [TestMethod]
        public void TestDeleteUsingHelperClass()
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

            Dictionary<string, string> httpHeaders = new Dictionary<string, string>()
            {
                { "Accept", xmlMediaType}
            };
            restResponse = HttpClientHelper.PerformPostRequest(postUrl, httpHeaders, xmlData, xmlMediaType);
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HttpClientHelper.PerformDeleteRequest(deleteUrl+ id);
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HttpClientHelper.PerformGetRequest(getUrl + id, httpHeaders);
            Assert.AreNotEqual(200, restResponse.StatusCode);
        }

        [TestMethod]
        public void TestSecureDeleteUsingHelperClass()
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

            string authHeader = "Basic " + Base64StringConverter.GetBase64String("admin", "welcome");
            Dictionary<string, string> httpHeaders = new Dictionary<string, string>()
            {
                { "Accept", xmlMediaType},
                { "Authorization", authHeader}
            };
            restResponse = HttpClientHelper.PerformPostRequest(securePostUrl, httpHeaders, xmlData, xmlMediaType);
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HttpClientHelper.PerformDeleteRequest(secureDeleteUrl + id, httpHeaders);
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HttpClientHelper.PerformGetRequest(secureGetUrl + id, httpHeaders);
            Assert.AreNotEqual(200, restResponse.StatusCode);
        }
    }
}
