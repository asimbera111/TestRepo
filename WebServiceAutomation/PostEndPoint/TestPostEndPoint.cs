using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebServiceAutomation.Helpers.Authentication;
using WebServiceAutomation.Helpers.Request;
using WebServiceAutomation.Helpers.ResponseData;
using WebServiceAutomation.Model;
using WebServiceAutomation.Model.Json_Model;
using WebServiceAutomation.Model.Xml_Model;

namespace WebServiceAutomation.PostEndPoint
{
    [TestClass]
    public class TestPostEndPoint
    {
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/find/";
        private string secureGetUrl = "http://localhost:8080/laptop-bag/webapi/secure/find/";
        private string securePostUrl = "http://localhost:8080/laptop-bag/webapi/secure/add/";

        private RestResponse restResponse;
        private RestResponse restResponseForGet;
        private string jsonMediaType = "application/json";
        private string xmlMediaType = "application/xml";
        private Random random = new Random();
        
        [TestMethod]
        public void TestPostEndpointWithJson()
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
                            "\"Id\": "+ id +"," +
                            "\"LaptopName\": \"Alienware M17\"" +
                            "}";

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Accept", jsonMediaType);

                HttpContent httpContent = new StringContent(jsonData,Encoding.UTF8, jsonMediaType);
                Task<HttpResponseMessage> postResponse = httpClient.PostAsync(postUrl,httpContent);

                HttpStatusCode statusCode = postResponse.Result.StatusCode;
                HttpContent responseContent = postResponse.Result.Content;
                string responseData = responseContent.ReadAsStringAsync().Result;

                restResponse = new RestResponse((int)statusCode, responseData);
                Assert.AreEqual(200, restResponse.StatusCode);
                Assert.IsNotNull(restResponse.ResponseData,"Response data is null/empty");

                Task<HttpResponseMessage> getResponse = httpClient.GetAsync(getUrl + id);
                restResponseForGet = new RestResponse((int)getResponse.Result.StatusCode,
                                                    getResponse.Result.Content.ReadAsStringAsync().Result);
                Root root = JsonConvert.DeserializeObject<Root>(restResponseForGet.ResponseData);

                Assert.AreEqual(id, root.Id);
                Assert.AreEqual("Alienware", root.BrandName);
                Assert.AreEqual("Alienware M17", root.LaptopName);
                Assert.IsTrue(root.Features.Feature.Contains("8th Generation Intel® Core™ i5-8300H"));
            }
        }

        [TestMethod]
        public void TestPostEndpointWithXml()
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
                                "<Id>"+ id +"</Id>" +
                                "<LaptopName>Alienware M17</LaptopName>" +
                             "</Laptop>";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent content = new StringContent(xmlData, Encoding.UTF8, xmlMediaType);
                Task<HttpResponseMessage> httpResponseMessage = httpClient.PostAsync(postUrl, content);
                HttpStatusCode statusCode = httpResponseMessage.Result.StatusCode;
                HttpContent responseContent = httpResponseMessage.Result.Content;
                string responseData = responseContent.ReadAsStringAsync().Result;

                restResponse = new RestResponse((int)statusCode, responseData);
                Assert.AreEqual(200, restResponse.StatusCode);
                Assert.IsNotNull(restResponse.ResponseData, "Response data is null/empty");

                httpResponseMessage = httpClient.GetAsync(getUrl + id);
                if (!httpResponseMessage.Result.IsSuccessStatusCode)
                {
                    Assert.Fail("the Http response was not successfull");
                }

                restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Laptop));
                TextReader textReader = new StringReader(restResponse.ResponseData);
                Laptop xmlObj = (Laptop)xmlSerializer.Deserialize(textReader);

                Assert.AreEqual("Alienware", xmlObj.BrandName);
            }
        }

        [TestMethod]
        public void TestPostEndpointWithSendAsyncJson()
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
            using (HttpClient httpClient = new HttpClient())
            {                
                using (HttpRequestMessage httpRequest = new HttpRequestMessage())
                {
                    httpRequest.Method = HttpMethod.Post;
                    httpRequest.RequestUri = new Uri(postUrl);
                    httpRequest.Content = new StringContent(jsonData, Encoding.UTF8, jsonMediaType);
                    Task<HttpResponseMessage> httpResponseMessage= httpClient.SendAsync(httpRequest);

                    restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, 
                                                                httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
                    Assert.AreEqual(200, restResponse.StatusCode);
                }
            }
        }

        [TestMethod]
        public void TestPostEndpointWithSendAsyncXml()
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
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Accept", xmlMediaType);
                using (HttpRequestMessage httpRequest = new HttpRequestMessage())
                {
                    httpRequest.Method = HttpMethod.Post;
                    httpRequest.RequestUri = new Uri(postUrl);
                    httpRequest.Content = new StringContent(xmlData, Encoding.UTF8, xmlMediaType);
                    Task<HttpResponseMessage> httpResponseMessage = httpClient.SendAsync(httpRequest);

                    restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode,
                                                                httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
                    Assert.AreEqual(200, restResponse.StatusCode);
                }
            }
        }

        [TestMethod]
        public void TestPostEndPointWithHelperClass()
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
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", jsonMediaType);
            restResponse= HttpClientHelper.PerformPostRequest(postUrl, httpHeader, jsonData, jsonMediaType);
            Assert.AreEqual(200, restResponse.StatusCode);

            Laptop xmlData = ResponseDataHelper.DeserializeXmlResponse<Laptop>(restResponse.ResponseData);
            Console.WriteLine(xmlData.ToString());

        }

        [TestMethod]
        public void TestSecurePostEndPoint()
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
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", xmlMediaType);
            string authHeader = "Basic " + Base64StringConverter.GetBase64String("admin", "welcome");
            httpHeader.Add("Authorization", authHeader);

            restResponse = HttpClientHelper.PerformPostRequest(securePostUrl, httpHeader, xmlData, xmlMediaType);
            Assert.AreEqual(200, restResponse.StatusCode);

            Laptop xmlObj = ResponseDataHelper.DeserializeXmlResponse<Laptop>(restResponse.ResponseData);
            Console.WriteLine(xmlObj.ToString());
        }
    }
}
