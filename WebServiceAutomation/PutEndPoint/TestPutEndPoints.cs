using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Helpers.Authentication;
using WebServiceAutomation.Helpers.Request;
using WebServiceAutomation.Helpers.ResponseData;
using WebServiceAutomation.Model;
using WebServiceAutomation.Model.Json_Model;
using WebServiceAutomation.Model.Xml_Model;

namespace WebServiceAutomation.PutEndPoint
{
    [TestClass]
    public class TestPutEndPoints
    {
        //Post to create a record
        //Put to update  the same record using Id
        //Get the same record using Id
        //validation

        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/find/";
        private string putUrl = "http://localhost:8080/laptop-bag/webapi/api/update";
        private string secureGetUrl = "http://localhost:8080/laptop-bag/webapi/secure/find/";
        private string securePostUrl = "http://localhost:8080/laptop-bag/webapi/secure/add/";
        private string securePutUrl = "http://localhost:8080/laptop-bag/webapi/secure/update/";
        
        private RestResponse restResponse;
        private string jsonMediaType = "application/json";
        private string xmlMediaType = "application/xml";
        private Random random = new Random();

        [TestMethod]
        public void TestPutMethodUsingXml()
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

            xmlData = "<Laptop>" +
                                    "<BrandName>Alienware</BrandName>" +
                                    "<Features>" +
                                        "<Feature>8th Generation IntelÂ® Coreâ„¢ i5-8300H</Feature>" +
                                        "<Feature>Windows 10 Home 64-bit English</Feature>" +
                                        "<Feature>NVIDIAÂ® GeForceÂ® GTX 1660 Ti 6GB GDDR6</Feature>" +
                                        "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                                        "<Feature>1TB of SSD</Feature>" +
                                    "</Features>" +
                                "<Id>" + id + "</Id>" +
                                "<LaptopName>Alienware M17</LaptopName>" +
                             "</Laptop>";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(xmlData, Encoding.UTF8, xmlMediaType);
                Task<HttpResponseMessage> httpResponseMessage = httpClient.PutAsync(putUrl, httpContent);
                restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode,
                                                httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(200, restResponse.StatusCode);
            }
            restResponse = HttpClientHelper.PerformGetRequest(getUrl + id, httpHeaders);
            Assert.AreEqual(200, restResponse.StatusCode);

            Laptop xmlObj =  ResponseDataHelper.DeserializeXmlResponse<Laptop>(restResponse.ResponseData);
            Assert.IsTrue(xmlObj.Features.Feature.Contains("1TB of SSD"),"Item not found");
           
        }

        [TestMethod]
        public void TestPutMethodUsingJson()
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
            Dictionary<string, string> httpHeaders = new Dictionary<string, string>()
            {
                { "Accept", jsonMediaType}
            };
            restResponse = HttpClientHelper.PerformPostRequest(postUrl, httpHeaders, jsonData, jsonMediaType);
            Assert.AreEqual(200, restResponse.StatusCode);

            jsonData = "{" +
                            "\"BrandName\": \"Alienware\"," +
                            "\"Features\": {" +
                            "\"Feature\": [" +
                            "\"8th Generation Intel® Core™ i5-8300H\"," +
                            "\"Windows 10 Home 64-bit English\"," +
                            "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                            "\"8GB, 2x4GB, DDR4, 2666MHz\"," +
                            "\"1TB of SSD\"" +
                            "]" +
                            "}," +
                            "\"Id\": " + id + "," +
                            "\"LaptopName\": \"Alienware M17\"" +
                            "}";
            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, jsonMediaType);
                Task<HttpResponseMessage> httpResponseMessage = httpClient.PutAsync(putUrl, httpContent);
                restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode,
                                                httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(200, restResponse.StatusCode);
            }
            restResponse = HttpClientHelper.PerformGetRequest(getUrl + id, httpHeaders);
            Assert.AreEqual(200, restResponse.StatusCode);

            Root jsonObj = ResponseDataHelper.DeserializeJsonResponse<Root>(restResponse.ResponseData);
            Assert.IsTrue(jsonObj.Features.Feature.Contains("1TB of SSD"), "Item not found");
        }

        [TestMethod]
        public void TestPutMethodUsingHelperClass_Json()
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
            Dictionary<string, string> httpHeaders = new Dictionary<string, string>()
            {
                { "Accept", jsonMediaType}
            };
            restResponse = HttpClientHelper.PerformPostRequest(postUrl, httpHeaders, jsonData, jsonMediaType);
            Assert.AreEqual(200, restResponse.StatusCode);

            jsonData = "{" +
                            "\"BrandName\": \"Alienware\"," +
                            "\"Features\": {" +
                            "\"Feature\": [" +
                            "\"8th Generation Intel® Core™ i5-8300H\"," +
                            "\"Windows 10 Home 64-bit English\"," +
                            "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                            "\"8GB, 2x4GB, DDR4, 2666MHz\"," +
                            "\"1TB of SSD\"" +
                            "]" +
                            "}," +
                            "\"Id\": " + id + "," +
                            "\"LaptopName\": \"Alienware M17\"" +
                            "}";
            restResponse = HttpClientHelper.PerformPutRequest(putUrl, httpHeaders, jsonData, jsonMediaType);
            Assert.AreEqual(200, restResponse.StatusCode);
            restResponse = HttpClientHelper.PerformGetRequest(getUrl + id, httpHeaders);
            Assert.AreEqual(200, restResponse.StatusCode);

            Root jsonObj = ResponseDataHelper.DeserializeJsonResponse<Root>(restResponse.ResponseData);
            Assert.IsTrue(jsonObj.Features.Feature.Contains("1TB of SSD"), "Item not found");
        }

        [TestMethod]
        public void TestPutMethodUsingHelperClass_Xml()
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

            xmlData = "<Laptop>" +
                                    "<BrandName>Alienware</BrandName>" +
                                    "<Features>" +
                                        "<Feature>8th Generation IntelÂ® Coreâ„¢ i5-8300H</Feature>" +
                                        "<Feature>Windows 10 Home 64-bit English</Feature>" +
                                        "<Feature>NVIDIAÂ® GeForceÂ® GTX 1660 Ti 6GB GDDR6</Feature>" +
                                        "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                                        "<Feature>1TB of SSD</Feature>" +
                                    "</Features>" +
                                "<Id>" + id + "</Id>" +
                                "<LaptopName>Alienware M17</LaptopName>" +
                             "</Laptop>";

            restResponse = HttpClientHelper.PerformPutRequest(putUrl, httpHeaders, xmlData, xmlMediaType);
            Assert.AreEqual(200, restResponse.StatusCode);
            restResponse = HttpClientHelper.PerformGetRequest(getUrl + id, httpHeaders);
            Assert.AreEqual(200, restResponse.StatusCode);

            Laptop xmlObj = ResponseDataHelper.DeserializeXmlResponse<Laptop>(restResponse.ResponseData);
            Assert.IsTrue(xmlObj.Features.Feature.Contains("1TB of SSD"), "Item not found");

        }

        [TestMethod]
        public void TestSecurePutEndPoint()
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

            xmlData = "<Laptop>" +
                                    "<BrandName>Alienware</BrandName>" +
                                    "<Features>" +
                                        "<Feature>8th Generation IntelÂ® Coreâ„¢ i5-8300H</Feature>" +
                                        "<Feature>Windows 10 Home 64-bit English</Feature>" +
                                        "<Feature>NVIDIAÂ® GeForceÂ® GTX 1660 Ti 6GB GDDR6</Feature>" +
                                        "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                                        "<Feature>1TB of SSD</Feature>" +
                                    "</Features>" +
                                "<Id>" + id + "</Id>" +
                                "<LaptopName>Alienware M17</LaptopName>" +
                             "</Laptop>";

            restResponse = HttpClientHelper.PerformPutRequest(securePutUrl, httpHeaders, xmlData, xmlMediaType);
            Assert.AreEqual(200, restResponse.StatusCode);
            restResponse = HttpClientHelper.PerformGetRequest(secureGetUrl + id, httpHeaders);
            Assert.AreEqual(200, restResponse.StatusCode);

            Laptop xmlObj = ResponseDataHelper.DeserializeXmlResponse<Laptop>(restResponse.ResponseData);
            Assert.IsTrue(xmlObj.Features.Feature.Contains("1TB of SSD"), "Item not found");
        }
    }    
}
